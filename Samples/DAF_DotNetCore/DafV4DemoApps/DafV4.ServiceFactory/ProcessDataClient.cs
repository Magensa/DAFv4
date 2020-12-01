using DafV4.Dtos;
using Magensa.DafV4.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DafV4.ServiceFactory
{
    public class ProcessDataClient : IProcessDataClient
    {
        private readonly IConfiguration _config;
        private string ServiceUrl => this._config["DAFV4SERVICEURL"].ToString();
        public ProcessDataClient(IConfiguration config)
        {
            _config = config;
        }
        public async Task<ProcessDataResponseDto> ProcessData(ProcessDataRequestDto dto)
        {
            var responseDto = new ProcessDataResponseDto();
            try
            {
                var soapRequest = new ProcessDataRequest
                {
                    AdditionalProcessDataRequestData = dto.AdditionalProcessDataRequestData.ToArray(),
                    Authentication = new Authentication()
                    {
                        CustomerCode = dto.CustomerCode,
                        Username = dto.Username,
                        Password = dto.Password
                    },
                    CustomerTransactionID = dto.CustomerTransactionID,
                    ProcessDataInputs = new List<ProcessDataInput>()
                {
                    new ProcessDataInput(){
                        AdditionalRequestData = dto.AdditionalRequestData.ToArray(),
                        Authentication=new Authentication()
                        {
                            CustomerCode=dto.ProcessDataInput_CustomerCode,
                            Username=dto.ProcessDataInput__Username,
                            Password=dto.ProcessDataInput_Password
                        },
                        BillingLabel=dto.BillingLabel,
                        CustomerTransactionID=dto.ProcessDataInput_CustomerTransactionID,

                        DataInput=new DataInput(){
                            Data=dto.Data,
                            DataFormatType= (dto.DataFormatType.Trim().ToUpper()=="TLV")?FormatType.TLV:FormatType.NONE,
                            EncryptionInfo = new EncryptionInfo()
                            {
                                EncryptionType=dto.EncryptionType,
                                KSN=dto.KSN,
                                NumberOfPaddedBytes=dto.NumberOfPaddedBytes
                            },
                            IsEncrypted=true,
                            OutputPanLast4Format=dto.OutputPanLast4Format
                        },
                        PayloadInfo=new PayloadInfo()
                        {
                            AdditionalPayloadInfoData = dto.AdditionalPayloadInfoData.ToArray(),
                            Base64ClientCert=dto.Base64ClientCert,
                            ClientCertPassword=dto.ClientCertPassword,
                            HTTPInfo= new HTTPInfo()
                            {
                                AdditionalHTTPInfoData=dto.AdditionalHTTPInfoData.ToArray(),
                                Headers=dto.Headers.ToArray()
                            },
                            NetworkProtocolType= (dto.NetworkProtocolType.Trim().ToUpper()=="HTTP")? NetworkProtocolType.HTTP:NetworkProtocolType.TCPIP,
                            Payload=dto.Payload,
                            TCPIPInfo = new TCPIPInfo()
                            {
                                AccessEngineHeaderHex=Encoding.ASCII.GetBytes(dto.AccessEngineHeaderHex),
                                AdditionalTCPIPInfoData = dto.AdditionalTCPIPInfoData.ToArray(),
                                NumberOfBytesToAddForLength = dto.NumberOfBytesToAddForLength,
                                Port=dto.Port
                            },
                            Uri= dto.Uri
                        }
                    }
                }.ToArray()
                };

                var svcEndPointAddress = new EndpointAddress(ServiceUrl);
                var svcEndPointConfig = DecryptAndForwardClient.EndpointConfiguration.BasicHttpBinding_IDecryptAndForward;
                var svcClient = new DecryptAndForwardClient(svcEndPointConfig, svcEndPointAddress);
                var requestInterceptor = new DafV4InspectorBehavior();
                svcClient.Endpoint.EndpointBehaviors.Add(requestInterceptor);
                _ = await svcClient.ProcessDataAsync(soapRequest);
                _ = requestInterceptor.LastRequestXML;
                string responseXML = requestInterceptor.LastResponseXML;
                responseDto.Content = responseXML;
            }
            catch (Exception ex) when (ex is CommunicationException || ex is ProtocolException || ex is FaultException || ex is Exception)
            {
                throw ex;
            }

            return responseDto;
        }

    }
}
