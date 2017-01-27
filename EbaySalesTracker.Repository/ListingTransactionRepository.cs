using EbaySalesTracker.Models;
using EbaySalesTracker.Repository.Helpers;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System;

namespace EbaySalesTracker.Repository
{
    public class ListingTransactionRepository : RepositoryBase<EbaySalesTrackerContext>, IListingTransactionRepository
    {
        private ListingTransactionEngine listingTransactionEngine = new ListingTransactionEngine();

        #region FromDatabase
        public IEnumerable<ListingTransaction> GetTransactionsForListing(long listingId)
        {
            var transactions = DataContext.ListingTransactions.Where(lt => lt.ListingId == listingId).ToList();
            return transactions;
        }
        #endregion



        #region FromEbay
        public ICollection<ListingTransaction> GetListingTransactionsByListingIdFromEbay(long listingId, string userId)
        {
            ApplicationUser user = new ApplicationUser();

            var userContext = new ApplicationDbContext();
            user = userContext.Users.Where(p => p.Id == userId).FirstOrDefault();

            ICollection<ListingTransaction> listingTransactions = listingTransactionEngine.GetListingTransactionByListingIdFromEbay(listingId, user.UserToken);
            
            foreach (var transaction in listingTransactions)
            {              
                    AddListingTransaction(transaction);                
            }
            return listingTransactions;
        }
        #endregion
        #region Helpers
        public void AddListingTransaction(ListingTransaction listingTransaction)
        {
            DataContext.ListingTransactions.AddOrUpdate(listingTransaction);
            DataContext.SaveChanges();
            //return listingTransaction;
        }

        #endregion
    }
}
