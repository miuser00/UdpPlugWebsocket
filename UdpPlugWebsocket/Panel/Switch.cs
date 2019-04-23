using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Miuser.NUDP.Sockets;

namespace UdpPlugWebsocket
{
    public class Switch
    {
        //存储以ID为
        public List<NODE> nodes;
        
        public Switch()
        {
            nodes = new List<NODE>();
        }
        /// <summary>
        /// 接收来自UDP的数据并进行转发
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="EndpointString"></param>
        public void SendFromUDP(byte[] bytes, string EndpointString)
        {
            HandleUDPRsvMsg?.Invoke(bytes, EndpointString);
            string content = System.Text.Encoding.Default.GetString(bytes);
            string ID = "";
            if (content.Length >= 19)
            {
                ID = content.Substring(9, 10);
                NODE node = GetTheNode((x =>
                  {
                      var Id = (string)x.ID;
                      return Id == ID;
                  }));
                //如果不存在则新建一个NODE
                if (node == null)
                {
                    node = new NODE(ID)
                    {
                        //连接Node消息到Switch上
                        HandleNodeClosed = HandleNodeClosed == null ? null : new Action<NODE>((nod) => { HandleNodeClosed(this, nod); }),
                        HandleNodeCreated = HandleNodeCreated == null ? null : new Action<NODE>((nod) => { HandleNodeCreated(this, nod); }),
                        HandleUDPAdded = new Action<string>(target => 
                        {
                            HandleNodeChanged?.Invoke(this, node);
                        }),
                        HandleUDPRemoved = new Action<string>(target =>
                        {
                            HandleNodeChanged?.Invoke(this, node);
                        }),
                        HandleWebSocketAdded = new Action<string>(target =>
                        {
                            HandleNodeChanged?.Invoke(this, node);
                        }),
                         HandleWebSocketRemoved= new Action<string>(target =>
                        {
                            HandleNodeChanged?.Invoke(this, node);
                        }),
                        HandleException=new Action<Exception>(ex=>
                        {
                            HandleException(ex);
                        })
                    };
                    node.ID = ID;
                    AddNode(node);
                }

                //判断连接是否为新连接，如果是则增加消息源
                if (node.UDPConnections.Where(x => { return EndpointString == x.EndpointString; }).FirstOrDefault() == null)
                {
                    node.AddUDPConnection(EndpointString);
                }
                else
                {
                    node.UDPConnections.Where(x => { return EndpointString == x.EndpointString; }).FirstOrDefault().Reactive();
                }
                node.Reactive();
                //分发消息给所有在线的Websocket端口
                lock (node.WebsocketConnections)
                {
                    foreach (WSConnection ws in node.WebsocketConnections.ToArray())
                    {
                        HandleWebSocketSendMsg?.Invoke(bytes, ws.EndpointString);
                    }
                }
                //分发消息给所有在线的UDP端口
                lock (node.UDPConnections)
                {
                    foreach (UDPConnection udp in node.UDPConnections.ToArray())
                    {
                        HandleUDPSendMsg?.Invoke(bytes, udp.EndpointString);
                    }
                }
            }

        }

        public void SendFromWebSocket(byte[] bytes, string EndpointString)
        {
            HandleWebsocketRsvMsg?.Invoke(bytes, EndpointString);
            string content = System.Text.Encoding.Default.GetString(bytes);
            string ID = "";
            if (content.Length >= 19)
            {
                ID = content.Substring(9, 10);
                NODE node = GetTheNode((x =>
                {
                    var Id = (string)x.ID;
                    return Id == ID;
                }));
                //如果不存在则新建一个NODE
                if (node == null)
                {
                    node = new NODE(ID)
                    {
                        //连接Node消息到Switch上
                        HandleNodeClosed = HandleNodeClosed == null ? null : new Action<NODE>((nod) => { HandleNodeClosed(this, nod); }),
                        HandleNodeCreated = HandleNodeCreated == null ? null : new Action<NODE>((nod) => { HandleNodeCreated(this, nod); }),
                        HandleUDPAdded = new Action<string>(target => 
                        {
                            HandleNodeChanged?.Invoke(this, node);
                        }),
                        HandleUDPRemoved = new Action<string>(target =>
                        {
                            HandleNodeChanged?.Invoke(this, node);
                        }),
                        HandleWebSocketAdded = new Action<string>(target =>
                        {
                            HandleNodeChanged?.Invoke(this, node);
                        }),
                         HandleWebSocketRemoved= new Action<string>(target =>
                        {
                            HandleNodeChanged?.Invoke(this, node);
                        }),
                        HandleException=new Action<Exception>(ex=>
                        {
                            HandleException(ex);
                        })

                    };
                    node.ID = ID;
                    AddNode(node);

                }
                
                //判断连接是否为新连接，如果是则增加消息源
                if (node.WebsocketConnections.Where(x => { return EndpointString == x.EndpointString; }).FirstOrDefault() == null)
                {
                    node.AddWebsocketConnection(EndpointString);
                }else
                {
                    node.WebsocketConnections.Where(x => { return EndpointString == x.EndpointString; }).FirstOrDefault().Reactive();
                }
                node.Reactive();

                //分发消息给所有在线的Websocket端口
                if (node.WebsocketConnections != null)
                {
                    lock (node.WebsocketConnections)
                    {
                        foreach (WSConnection ws in node.WebsocketConnections.ToArray())
                        {
                            HandleWebSocketSendMsg?.Invoke(bytes, ws.EndpointString);
                        }
                    }
                }
                //分发消息给所有在线的UDP端口
                if (node.UDPConnections != null)
                {
                    lock (node.UDPConnections)
                    {
                        foreach (UDPConnection udp in node.UDPConnections.ToArray())
                        {
                            HandleUDPSendMsg?.Invoke(bytes, udp.EndpointString);
                        }
                    }
                }
            }

        }

