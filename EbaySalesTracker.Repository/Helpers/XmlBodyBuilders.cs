using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EbaySalesTracker.Repository.Helpers
{
    public class XmlBodyBuilders
    {
        XNamespace ns = "urn:ebay:apis:eBLBaseComponents";
        string xmlHead = "<?xmlversion=\"1.0\" encoding=\"utf-8\"?>";

        public XElement BuildGetUserTokenRequestBody(string sessionId)
        {
            XElement body = new XElement(ns + "FetchTokenRequest", 
                new XElement(ns + "SessionID", sessionId));
            return body;
        }
        public XElement BuildGetSessionIdRequestBody()
        {
            var ru_name = "Cameron_Blazevi-CameronB-EbayFe-dsbmnl";
            var apiElement = "GetSessionIDRequest";
            XElement body = new XElement(ns + apiElement,
                new XElement(ns + "RuName", ru_name));
            return body;
        }
        public XElement BuildCommonRequestBody(string apiElement, string userToken)
        {
            XElement body = new XElement(ns + apiElement,
                new XElement(ns + "RequesterCredentials",
                                    new XElement(ns + "eBayAuthToken", userToken)));
            return body;
        }
        public XElement AddElement(XElement body, string name, object value)
        {
            body.Add(new XElement(ns + name, value));
            return body;
        }
        public XElement AddElementWithAttribute(XElement body, string name, string attribute, object attrValue, object value)
        {
            body.Add(new XElement(ns + name, new XAttribute(attribute, attrValue)));
            return body;
        }

        public XElement AddElementWithinElement(XElement body, string parentElement, string elementToAdd, object valueOfElementToAdd)
        {
            body.Element(ns + parentElement).Add(new XElement(ns + elementToAdd, valueOfElementToAdd));          
            return body;
        }

        public string StringifyRequest(XElement body)
        {
            string bodyString = xmlHead + body.ToString();
            return bodyString;
        }


    }
}
