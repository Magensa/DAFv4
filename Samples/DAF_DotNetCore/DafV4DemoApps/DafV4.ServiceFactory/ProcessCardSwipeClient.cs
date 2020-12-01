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

    public class ProcessCardSwipeClient : IProcessCardSwipeClient
    {
        private readonly IConfiguration _config;
        private string ServiceUrl => this._config["DAFV4SERVICEURL"].ToString();

        public ProcessCardSwipeClient(IConfiguration config)
        {
            _config = config;
        }

        public async Task<ProcessCardSwipeResponseDto> ProcessCardSwipe(ProcessCardSwipeRequestDto dto)
        {
            var responseDto = new ProcessCardSwipeResponseDto();

            try
            {
                var soapRequest = new ProcessCardSwipeRequest
                {
                    AdditionalProcessCardSwipeRequestData = dto.AdditionalProcessCardSwipeRequestData.ToArray(),
                    Authentication = new Authentication()
                    {
                        CustomerCode = dto.CustomerCode,
                        Username = dto.Username,
                        Password = dto.Password
                    },
                    CustomerTransactionID = dto.CustomerTransactionID,
                    ProcessCardSwipeInputs = new List<ProcessCardSwipeInput>()
                {
                    new ProcessCardSwipeInput(){
                        AdditionalRequestData =  dto.AdditionalRequestData.ToArray(),
                        Authentication=new Authentication()
                        {
                            CustomerCode=dto.ProcessCardSwipeInput_CustomerCode,
                            Username=dto.ProcessCardSwipeInput_Username,
                            Password=dto.ProcessCardSwipeInput_Password
                        },
                        BillingLabel=dto.BillingLabel,
                        CustomerTransactionID=dto.ProcessCardSwipeInput_CustomerTransactionID,
                        EncryptedCardSwipe=new EncryptedCardSwipe(){
                            DeviceSN=dto.DeviceSN,
                            IsReturnCardID=bool.Parse(dto.IsReturnCardID),
                            KSN=dto.KSN,
                            MagnePrint=dto.MagnePrint,
                            MagnePrintStatus=dto.MagnePrintStatus,
                            Track1=dto.Track1,
                            Track2=dto.Track2,
                            Track3=dto.Track3,
                            AdditionalEncryptedCardSwipeData=dto.AdditionalEncryptedCardSwipeData.ToArray(),
                        },
                        PayloadInfo=new PayloadInfo()
                        {
                            HTTPInfo= new HTTPInfo()
                            {
                                Headers=dto.Headers.ToArray()
                            },
                            NetworkProtocolType= (dto.NetworkProtocolType.Trim().ToUpper()=="HTTP")? NetworkProtocolType.HTTP:NetworkProtocolType.TCPIP,
                            Payload= dto.Payload,
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
                _ = await svcClient.ProcessCardSwipeAsync(soapRequest);
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
