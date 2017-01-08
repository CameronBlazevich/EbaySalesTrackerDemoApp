using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EbaySalesTracker.ViewModels
{
    public class ListingsViewModel
    {
        public Listing Listing { get; set; }
        public List<Listing> Listings { get; set; }
        public List<InventoryItem> Items { get; set;}    
        public int SelectedItem { get; set; }
        public int TotalListings { get; set; }
        [Display(Name = "Product")]
        public IEnumerable<SelectListItem> InventoryItems
        {
            get { return new SelectList(Items, "Id", "Description"); }
        }
        //public double Profit { get; set;}
        //public SelectList InventoryItemSelectList { get; set; }

    }
}