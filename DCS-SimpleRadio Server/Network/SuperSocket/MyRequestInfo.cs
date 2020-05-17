using Ciribob.DCS.SimpleRadio.Standalone.Common.Network;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciribob.DCS.SimpleRadio.Standalone.Server.Network.SuperSocket
{
    public class MyRequestInfo : IRequestInfo
    {
        public string Key { get; set; }
        public List<NetworkMessage> Messages { get; set; }

        // You can add more properties here
    }
}
