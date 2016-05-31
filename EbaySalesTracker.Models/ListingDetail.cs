using eBay.Service.Core.Soap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Models
{
    public class ListingDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long RefNumber { get; set; }
        [Required]
        public long ItemId { get; set; }
        [Required]
        public AccountDetailEntryCodeType Type { get; set; }
        public string Description { get; set; }
        public DateTime PostDate { get; set; }
        public double GrossAmount { get; set; }
        public double NetAmount { get; set; }
        public string Memo { get; set; }
        
    }
}
