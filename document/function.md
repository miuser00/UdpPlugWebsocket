## 工作原理 ##
本项目仿照硬件设计思想，基于模块化原理开发，分为UDP连接池、Websocket连接池、交换面板三个大的模块。各个模块相互独立，除消息通讯外无模块间耦合。每个模块的功能部分和UI部分彼此亦基本独立。UDP和Websocket两个模块采用.net内置的Action对象与交换面板进行异步消息通讯。

**Endpoint是外部连接的唯一索引。**

**Endpoint字符串的格式为："XXX.XXX.XXX.XXX:XXXX" 前四组三位数为IP地址，最后一组四位数为远程端口号**

UDP连接池是一个UDP连接管理模块，默认开放7101端口，供外部连接接入。每个成功连接的设备或模拟程序会被分配一个SocketConnection，SocketConnction通过Tag属性进行索引（Tag的值默认为Endpoint）并添加到一个列表里，该列表可以通过 GetConnectionList()获得，也可以使用GetTheConnection(Func<SocketConnection, bool> predicate)函数通过LINQ语句进行指定连接的查询。 



以下为UDP连接池对外暴露的函数接口


        /// 开始服务，监听客户端
        public void StartServer()
        ///按照IP地址和端口发送字符串
        public void Send(string EndpointString, byte[] bytes)
        ///开始异步监听端口
        public void Listening()
        /// 关闭指定客户端连接
        public void CloseConnection(SocketConnection theConnection)
        /// 添加客户端连接
        public void AddConnection(SocketConnection theConnection)
        /// 通过条件获取客户端连接列表
        public IEnumerable<SocketConnection> GetConnectionList(Func<SocketConnection, bool> predicate)
        /// 获取所有客户端连接列表
        public IEnumerable<SocketConnection> GetConnectionList()
        /// 寻找特定条件的客户端连接
        public SocketConnection GetTheConnection(Func<SocketConnection, bool> predicate)
        /// 获取客户端连接数
        public int GetConnectionCount()

以下为UDP连接池对外暴露的事件接口

        /// 服务启动后执行
        public Action<SocketServer> HandleServerStarted { get; set; }
        /// 当新客户端连接后执行
        public Action<SocketServer, SocketConnection> HandleNewClientConnected { get; set; }
        /// 客户端连接接受新的消息后调用
        public Action<byte[], SocketConnection, SocketServer> HandleRecMsg { get; set; }
        /// 客户端连接发送消息后回调
        public Action<byte[], SocketConnection, SocketServer> HandleSendMsg { get; set; }
        /// 客户端连接关闭后回调
        public Action<SocketConnection, SocketServer> HandleClientClose { get; set; }
        /// 异常处理程序
        public Action<Exception> HandleException { get; set; }

Websocket连接池是一个websocket管理模块，默认开放9000端口，供外部连接接入。每个成功联机的APP页面会分配一个唯一的session，Websocket对象维护了一个Sessions列表，该列表可以通过Where表达式，通过LINQ语句筛选。

以下为Websocket连接池对外暴露的函数接口

        /// 发送字节
        public void Send(string EndpointString,byte[] bytes)

以下为Websocket连接池对外暴露的事件接口

        /// 当新客户端连接后执行
        public Action<WebSocketSharp.Net.WebSockets.WebSocketContext> HandleNewWebSocketClientConnected { get; set; }
        /// 客户端连接接受新的消息后调用
        public Action<byte[], WebSocketSharp.Net.WebSockets.WebSocketContext> HandleWebSocketRecMsg { get; set; }
        /// 客户端连接发送消息后回调
        public Action<byte[], WebSocketSharp.Net.WebSockets.WebSocketContext> HandleWebSocketSendMsg { get; set; }
        /// 客户端连接关闭后回调
        public Action<WebSocketSharp.Net.WebSockets.WebSocketContext> HandleWebSocketClientClose { get; set; }

交换面板（Switch）是一个消息转发器，他接收UDP与Websocket传入的数据包，并根据数据包中ID部分对信息进行分组转发，既收到信息后，向ID相同的全部在线端口转发收到的原始消息。MM是标识设备控制权限的，供上层应用使用，服务端不做处理。

对于每一个ID，Switch会生成一个结点（Node），该节点存储了含有相同ID的Websocket和UDP连接信息 1）TTL（剩余生存时间）2 Endpoint（客户连接端口），每个结点自带生命计时器，靠上层程序定期执行Reactive()函数保活。

        private void Init_Forms()
        {
            try
            {
				...
	
                //连接模块与系统终端窗口
                Device.Instance.HandleMessage += new Action<string>(MessageForm.Log);
                Device.Instance.HandleError += new Action<string>(ErrorForm.Log);

                Browser.Instance.HandleMessage += new Action<string>(MessageForm.Log);
                Browser.Instance.HandleError += new Action<string>(ErrorForm.Log);
                //连接Pannel数据输入端   Device->Panel
                Device.Instance.server.HandleRecMsg += new Action<byte[], Miuser.NUDP.Sockets.SocketConnection, Miuser.NUDP.Sockets.SocketServer>((bytes, conn, server) =>
                {
                    Panel.Instance.sw.SendFromUDP(bytes, conn.Tag.ToString());
                });
                //连接Pannel数据输入端   Browser->Panel
                Browser.Instance.HandleWebSocketRecMsg += new Action<byte[], WebSocketSharp.Net.WebSockets.WebSocketContext>((bytes, context) =>
                {
                    Panel.Instance.sw.SendFromWebSocket(bytes, context.UserEndPoint.ToString());
                });
                //连接Pannel数据输出端   Panel->Device
                Panel.Instance.sw.HandleUDPSendMsg += new Action<byte[], string>((bytes, endpointString) =>
                {
                    Device.Instance.server.Send(endpointString, bytes);
                });
                //连接Panel数据输出端  Panel->Browser
                Panel.Instance.sw.HandleWebSocketSendMsg += new Action<byte[], string>((bytes, endpointString)=>
                {
                    Browser.Instance.Send(endpointString, bytes);
                });

				...
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace,"系统模块初始化错误");
                
            }
        }
