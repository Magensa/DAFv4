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
    public class ProcessTokenClient : IProcessTokenClient
    {
        private readonly IConfiguration _config;
        private string ServiceUrl => this._config["DAFV4SERVICEURL"].ToString();
        public ProcessTokenClient(IConfiguration config)
        {
            _config = config;
        }
        public async Task<ProcessTokenResponseDto> ProcessToken(ProcessTokenRequestDto dto)
        {
            var responseDto = new ProcessTokenResponseDto();

            try
            {
                var soapRequest = new ProcessTokenRequest
                {
                    AdditionalProcessTokenRequestData = dto.AdditionalProcessTokenRequestData.ToArray(),
                    Authentication = new Authentication()
                    {
                        CustomerCode = dto.CustomerCode,
                        Username = dto.Username,
                        Password = dto.Password
                    },
                    CustomerTransactionID = dto.CustomerTransactionID,
                    ProcessTokenInputs = new List<ProcessTokenInput>()
                {
                    new ProcessTokenInput(){
                        AdditionalRequestData = dto.AdditionalRequestData.ToArray(),
                        Authentication=new Authentication()
                        {
                            CustomerCode=dto.ProcessTokenInput_CustomerCode,
                            Username=dto.ProcessTokenInput_Username,
                            Password=dto.ProcessTokenInput_Password
                        },
                        BillingLabel=dto.BillingLabel,
                        CustomerTransactionID=dto.ProcessTokenInput_CustomerTransactionID,
                        PayloadInfo=new PayloadInfo()
                        {
                            AdditionalPayloadInfoData=dto.AdditionalPayloadInfoData.ToArray(),
                            Base64ClientCert=dto.Base64ClientCert,
                            ClientCertPassword=dto.ClientCertPassword,
                            HTTPInfo= new HTTPInfo()
                            {
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
                            Uri= dto.Uri,

                        },
                        Token=dto.Token
                    }
                }.ToArray()
                };

                var svcEndPointAddress = new EndpointAddress(ServiceUrl);
                var svcEndPointConfig = DecryptAndForwardClient.EndpointConfiguration.BasicHttpBinding_IDecryptAndForward;
                var svcClient = new DecryptAndForwardClient(svcEndPointConfig, svcEndPointAddress);
                var requestInterceptor = new DafV4InspectorBehavior();
                svcClient.Endpoint.EndpointBehaviors.Add(requestInterceptor);
                _ = await svcClient.ProcessTokenAsync(soapRequest);
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
