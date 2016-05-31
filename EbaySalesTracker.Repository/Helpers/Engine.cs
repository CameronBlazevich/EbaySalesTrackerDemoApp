using eBay.Service.Core.Soap;
using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EbaySalesTracker.Repository.Helpers
{
    public class Engine
    {
        public string GetAttributeData(XElement entry, string elemName)
        {
            return entry.Descendants().Where(p => p.Name.LocalName == elemName).FirstOrDefault().Value;
        }

        public DateTime GetDateTime(XElement entry, string elemName)
        {
            var input = GetAttributeData(entry, elemName);
            if (input == null) return DateTime.Now; ;

            DateTime value;

            if (DateTime.TryParse(input, out value)) return value;
            return DateTime.Now;
        }

        public long GetLong(XElement entry, string elemName)
        {
            var input = GetAttributeData(entry, elemName);
            if (input == null) return 0L;

            long value;

            if (long.TryParse(input, out value)) return value;
            return 0L;
        }

        public decimal GetDecimal(XElement entry, string elemName)
        {
            var input = GetAttributeData(entry, elemName);
            if (input == null) return 0;

            decimal value;

            if (decimal.TryParse(input, out value)) return value;
            return 0;
        }

        public double GetDouble(XElement entry, string elemName)
        {
            var input = GetAttributeData(entry, elemName);
            if (input == null) return 0;

            double value;

            if (double.TryParse(input, out value)) return value;
            return 0;
        }

        public int GetInt(XElement entry, string elemName)
        {
            var input = GetAttributeData(entry, elemName);
            if (input == null) return 0;

            int value;

            if (int.TryParse(input, out value)) return value;
            return 0;
        }

        public ListingStatusCodeType GetStatusEnum(XElement entry, string elemName)
        {
            var input = GetAttributeData(entry, elemName);
            if (input == null) return 0;

            ListingStatusCodeType value = input.ToEnum<ListingStatusCodeType>();
            return value;
        }

        public AccountDetailEntryCodeType GetDetailTypeEnum(XElement entry, string elemName)
        {
            var input = GetAttributeData(entry, elemName);
            if (input == null) return 0;

            AccountDetailEntryCodeType value = input.ToEnum<AccountDetailEntryCodeType>();
            return value;
        }

    }
}
