using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml;


namespace DafV4.ServiceFactory
{
    public class DafV4MessageInspector : IClientMessageInspector
    {
        public string LastRequestXML { get; private set; }
        public string LastResponseXML { get; private set; }
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            LastResponseXML = reply.ToString();
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            var soapAction = request.Headers.Action;
            XmlDictionaryReader bodyReader = request.GetReaderAtBodyContents();
            var soapXml = bodyReader.ReadOuterXml();
            DafV4BodyWriter newBody = new DafV4BodyWriter(soapXml);
            Message replacedMessage = Message.CreateMessage(request.Version, soapAction, newBody);
            replacedMessage.Properties.CopyProperties(request.Properties);
            request = replacedMessage;
            LastRequestXML = request.ToString();
            return request;
        }
    }
}
