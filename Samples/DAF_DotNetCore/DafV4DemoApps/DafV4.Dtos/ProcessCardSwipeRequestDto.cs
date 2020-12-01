using System.Collections.Generic;

namespace DafV4.Dtos
{
    public class ProcessCardSwipeRequestDto
    {
        public List<KeyValuePair<string, string>> AdditionalProcessCardSwipeRequestData { get; set; }
        public string CustomerTransactionID { get; set; }
        public string CustomerCode { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public List<KeyValuePair<string, string>> AdditionalRequestData { get; set; }
        public string ProcessCardSwipeInput_CustomerCode { get; set; }
        public string ProcessCardSwipeInput_Username { get; set; }
        public string ProcessCardSwipeInput_Password { get; set; }
        public string BillingLabel { get; set; }
        public string ProcessCardSwipeInput_CustomerTransactionID { get; set; }
        public string DeviceSN { get; set; }
        public string IsReturnCardID { get; set; }
        public string KSN { get; set; }
        public string MagnePrint { get; set; }
        public string MagnePrintStatus { get; set; }
        public string Track1 { get; set; }
        public string Track2 { get; set; }
        public string Track3 { get; set; }
        public List<KeyValuePair<string, string>> AdditionalEncryptedCardSwipeData { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }
        public List<KeyValuePair<string, string>> AdditionalPayloadInfoData { get; set; }
        public string NetworkProtocolType { get; set; }
        public string Base64ClientCert { get; set; }
        public string ClientCertPassword { get; set; }
        public List<KeyValuePair<string, string>> AdditionalHTTPInfoData { get; set; }
        public string Payload { get; set; }
        public string AccessEngineHeaderHex { get; set; }
        public List<KeyValuePair<string, string>> AdditionalTCPIPInfoData { get; set; }
        public int NumberOfBytesToAddForLength { get; set; }
        public int Port { get; set; }
        public string Uri { get; set; }

    }
}