        #region 内部函数
        private void RemoveNode(NODE node)
        {
            RWLock_ClientList.EnterWriteLock();
            try
            {
                nodes.Remove(node);
            }
            finally
            {
                RWLock_ClientList.ExitWriteLock();
            }
        }
        /// <summary>
        /// 删除不活动的客户端连接
        /// </summary>
        /// <param name="theConnection">指定的客户端连接</param>
        private void RemoveInactiveNodes()
        {
            List<NODE> nodes = new List<NODE>();
            foreach (NODE node in nodes.ToArray())
            {
                if (node.IsActive == false)
                {
                    nodes.Add(node);
                }
            }
            foreach (NODE node in nodes.ToArray())
            {
                RemoveInactiveNode(node);
            }
        }
        /// <summary>
        /// 检测该客户端，如果不活动则删除连接
        /// </summary>
        /// <param name="theConnection">指定的客户端连接</param>
        private void RemoveInactiveNode(NODE node)
        {
            if (node.IsActive == false)
            {
                RemoveNode(node);
            }
        }

        /// <summary>
        /// 当某连接不活动超时激活该函数
        /// </summary>
        private void HandleConnClientClose(NODE node)
        {
            RemoveNode(node);
        }

        #endregion


        /// <summary>
        /// 维护客户端列表的读写锁
        /// </summary>
        public ReaderWriterLockSlim RWLock_ClientList { get; } = new ReaderWriterLockSlim();

        /// <summary>
        /// 关闭指定客户端连接
        /// </summary>
        /// <param name="theConnection">指定的客户端连接</param>
        public void CloseNode(NODE node)
        {
            RemoveNode(node);
            //调用外部回调函数通知连接被关闭
            HandleNodeClosed?.Invoke(this,node);
        }

        /// <summary>
        /// 添加客户端连接
        /// </summary>
        /// <param name="theConnection">需要添加的客户端连接</param>
        public enum Source {UDPPort,WebSocket }
        public void AddNode(NODE node)
        {
            RWLock_ClientList.EnterWriteLock();

            try
            {
                nodes.Add(node);
                node.HandleNodeClosed = new Action<NODE>((id => { nodes.Remove(id); }));

            }
            finally
            {
                RWLock_ClientList.ExitWriteLock();
                //调用外部回调函数通知新连接建立
                HandleNodeCreated?.Invoke(this, node);
            }
        }

        /// <summary>
        /// 通过条件获取客户端连接列表
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns></returns>
        public IEnumerable<NODE> GetNodeList(Func<NODE, bool> predicate)
        {

            //RemoveInactiveNodes();
            IEnumerable<NODE> ret;
            lock (nodes)
            {
                ret = nodes.Where(predicate);
            }
            return ret;
        }

        /// <summary>
        /// 获取所有客户端连接列表
        /// </summary>
        /// <returns></returns>
        public List<NODE> GetNodeList()
        {
            //RemoveInactiveNodes();
            return nodes;
        }

        /// <summary>
        /// 寻找特定条件的客户端连接
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns></returns>
        public NODE GetTheNode(Func<NODE, bool> predicate)
        {
            NODE node;
            lock (nodes)
            {
                node = nodes.Where(predicate).FirstOrDefault();
            }
            if (node == null) return null;
            //RemoveInactiveNode(node);

            if (node.IsActive) return node;
            return null;
        }

        /// <summary>
        /// 获取客户端连接数
        /// </summary>
        /// <returns></returns>
        public int GetConnectionCount()
        {
            int ret;
            lock (nodes)
            {
                ret = nodes.Count;
            }
            return ret;

        }


        #region UDP客户端连接事件

        /// <summary>
        //  从UDP端口接收到消息
        /// </summary>
        public Action<byte[], string> HandleUDPRsvMsg { get; set; }

        /// <summary>
        //  转发消息给UDP端口
        /// </summary>
        public Action<byte[], string> HandleUDPSendMsg { get; set; }

        #endregion

        #region Websocket客户端连接事件

        /// <summary>
        //  从Websocket端口接收到消息
        /// </summary>
        public Action<byte[], string> HandleWebsocketRsvMsg { get; set; }

        /// <summary>
        /// 转发消息给Websocket端口
        /// </summary>
        public Action<byte[], string> HandleWebSocketSendMsg { get; set; }
        #endregion

        #region 客户端连接事件
        /// <summary>
        /// 当新NODE建立后执行
        /// </summary>
        public Action<Switch,NODE> HandleNodeCreated { get; set; }
        /// <summary>
        /// NODE关闭后回调
        /// </summary>
        public Action<Switch,NODE> HandleNodeClosed { get; set; }
        /// <summary>
        /// 异常处理程序
        /// </summary>
        public Action<Exception> HandleException { get; set; }
        /// <summary>
        /// 节点状态发生变化
        /// </summary>
        public Action<Switch, NODE> HandleNodeChanged { get; set; }
        #endregion



    }
}
