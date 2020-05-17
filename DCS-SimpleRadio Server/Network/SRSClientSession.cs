using Ciribob.DCS.SimpleRadio.Standalone.Common.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using Ciribob.DCS.SimpleRadio.Standalone.Server.Network.SuperSocket;

namespace Ciribob.DCS.SimpleRadio.Standalone.Server.Network
{
    public class SRSClientSession : AppSession<SRSClientSession, MyRequestInfo>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public string SRSGuid { get; set; }

        protected override void OnSessionStarted()
        {
            Logger.Info($"Client Connected {this.SessionID} ");
        }

        protected override void HandleUnknownRequest(MyRequestInfo requestInfo)
        {
            Logger.Info($"Client Unknown Request {this.SessionID} ");
           
        }

        protected override void HandleException(Exception e)
        {
            Logger.Error(e, $"Caught Client Session Exception {this.SessionID}");
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            Logger.Info($"Client Disconnecting {this.SessionID}: {reason}");

            
        }
    }
}
