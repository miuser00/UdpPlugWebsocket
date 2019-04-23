using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Miuser.NUDP.Sockets
{
    #region
    /// <summary>
    /// 异常处理程序
    /// </summary>
    public class SocketExceptionEventArgs : EventArgs { public Exception Exception; public EndPoint Remote; }
    /// <summary>
    /// 服务启动后执行
    /// </summary>
    public class ServerStartedEventArgs : EventArgs { public EndPoint Server; }
    /// <summary>
    /// 服务端关闭客户端后执行
    /// </summary>
    public class ServerClosedEventArgs : EventArgs { public EndPoint Server; }
    /// <summary>
    /// 当新客户端连接后执行
    /// </summary>
    public class NewClientEventArgs : EventArgs { public EndPoint Remote; }
    
    /// <summary>
    /// 客户端连接关闭后回调
    /// </summary>
    public class ClientClosedEventArgs : EventArgs { public EndPoint Remote; }
    /// <summary>
    /// 客户端连接接受新的消息后调用
    /// </summary>
    public class MsgReceivedEventArgs : EventArgs { public byte[] Received; public EndPoint Remote; }
    /// <summary>
    /// 客户端连接发送消息后回调
    /// </summary>
    public class MsgSendEventArgs : EventArgs { public byte[] Received; public EndPoint Remote; }

    #endregion

}
