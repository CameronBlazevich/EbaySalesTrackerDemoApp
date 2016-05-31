using EbaySalesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbaySalesTracker.Repository
{
    public interface IInventoryRepository
    {
        List<InventoryItem> GetInventoryItemsByUser(string userId);
    }
}
