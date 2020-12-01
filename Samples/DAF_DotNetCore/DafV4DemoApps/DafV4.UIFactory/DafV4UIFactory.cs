using DafV4.Dtos;
using DafV4.ServiceFactory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DafV4.UIFactory
{
    public class Mppgv4UIfactory : IDafV4UIFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public Mppgv4UIfactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        #region helper functions
        private static string Read_Optional_String_Input(string question)
        {
            return Read_String_Input($"{question}:", true);
        }
        private static string Read_Mandatory_String_Input(string question)
        {
            return Read_String_Input($"{question}:", false);
        }
        private static string Read_String_Input(string question, bool isOptional)
        {
            Console.WriteLine($"{question}");
            var ans = Console.ReadLine();
            if ((!isOptional) && string.IsNullOrWhiteSpace(ans))
            {
                return Read_String_Input(question, isOptional);
            }
            return ans;
        }
        private static string Read_LongString_Input(string question, bool isOptional)
        {
            Console.WriteLine($"{question}");
            byte[] inputBuffer = new byte[262144];//increse the size as per the needs
            Stream inputStream = Console.OpenStandardInput(262144);
            Console.SetIn(new StreamReader(inputStream, Console.InputEncoding, false, inputBuffer.Length));
            string strInput = Console.ReadLine();
            if ((!isOptional) && string.IsNullOrWhiteSpace(strInput))
            {
                return Read_LongString_Input(question, isOptional);
            }
            return strInput;
        }
        private static string Read_DataFormatType_Input(string question)
        {
            List<string> lst = new List<string> { "TLV", "NONE" };
            var ans = Read_Mandatory_String_Input(question + " " + "(" + String.Join(",", lst) + ")");
            if (lst.Contains<string>(ans.ToUpper()))
                return ans.ToUpper();
            else
            {
                Console.WriteLine("Invalid Option.");
                return Read_DataFormatType_Input(question);
            }
        }
        private static string Read_EncryptionType_Input(string question)
        {
            List<string> lst = new List<string> { "DATA", "PIN" };
            var ans = Read_Mandatory_String_Input(question + " " + "(" + String.Join(",", lst) + ")");
            if (lst.Contains<string>(ans.ToUpper()))
                return ans.ToUpper();
            else
            {
                Console.WriteLine("Invalid Option.");
                return Read_EncryptionType_Input(question);
            }
        }
        private static string Read_TrueFalse_Input(string question)
        {
            List<string> lst = new List<string> { "true", "false", "0", "1" };
            var ans = Read_Mandatory_String_Input(question + " " + "(" + String.Join(",", lst) + ")");
            if (lst.Contains<string>(ans.ToLower()))
                return ans.ToLower();
            else
            {
                Console.WriteLine("Invalid Option.");
                return Read_TrueFalse_Input(question);
            }
        }
        private static string Read_NetworkProtocolType_Input(string question)
        {
            List<string> lst = new List<string> { "TCPIP", "HTTP" };
            var ans = Read_Mandatory_String_Input(question + " " + "(" + String.Join(",", lst) + ")");
            if (lst.Contains<string>(ans.ToUpper()))
                return ans.ToUpper();
            else
            {
                Console.WriteLine("Invalid Option.");
                return Read_NetworkProtocolType_Input(question);
            }
        }
        private static int Read_Intuser_Input(string question)
        {
            var ans = Read_Mandatory_String_Input(question);
            try
            {
                var temp = int.Parse(ans);
                return temp;
            }
            catch
            {
                Console.WriteLine("Invalid Input.");
                return Read_Intuser_Input(question);
            }
        }
        private static List<KeyValuePair<string, string>> Read_MultipleKeysInput(string question)
        {
            var noOfKeys = Read_Intuser_Input($"Please Enter No of Keys for {question}");
            var result = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < noOfKeys; i++)
            {
                var key = Read_Optional_String_Input("Key");
                var val = Read_Optional_String_Input("Value");
                result.Add(new KeyValuePair<string, string>(key, val));
            }
            return result;
        }
        public static bool IsValidXml(string xml)
        {
            try
            {
                XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string PrettyXml(string xml)
        {
            if (IsValidXml(xml)) //print xml in beautiful format
            {
                var stringBuilder = new StringBuilder();
                var element = XElement.Parse(xml);
                var settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true,
                    NewLineOnAttributes = true
                };
                using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                {
                    element.Save(xmlWriter);
                }
                return stringBuilder.ToString();
            }
            else
            {
                return xml;
            }
        }
        #endregion

        public void ShowUI(DafV4UI mPPGv4UI)
        {
            switch (mPPGv4UI)
            {
                case DafV4UI.PROCESSCARDSWIPE:
                    ShowProcessCardSwipeUI();
                    break;
                case DafV4UI.PROCESSDATA:
                    ShowProcessDataUI();
                    break;
                case DafV4UI.PROCESSTOKEN:
                    ShowProcessTokenUI();
                    break;
                default:
                    break;
            }
        }
        private void ShowProcessCardSwipeUI()
        {
            var requestDto = new ProcessCardSwipeRequestDto();
            try
            {
                requestDto.AdditionalProcessCardSwipeRequestData = Read_MultipleKeysInput("AdditionalProcessCardSwipeRequestData");
                requestDto.CustomerCode = Read_Mandatory_String_Input("CustomerCode");
                requestDto.Username = Read_Mandatory_String_Input("Username");
                requestDto.Password = Read_Mandatory_String_Input("Password");
                requestDto.CustomerTransactionID = Read_Optional_String_Input("CustomerTransactionID");
                requestDto.AdditionalRequestData = Read_MultipleKeysInput("AdditionalRequestData");
                requestDto.ProcessCardSwipeInput_CustomerCode = Read_Optional_String_Input("ProcessCardSwipeInput_CustomerCode");
                requestDto.ProcessCardSwipeInput_Username = Read_Optional_String_Input("ProcessCardSwipeInput_Username");
                requestDto.ProcessCardSwipeInput_Password = Read_Optional_String_Input("ProcessCardSwipeInput_Password");
                requestDto.BillingLabel = Read_Optional_String_Input("BillingLabel");
                requestDto.ProcessCardSwipeInput_CustomerTransactionID = Read_Optional_String_Input("ProcessCardSwipeInput_CustomerTransactionID");
                requestDto.DeviceSN = Read_Optional_String_Input("DeviceSN");
                requestDto.IsReturnCardID = Read_TrueFalse_Input("IsReturnCardID");
                requestDto.KSN = Read_Mandatory_String_Input("KSN");
                requestDto.MagnePrint = Read_Mandatory_String_Input("MagnePrint");
                requestDto.MagnePrintStatus = Read_Mandatory_String_Input("MagnePrintStatus");
                requestDto.Track1 = Read_Optional_String_Input("Track1");
                requestDto.Track2 = Read_Mandatory_String_Input("Track2");
                requestDto.Track3 = Read_Optional_String_Input("Track3");
                requestDto.AdditionalEncryptedCardSwipeData = Read_MultipleKeysInput("AdditionalEncryptedCardSwipeData");
                requestDto.Headers = Read_MultipleKeysInput("Headers");
                requestDto.AdditionalPayloadInfoData = Read_MultipleKeysInput("AdditionalPayloadInfoData");
                requestDto.NetworkProtocolType = Read_NetworkProtocolType_Input("NetworkProtocolType");
                requestDto.Base64ClientCert = Read_Optional_String_Input("Base64ClientCert");
                requestDto.ClientCertPassword = Read_Optional_String_Input("ClientCertPassword");
                requestDto.AdditionalHTTPInfoData = Read_MultipleKeysInput("AdditionalHTTPInfoData");
                requestDto.Payload = Read_LongString_Input("Payload", false);
                requestDto.AccessEngineHeaderHex = Read_Optional_String_Input("AccessEngineHeaderHex");
                requestDto.AdditionalTCPIPInfoData = Read_MultipleKeysInput("AdditionalTCPIPInfoData");
                requestDto.NumberOfBytesToAddForLength = 0;
                requestDto.Port = 0;
                if (requestDto.NetworkProtocolType != "HTTP")
                {
                    requestDto.NumberOfBytesToAddForLength = Read_Intuser_Input("NumberOfBytesToAddForLength");
                    requestDto.Port = Read_Intuser_Input("Port");
                }
                requestDto.Uri = Read_Optional_String_Input("Uri");

                var svc = _serviceProvider.GetService<IProcessCardSwipeClient>();
                var responseDto = svc.ProcessCardSwipe(requestDto).Result;
                Console.WriteLine("=====================Response Start======================");
                Console.WriteLine("Response:");
                Console.Write(PrettyXml(responseDto.Content) + "\n");
                Console.WriteLine("=====================Response End======================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occurred while Processing ProcessCardSwipe" + ex.Message.ToString());
            }
        }

        //private void ShowProcessCardSwipeUI1()
        //{
        //    var requestDto = new ProcessCardSwipeRequestDto();
        //    try
        //    {
        //        requestDto.AdditionalProcessCardSwipeRequestData = new List<KeyValuePair<string, string>>() 
        //        {
        //        };
        //        requestDto.CustomerCode = "VV73029843";
        //        requestDto.Username = "demo";
        //        requestDto.Password = "demo4DAFtoken!";
        //        requestDto.CustomerTransactionID = "";
        //        requestDto.AdditionalRequestData = new List<KeyValuePair<string, string>>() 
        //        { 
        //            new KeyValuePair<string, string>("EncryptionType","81") ,
        //            new KeyValuePair<string, string>("TokenDataFormat","5A$Iif($IsEqualStrings($Substring({CCNum},0,2),34),08{CCNum}F,$Iif($IsEqualStrings($Substring({CCNum},0,2),37),08{CCNum}F,08{CCNum}))5F2402{YY}{MM}"),
        //            new KeyValuePair<string, string>("TokenValidUntilUTC","12/12/2025")

        //        };
        //        requestDto.ProcessCardSwipeInput_CustomerCode = "VV73029843";
        //        requestDto.ProcessCardSwipeInput_Username = "demo";
        //        requestDto.ProcessCardSwipeInput_Password = "demo4DAFtoken!";
        //        requestDto.BillingLabel = "BillingLabel";
        //        requestDto.ProcessCardSwipeInput_CustomerTransactionID = "CustomerTransactionID";
        //        requestDto.DeviceSN = "B00CC5B";
        //        requestDto.IsReturnCardID = "true";
        //        requestDto.KSN = "9011400B00CC5B0011A1";
        //        requestDto.MagnePrint = "12C458E3EE877AC87975E805E857EE00CB780FD477EEACCC1F49E56117C3D16215CFF1397CA954A63DA2B35CEDD06C66E2F59381AEAA8025";
        //        requestDto.MagnePrintStatus = "2098705";
        //        requestDto.Track1 = "9E76622E68086DBD810C91FA754CA2DD4DF56EBB60C459FE44BAB35A4A1F1DD1087BC967DE4E0071AE8D23DE0A2F20DC3B691A1835EB5CFB038B6C4888C2BD94002FFAA2F9ECC6B5";
        //        requestDto.Track2 = "C74580955B180F94EE66B8A14BB9287B4F28D139A96BC7423071899FB6B93EA7";
        //        requestDto.Track3 = "";
        //        requestDto.AdditionalEncryptedCardSwipeData = new List<KeyValuePair<string, string>>()
        //        {
        //        };
        //        requestDto.Headers = new List<KeyValuePair<string, string>>()
        //        {
        //            new KeyValuePair<string, string>("Content-Type","text/xml; charset=utf-8")
        //        };
        //        requestDto.AdditionalPayloadInfoData = new List<KeyValuePair<string, string>>() 
        //        { 
        //        };
        //        requestDto.NetworkProtocolType = "HTTP";
        //        requestDto.Base64ClientCert = "";
        //        requestDto.ClientCertPassword = "";
        //        requestDto.AdditionalHTTPInfoData = new List<KeyValuePair<string, string>>()
        //        { 
        //        };
        //        requestDto.Payload = File.ReadAllText(@"C:\Users\spottumuttu\Desktop\payload.txt");
        //        requestDto.AccessEngineHeaderHex = "";
        //        requestDto.AdditionalTCPIPInfoData = new List<KeyValuePair<string, string>>() 
        //        { 
        //        };
        //        requestDto.NumberOfBytesToAddForLength = 0;
        //        requestDto.Port = 0;
        //        if (requestDto.NetworkProtocolType != "HTTP")
        //        {
        //            requestDto.NumberOfBytesToAddForLength = Read_Intuser_Input("NumberOfBytesToAddForLength");
        //            requestDto.Port = Read_Intuser_Input("Port");
        //        }
        //        requestDto.Uri = @"https://stg.dw.us.fdcnet.biz/rc";

        //        var svc = _serviceProvider.GetService<IProcessCardSwipeClient>();
        //        var responseDto = svc.ProcessCardSwipe(requestDto).Result;
        //        Console.WriteLine("=====================Response Start======================");
        //        Console.WriteLine("Response:");
        //        Console.Write(PrettyXml(responseDto.Content) + "\n");
        //        Console.WriteLine("=====================Response End======================");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error Occurred while Processing ProcessCardSwipe" + ex.Message.ToString());
        //    }
        //}

        private void ShowProcessDataUI()
        {
            var requestDto = new ProcessDataRequestDto();
            try
            {
                requestDto.AdditionalProcessDataRequestData = Read_MultipleKeysInput("AdditionalProcessDataRequestData");
                requestDto.CustomerCode = Read_Mandatory_String_Input("CustomerCode");
                requestDto.Username = Read_Mandatory_String_Input("Username");
                requestDto.Password = Read_Mandatory_String_Input("Password");
                requestDto.CustomerTransactionID = Read_Optional_String_Input("CustomerTransactionID");
                requestDto.AdditionalRequestData = Read_MultipleKeysInput("AdditionalRequestData");
                requestDto.ProcessDataInput_CustomerCode = Read_Optional_String_Input("ProcessDataInput_CustomerCode");
                requestDto.ProcessDataInput__Username = Read_Optional_String_Input("ProcessDataInput__Username");
                requestDto.ProcessDataInput_Password = Read_Optional_String_Input("ProcessDataInput_Password");
                requestDto.BillingLabel = Read_Optional_String_Input("BillingLabel");
                requestDto.ProcessDataInput_CustomerTransactionID = Read_Optional_String_Input("ProcessDataInput_CustomerTransactionID");
                requestDto.Data = Read_LongString_Input("Data", false);
                requestDto.DataFormatType = Read_DataFormatType_Input("DataFormatType");
                requestDto.EncryptionType = Read_EncryptionType_Input("EncryptionType");
                requestDto.KSN = Read_Mandatory_String_Input("KSN");
                requestDto.NumberOfPaddedBytes = Read_Intuser_Input("NumberOfPaddedBytes");
                requestDto.IsEncrypted = Read_TrueFalse_Input("IsEncrypted");
                requestDto.OutputPanLast4Format = Read_Optional_String_Input("OutputPanLast4Format");
                requestDto.AdditionalPayloadInfoData = Read_MultipleKeysInput("AdditionalPayloadInfoData");
                requestDto.Base64ClientCert = Read_Optional_String_Input("Base64ClientCert");
                requestDto.ClientCertPassword = Read_Optional_String_Input("ClientCertPassword");
                requestDto.AdditionalHTTPInfoData = Read_MultipleKeysInput("AdditionalHTTPInfoData");
                requestDto.Headers = Read_MultipleKeysInput("Headers");
                requestDto.NetworkProtocolType = Read_NetworkProtocolType_Input("NetworkProtocolType");
                requestDto.Payload = Read_LongString_Input("Payload", false);
                requestDto.AccessEngineHeaderHex = Read_Optional_String_Input("AccessEngineHeaderHex");
                requestDto.AdditionalTCPIPInfoData = Read_MultipleKeysInput("AdditionalTCPIPInfoData");
                if (requestDto.NetworkProtocolType != "HTTP")
                {
                    requestDto.NumberOfBytesToAddForLength = Read_Intuser_Input("NumberOfBytesToAddForLength");
                    requestDto.Port = Read_Intuser_Input("Port");
                }
                requestDto.Uri = Read_Mandatory_String_Input("Uri");
                var svc = _serviceProvider.GetService<IProcessDataClient>();
                var responseDto = svc.ProcessData(requestDto).Result;
                Console.WriteLine("=====================Response Start======================");
                Console.WriteLine("Response:");
                Console.Write(PrettyXml(responseDto.Content) + "\n");
                Console.WriteLine("=====================Response End======================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occurred while Processing ProcessData" + ex.Message.ToString());
            }
        }
        private void ShowProcessTokenUI()
        {
            var requestDto = new ProcessTokenRequestDto();
            try
            {
                requestDto.AdditionalProcessTokenRequestData = Read_MultipleKeysInput("AdditionalProcessTokenRequestData");
                requestDto.CustomerCode = Read_Mandatory_String_Input("Customer Code");
                requestDto.Username = Read_Mandatory_String_Input("Username");
                requestDto.Password = Read_Mandatory_String_Input("Password");
                requestDto.CustomerTransactionID = Read_Optional_String_Input("CustomerTransactionID");
                requestDto.AdditionalRequestData = Read_MultipleKeysInput("AdditionalRequestData");
                requestDto.ProcessTokenInput_CustomerCode = Read_Optional_String_Input("ProcessTokenInput_CustomerCode");
                requestDto.ProcessTokenInput_Username = Read_Optional_String_Input("ProcessTokenInput_Username");
                requestDto.ProcessTokenInput_Password = Read_Optional_String_Input("ProcessTokenInput_Password");
                requestDto.BillingLabel = Read_Optional_String_Input("BillingLabel");
                requestDto.ProcessTokenInput_CustomerTransactionID = Read_Optional_String_Input("ProcessTokenInput_CustomerTransactionID");
                requestDto.AdditionalPayloadInfoData = Read_MultipleKeysInput("AdditionalPayloadInfoData");
                requestDto.Base64ClientCert = Read_Optional_String_Input("Base64ClientCert");
                requestDto.OutputPanLast4Format = Read_Optional_String_Input("OutputPanLast4Format");
                requestDto.ClientCertPassword = Read_Optional_String_Input("ClientCertPassword");
                requestDto.AdditionalHTTPInfoData = Read_MultipleKeysInput("AdditionalHTTPInfoData");
                requestDto.Headers = Read_MultipleKeysInput("Headers");
                requestDto.NetworkProtocolType = Read_NetworkProtocolType_Input("NetworkProtocolType");
                requestDto.Payload = Read_LongString_Input("Payload", false);
                requestDto.AccessEngineHeaderHex = Read_Optional_String_Input("AccessEngineHeaderHex");
                requestDto.AdditionalTCPIPInfoData = Read_MultipleKeysInput("AdditionalTCPIPInfoData");
                if (requestDto.NetworkProtocolType != "HTTP")
                {
                    requestDto.NumberOfBytesToAddForLength = Read_Intuser_Input("NumberOfBytesToAddForLength");
                    requestDto.Port = Read_Intuser_Input("Port");
                }
                requestDto.Uri = Read_Mandatory_String_Input("Uri");
                requestDto.Token = Read_Mandatory_String_Input("Token");
                var svc = _serviceProvider.GetService<IProcessTokenClient>();
                var responseDto = svc.ProcessToken(requestDto).Result;
                Console.WriteLine("=====================Response Start======================");
                Console.WriteLine("Response:");
                Console.Write(PrettyXml(responseDto.Content) + "\n");
                Console.WriteLine("=====================Response End======================");
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error Occurred while Processing ProcessToken" + ex.Message.ToString());
            }
        }

    }
}
