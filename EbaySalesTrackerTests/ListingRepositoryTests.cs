using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EbaySalesTracker.Repository;
using EbaySalesTracker.Models;
using System.Data.Entity;
using Moq;

namespace EbaySalesTrackerTests
{
    [TestClass]
    public class ListingRepositoryTests
    {
        Mock<IListingRepository> listingRepo = new Mock<IListingRepository>();

        [TestMethod]
        public void CalculateProfitTestValid()
        {
            //Arrange           

            //Act

            //Assert
        }

        

    }
}
