using EbaySalesTracker.Models;
using EbaySalesTracker.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EbaySalesTracker.Repository
{
    public class ListingRepository : RepositoryBase<EbaySalesTrackerContext>, IListingRepository
    {      
        //whats the best place to new up an engine?
        private ListingEngine engine = new ListingEngine();
        public Listing GetListingByItemIdFromEbay(long itemId, string userToken)
        {
            using (DataContext)
            {
                var listing = DataContext.Listings.Where(s => s.ItemId == itemId).SingleOrDefault();
                if (listing == null)
                {
                    listing = engine.GetListingByItemIdFromEbay(itemId, userToken);
                    //listing.TotalNetFees = DataContext.ListingDetails.Sum(x => x.NetAmount);
                    DataContext.Listings.Add(listing);

                    var opStatus = Save(listing);
                    if (!opStatus.Status)
                    {
                        listing = new Listing { Title = "Error getting listing." };
                    }
                }
                return listing;
            }
        }


        public List<Listing> GetAllListingsSinceDateFromEbay(DateTime startDate, string userId)
        {
            IListingDetailRepository detailRepo = new ListingDetailRepository();
            ApplicationUser user = new ApplicationUser();
            var userContext = new ApplicationDbContext();

            user = userContext.Users.Where(p => p.Id == userId).FirstOrDefault();
                 
            
            List<Listing> listings = engine.GetAllListingsSinceDateFromEbay(startDate, user.UserToken);


            if (listings != null && listings.Count > 0)
            {
                using (DataContext)
                {
                    foreach (var listing in listings)
                    {
                        //check if the listing already exists in db
                        var existingListing = DataContext.Listings.SingleOrDefault(l => l.ItemId == listing.ItemId);
                        if (existingListing != null)
                        {
                            existingListing.CurrentPrice = listing.CurrentPrice;
                            existingListing.ListingStatus = listing.ListingStatus;
                            existingListing.QuantitySold = listing.QuantitySold;
                            existingListing.Type = listing.Type;
                            DataContext.SaveChanges();                          
                        }
                        else
                        {
                            //should figure out how to better implement opStatus/BaseRepo
                            listing.UserId = userId;
                            DataContext.Listings.Add(listing);
                            var opStatus = Save(listing);
                            if (!opStatus.Status)
                            {
                                listing.Title = "Error saving listing.";
                            }
                        }
                        UpdateFeesById(listing.ItemId, user.UserToken);
                    }
                }
            }

            return listings;

        }

        public List<Listing> GetListingsByEndDateFromEbay(DateTime endDateFrom, DateTime endDateTo, string userToken)
        {
            List<Listing> listings = engine.GetListingsByEndDateFromEbay(endDateFrom, endDateTo, userToken);

            if (listings != null && listings.Count > 0)
            {
                using (DataContext)
                {
                    foreach(var listing in listings)
                    {
                        //check if the listing already exists in db
                        var exists = DataContext.Listings.Where(l => l.ItemId == listing.ItemId).Any();
                        if(!exists)
                        {
                            DataContext.Listings.Add(listing);
                            var opStatus = Save(listing);
                            if (!opStatus.Status)
                            {
                                listing.Title = "Error saving listing.";
                            }

                        }
                    }
                }
            }

            return listings;

        }

        public void UpdateFeesById(long itemId, string userToken)
        {
            IListingDetailRepository detailRepo = new ListingDetailRepository();
            Listing listing = DataContext.Listings.Where(x => x.ItemId == itemId).FirstOrDefault();
            double netFees = 0;
            double grossFees = 0;
            
            //update db with new listing details and get them to use for fee calculation
            var listingDetails = detailRepo.GetListingDetailsByItemIdFromEbay(itemId, userToken);

            foreach (var detail in listingDetails)
            {
                netFees += detail.NetAmount;
                grossFees += detail.GrossAmount;
            }
            listing.TotalNetFees = Math.Round(netFees,2);
            listing.TotalGrossFees = Math.Round(grossFees,2);
            DataContext.SaveChanges();
    }

        public List<Listing> GetListingsByStartDateFromEbay(DateTime startDateFrom, DateTime startDateTo, string userId, string userToken)
        {
            List<Listing> listings = engine.GetListingsByStartDateFromEbay(startDateFrom, startDateTo,userToken);

            if (listings != null && listings.Count > 0)
            {
                using (DataContext)
                {
                    foreach (var listing in listings)
                    {
                        //check if the listing already exists in db
                        var exists = DataContext.Listings.Where(l => l.ItemId == listing.ItemId).Any();
                        if (!exists)
                        {
                            listing.UserId = userId;
                            DataContext.Listings.Add(listing);
                            var opStatus = Save(listing);
                            if (!opStatus.Status)
                            {
                                listing.Title = "Error saving listing.";
                            }

                        }
                    }
                }
            }
            return listings;
        }


        #region FromDb

        public IEnumerable<Listing> GetListingsByEndDate(DateTime startDate, DateTime endDate, string userId)
        {           
            return DataContext.Listings.Where(x => x.EndDate >= startDate && x.EndDate >= endDate).ToList();
        }
       
        public List<Listing> GetAllListingsByUser(string userId)
        {
            return DataContext.Listings.Where(x => x.UserId == userId).ToList();            
        }

        public Listing GetListingById(long id)
        {
            return DataContext.Listings.Find(id);           
        }

        public Listing AddListing(Listing listing)
        {
            DataContext.Listings.Add(listing);
            DataContext.SaveChanges();
            return listing;
        }

        public void DeleteListing(long id)
        {
            Listing listing = DataContext.Listings.Find(id);
            DataContext.Listings.Remove(listing);
            DataContext.SaveChanges();
        }

        public void AssociateInventoryItem(long listingId, int inventoryItemId)
        {
            var listing = DataContext.Listings.Where(l => l.ItemId == listingId).FirstOrDefault();
            listing.InventoryItemId = inventoryItemId;
            DataContext.SaveChanges();
        }

        public void DissociateInventoryItem(long listingId)
        {
            var listing = DataContext.Listings.Where(l => l.ItemId == listingId).FirstOrDefault();
            listing.InventoryItemId = null;
            DataContext.SaveChanges();
        }
        public void UpdateListing(Listing listing)
        {
            DataContext.Entry(listing).State = System.Data.Entity.EntityState.Modified;
            DataContext.SaveChanges();
        }

        public IEnumerable<Listing> GetSoldListingsByInventoryItem(int inventoryItemId, string userId)
        {
            return DataContext.Listings.Where(x => x.InventoryItemId == inventoryItemId && x.TotalNetFees != 0 && x.QuantitySold > 0 && x.UserId == userId).OrderBy(x => x.EndDate).ToList(); 
        }
             
        #endregion
    }
}
