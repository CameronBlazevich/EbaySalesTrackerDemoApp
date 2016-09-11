using EbaySalesTracker.Models;
using EbaySalesTracker.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Repository
{
    public class ListingTransactionRepository : RepositoryBase<EbaySalesTrackerContext>, IListingTransactionRepository
    {
        private ListingTransactionEngine listingTransactionEngine = new ListingTransactionEngine();

        public ICollection<ListingTransaction> GetListingTransactionsByListingIdFromEbay(long listingId, string userToken)
        {
            ApplicationUser user = new ApplicationUser();
            var userContext = new ApplicationDbContext();

            user = userContext.Users.Where(p => p.Id == user.Id).FirstOrDefault();

            ICollection<ListingTransaction> listingTransactions = listingTransactionEngine.GetListingTransactionByListingIdFromEbay(listingId, user.UserToken);
            var existingTransactions = DataContext.ListingTransactions.Where(l => l.ListingId == listingId).ToList();
            foreach (var transaction in listingTransactions)
            {
                var exists = existingTransactions.Where(x => x.TransactionId == transaction.TransactionId).Any();
                if (!exists)
                {
                    AddListingTransaction(transaction);
                }
            }
            return listingTransactions;
        }
        public void AddListingTransaction(ListingTransaction listingTransaction)
        {
            DataContext.ListingTransactions.Add(listingTransaction);
            DataContext.SaveChanges();
            //return listingTransaction;
        }
    }
}
