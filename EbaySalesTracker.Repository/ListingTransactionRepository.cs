using EbaySalesTracker.Models;
using EbaySalesTracker.Repository.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace EbaySalesTracker.Repository
{
    public class ListingTransactionRepository : RepositoryBase<EbaySalesTrackerContext>, IListingTransactionRepository
    {
        private ListingTransactionEngine listingTransactionEngine = new ListingTransactionEngine();

        public ICollection<ListingTransaction> GetListingTransactionsByListingIdFromEbay(long listingId, string userId)
        {
            ApplicationUser user = new ApplicationUser();

            var userContext = new ApplicationDbContext();
            user = userContext.Users.Where(p => p.Id == userId).FirstOrDefault();

            ICollection<ListingTransaction> listingTransactions = listingTransactionEngine.GetListingTransactionByListingIdFromEbay(listingId, user.UserToken);
            var existingTransactions = DataContext.ListingTransactions.Where(l => l.ListingId == listingId).ToList();
            foreach (var transaction in listingTransactions)
            {
                if (!existingTransactions.Where(x => x.TransactionId == transaction.TransactionId).Any())
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
