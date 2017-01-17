using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using EbaySalesTracker.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EbaySalesTracker.Repository
{
    public class UserEngine
    {         
        public string GetSessionId()
        {
            string sessionId = "";
            var context = RequestBuilder.CreateNewApiCall();
            GetSessionIDCall sessionIdCall = new GetSessionIDCall(context);
            sessionIdCall.RuName = "Cameron_Blazevi-CameronB-EbayFe-urvcak";
            sessionIdCall.Execute();

            sessionId = sessionIdCall.SessionID;
            return sessionId;
        }

        public List<string> GetUserToken(string sessionId)
        {
            List<string> tokenWithExpDate = new List<string>();
            var context = RequestBuilder.CreateNewApiCall();
            FetchTokenCall fetchTokenCall = new FetchTokenCall(context);
            fetchTokenCall.SessionID = sessionId;
            fetchTokenCall.Execute();

            string authToken = fetchTokenCall.eBayToken;
            string expDate = fetchTokenCall.HardExpirationTime.ToString();

            tokenWithExpDate.Add(authToken);
            tokenWithExpDate.Add(expDate);

            return tokenWithExpDate;
          
        }

        public bool GetEbayOfficialTime(string userToken)
        {

            var context = RequestBuilder.CreateNewApiCall(userToken);
            GeteBayOfficialTimeCall ebayOfficialTimeCall = new GeteBayOfficialTimeCall(context);
            DateTime officialTime = ebayOfficialTimeCall.GeteBayOfficialTime();

            var success = ebayOfficialTimeCall.ApiResponse.Ack.ToString();

            return (success == "Success");
        }
        


    }
}
