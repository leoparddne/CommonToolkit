﻿using System.Net;
using System.Net.Sockets;

namespace Common.Toolkit.Helper
{
    public class PortHelper
    {

        /// <summary>
        /// 获取随机端口
        /// </summary>
        /// <returns></returns>
        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Any, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
