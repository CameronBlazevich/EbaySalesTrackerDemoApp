using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Util;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace EbaySalesTracker.Repository.Helpers
{
    public static class RequestBuilder
    {
        private static ApiContext apiContext = null;

        static ApiContext GetApiContext()
        {
            //apiContext is a singleton,
            //to avoid duplicate config file reading
            if (apiContext != null)
            {
                return apiContext;
            }
            else
            {
                apiContext = new ApiContext();
                return apiContext;
            }
        }


        public static ApiContext CreateNewApiCall(string userToken)
        {
            var context = GetApiContext();
            context.SoapApiServerUrl = ConfigurationManager.AppSettings["EbaySoapApiServerUrl"];
            context.ApiCredential.eBayToken = userToken;
            //context.ApiLogManager = new ApiLogManager();
            //context.ApiLogManager.ApiLoggerList.Add(new FileLogger("log.txt", true, true, true));
            //context.ApiLogManager.EnableLogging = true;
            context.Version = ConfigurationManager.AppSettings["ApiVersion"];
            context.Site = SiteCodeType.US;
            return context;
        }

        public static ApiContext CreateNewApiCall()
        {
            var context = GetApiContext();
            context.SoapApiServerUrl = ConfigurationManager.AppSettings["EbaySoapApiServerUrl"];
            context.ApiCredential.ApiAccount.Application = ConfigurationManager.AppSettings["ApplicationApiCredential"];
            context.ApiCredential.ApiAccount.Developer = ConfigurationManager.AppSettings["DeveloperApiCredential"];
            context.ApiCredential.ApiAccount.Certificate = ConfigurationManager.AppSettings["CertificateApiCredential"];
            context.Version = ConfigurationManager.AppSettings["ApiVersion"];
            context.Site = SiteCodeType.US;
            return context;
        }
        public static ApiContext CreateNewSandboxApiCall()
        {
            var context = GetApiContext();
            context.SoapApiServerUrl = "https://api.sandbox.ebay.com/ws/api.dll";
            context.ApiCredential.eBayToken = "AgAAAA**AQAAAA**aAAAAA**kJyNVw**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wJnY+lCpmBpwidj6x9nY+seQ**sc4DAA**AAMAAA**5UqGFCLKB92sBK3eZHS/7dn/x21q5q8nyCHoRuStHNN40pHn5lNWiVRcwGbMNE8W0BmujTHgaoydqptKARDL+bldgcnTK3XcQ193gJQEK423y7X0nFseK9PH3N0DwrmUgwLEvfDwV0Pb4qZg2y1zms4pBLPBirp9RStbCUabS2uOk4QX8LwEJGe1leJRHvH+koIoYsElnDuyd1cHIrUqV4uSd8FgC2TclrrbX8M6t/wJ3kiJJbh8Sj1LhIDJrVUJdpJQAM1Zt+HViPe/VcJ2uJexOeQBrTYyG0ees1o0qzXX339axPe3VlhODlskPrP3KtRtsGWvV18bQuvGA1lOVIkvDr91MNeEDlGCAI6ElIIx3o5j5ZISQc+YJxSXSTzAEs4/39urY5Sghfr750l0xAth6fWZdNEhrUsfl3TlsvsbTk5eOGzMVUnjfptTgjcdvRlUT1XvoAeWMeN206tbKOYwMkCCHCOrKotJ4v+k86/9RSiRQxy9tBfsD952Wtt5a5+bcFmqtOkJ6oHw3N6m+4z2/D5hkFmrttkUpD6cndf6QWOFlmAdsmXiOW0OOkSBMDXre6Rl7edKVKCTqaJB6Lcy01qRlYnTnzzXOJWAZIrGl55fvgL2n1qddvsAWwPKL1e5MnTyga4v7L5kpy+t/jxyWFSdyF1WS1MM2G5YxiOGeM5YpjXx8UdTBMikG+JBPm595SqYpV9BId55eXLfrfXrDaotlooy9HkmeKDrX/16KSSefz8HeneGPMjVh89r";
            //context.ApiCredential.ApiAccount.Application = "CameronB-EbayFeeT-SBX-b4d8cb72c-48a0b878";
            //context.ApiCredential.ApiAccount.Developer = "7ca838e8-8f1c-4d08-80c7-a4ba24561e8c";
            //context.ApiCredential.ApiAccount.Certificate = "SBX-4d8cb72c01c1-d197-4d4c-984a-6a60";
            context.Version = "971";
            context.Site = SiteCodeType.US;
            return context;

        }
        
       

    }
}
