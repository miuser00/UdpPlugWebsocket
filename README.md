## 版本： ##
### Ver 0.22 ###

## 项目说明  ##




通过手机APP终端借助一台中间服务器连接到物联网硬件设备，这是一个非常一般性的用户需求。 目前比较流行的的阿里云等MQTT系物联网协议虽然技术先进，但进入门槛较高，用户也使用相对较为复杂，未来的成本也不容乐观。 因此出于兴趣爱好决定自己搞一个这样的服务器程序项目。项目使用自己比较熟悉的.Net技术，基于C#编写。 未来也有可能会迁移到Linux环境下。

## 项目功能 ##
这是一个独立的可执行Win32服务端程序，用于桥接终端APP和物联网硬件。 APP端为Websocet接口的静态html页面，硬件端为基于合宙Luat2G通讯模块。 模块与服务器通讯使用UDP协议通讯。 基本通讯方式如下： Html5页面通过Webscoket接口发送一个自定义的字符串报文给服务端，服务端根据报文中的ID把报文转发给相同ID的UDP硬件设备（或UDP测试程序）

与UPWS相适配的基于合宙Air系列2G模块的开源硬件在以下地址

  https://github.com/miuser00/V3R_DemoBoard

为方便您的使用，本Respository提供了一个C#编写的简易UDP测试程序用来模拟硬件设备。

![](./document/preview.png)

## 使用方法 ##

在本地windows环境下运行.\UdpPlugWebsocket\bin\Debug\UdpPlugWebsocket.exe即可按照config.xml中描述的端口启动服务程序。默认UDP端口为7101，Websocket端口为9000。此时外部的UDP连接和Websocket连接都会在UI中显示。其中UDP在终端设备页面中显示，Websocket连接在控制页面中显示，通讯状况在交换面板页面中显示。您可以通过配置->显示调试系信息 打开或关闭UI的通讯Log。


## 数据包格式 （UDP与Websocket相同）##

![](./document/packageintro.png)


**Endpoint是外部连接的唯一索引。**

**Endpoint字符串的格式为："XXX.XXX.XXX.XXX:XXXX" 前四组三位数为IP地址，最后一组四位数为远程端口号**

##UDP通讯测试##

- 运行.\UdpExample\client\bin\Debug\client.exe 在程序控制台中输入如下数据包
- 004832A08000000000200000000000000001234testtest05 即可在终端设备页面看到本机的连接。
![](./document/clienttest.png)
## Websocket通讯测试 ##
- 任意支持websocket的浏览器中运行.\UPW_Browser\index.html?ID=0000000002&MM=0000000000000000与UDP测试程序建立连接（参数区分大小写）
![](./document/webtest.png)

## 目录结构 ##
	.\UdpPlugWebsocket //UPWS服务程序（主服务程序）	
	.\UdpPlugWebsocket\UdpExample //UDP测试程序 	
	.\UPW_Browser //Websocket测试程序
	.\bin_UPWS //编译后的Win32项目二进制文
	...	 //其余目录为参考代码目录，可删除	


##编译环境##
Visual Studio 2015,C#,Win10


##测试地址##

您可以尝试在以下地址测试UPWS服务器

UDP服务端口：udp://box.miuser.net:7101 （Luat 2G通讯模块连接的UDP端口）

Websocket服务端口：ws://box.miuser.net:9000 （前端js代码连接的websocket端口）

H5前端测试页面地址：http://www.miuser.net/box/switch.htm （需要再地址上加上设备参数后缀再使用）

前端测试样例：http://www.miuser.net/box/switch.htm?ID=9030026845&MM=HlPpA86131853284 （请自行修改ID和MM）
	
## 使用到的开源库 ##
- 
- Websocket-Sharp
- https://github.com/sta/websocket-sharp
- Coldairarrow.Util.Sockets	
- https://github.com/Coldairarrow/Sockets
  https://github.com/miuser00/V3R_DemoBoard

## 关键词  ##

C#,Websocket,UDP,物联网,异步编程,接口,lamda表达式,线程池,Action,Invoke,Arduino,Air208，Luat

## 版权声明 ##

如果您愿意使用 UdpPlugWebSocket 组件，请遵循 MIT 许可所述内容.