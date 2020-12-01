using System.ServiceModel.Channels;
using System.Xml;

namespace DafV4.ServiceFactory
{
    public class DafV4BodyWriter : BodyWriter
    {
        readonly string body;
        public DafV4BodyWriter(string strData) : base(true)
        {
            body = strData;
        }
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            //developer comments: The default behaviour of the soap message 
            //encodes the cdata tags . The encoded values will be sent to soap service and the soap request fails
            //therefore replacing the encoded tags
            var modifiedBody = body.Replace("&lt;", "<").Replace("&gt;", ">");
            writer.WriteRaw(modifiedBody);
        }
    }
}
