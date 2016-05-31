using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace EbaySalesTracker.Repository.Helpers
{
    public static class RequestBuilder
    {

        public static ApiContext CreateNewApiCall(string userToken)
        {
            var context = new ApiContext();
            context.SoapApiServerUrl = "https://api.ebay.com/wsapi";
            context.ApiCredential.eBayToken = userToken;
            //context.ApiCredential.ApiAccount.Application = "CameronB-EbayFeeT-PRD-e8a129233-5ff958d9";
            //context.ApiCredential.ApiAccount.Developer = "7ca838e8-8f1c-4d08-80c7-a4ba24561e8c";
            //context.ApiCredential.ApiAccount.Certificate = "PRD-8a1292338ccc-48d6-439d-9041-2691";
            context.Version = "949";
            context.Site = SiteCodeType.US;
            return context;
        }

        public static ApiContext CreateNewApiCall()
        {
            var context = new ApiContext();
            context.SoapApiServerUrl = "https://api.ebay.com/wsapi";
            context.ApiCredential.ApiAccount.Application = "CameronB-EbayFeeT-PRD-e8a129233-5ff958d9";
            context.ApiCredential.ApiAccount.Developer = "7ca838e8-8f1c-4d08-80c7-a4ba24561e8c";
            context.ApiCredential.ApiAccount.Certificate = "PRD-8a1292338ccc-48d6-439d-9041-2691";
            context.Version = "949";
            context.Site = SiteCodeType.US;
            return context;
        }

    }
}
