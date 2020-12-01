using System.Collections.Generic;

namespace DafV4.Dtos
{
    public class ProcessTokenRequestDto
    {
        public List<KeyValuePair<string, string>> AdditionalProcessTokenRequestData { get; set; }
        public string CustomerCode { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string CustomerTransactionID { get; set; }
        public List<KeyValuePair<string, string>> AdditionalRequestData { get; set; }
        public string ProcessTokenInput_CustomerCode { get; set; }
        public string ProcessTokenInput_Password { get; set; }
        public string ProcessTokenInput_Username { get; set; }
        public string BillingLabel { get; set; }
        public string ProcessTokenInput_CustomerTransactionID { get; set; }
        public List<KeyValuePair<string, string>> AdditionalPayloadInfoData { get; set; }
        public string Base64ClientCert { get; set; }
        public string OutputPanLast4Format { get; set; }
        public string ClientCertPassword { get; set; }
        public List<KeyValuePair<string, string>> AdditionalHTTPInfoData { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }
        public string NetworkProtocolType { get; set; }
        public string Payload { get; set; }
        public string AccessEngineHeaderHex { get; set; }
        public List<KeyValuePair<string, string>> AdditionalTCPIPInfoData { get; set; }
        public int NumberOfBytesToAddForLength { get; set; }
        public int Port { get; set; }
        public string Uri { get; set; }
        public string Token { get; set; }
    }
}
