using Ciribob.DCS.SimpleRadio.Standalone.Common.Network;
using Newtonsoft.Json;
using NLog;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciribob.DCS.SimpleRadio.Standalone.Server.Network.SuperSocket
{
    public class MyReceiveFilter : IReceiveFilter<MyRequestInfo>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        // This Method (Filter) is called whenever there is a new request from a connection/session 
        //- This sample method will convert the incomming Byte Array to Unicode string


        // Received data string.
        private readonly StringBuilder _receiveBuffer = new StringBuilder();

        public MyRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;

            try
            {
                _receiveBuffer.Append(Encoding.UTF8.GetString(readBuffer, (int)offset, (int)length));

                var messages = GetNetworkMessage();

                if(messages.Count>0)
                {
                    var deviceRequest = new MyRequestInfo { Messages = messages };
                    return deviceRequest;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Unable to process JSON");
            }

            return null;
        }

        private List<NetworkMessage> GetNetworkMessage()
        {
            List<NetworkMessage> messages = new List<NetworkMessage>();
            //search for a \n, extract up to that \n and then remove from buffer
            var content = _receiveBuffer.ToString();
            while (content.Length > 2 && content.Contains("\n"))
            {
                //extract message
                var message = content.Substring(0, content.IndexOf("\n", StringComparison.Ordinal) + 1);

                //now clear from buffer
                _receiveBuffer.Remove(0, message.Length);

                try
                {

                    var networkMessage = (JsonConvert.DeserializeObject<NetworkMessage>(message.Trim()));
                    //trim the received part
                    messages.Add(networkMessage);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"Unable to process JSON: \n {message}");
                }


                //load in next part
                content = _receiveBuffer.ToString();
            }

            return messages;
        }

        public void Reset()
        {
            _receiveBuffer.Clear();
        }

        public int LeftBufferSize { get; }
        public IReceiveFilter<MyRequestInfo> NextReceiveFilter { get; }
        public FilterState State { get; }
    }
}
