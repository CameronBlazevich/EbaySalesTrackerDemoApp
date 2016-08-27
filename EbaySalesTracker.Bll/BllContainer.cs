using Microsoft.Practices.Unity;


namespace EbaySalesTracker.Bll
{
    //This is a simplified version of the code shown in the videos
    //The instance of UnityContainer is created in the constructor 
    //rather than checking in the Instance property and performing a lock if needed
    public static class BllContainer
    {
        private static IUnityContainer _Instance;

        static BllContainer()
        {
            _Instance = new UnityContainer();
        }

        public static IUnityContainer Instance
        {
            get
            {
                _Instance.RegisterType<IListingBll, ListingBll>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IInventoryBll, InventoryBll>(new HierarchicalLifetimeManager());
                //    _Instance.RegisterType<IListingDetailRepository, ListingDetailRepository>(new HierarchicalLifetimeManager());
                //    _Instance.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
                //    _Instance.RegisterType<IInventoryRepository, InventoryRepository>(new HierarchicalLifetimeManager());
                return _Instance;
            }
        }
    }
}

