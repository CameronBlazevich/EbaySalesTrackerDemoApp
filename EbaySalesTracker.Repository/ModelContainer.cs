using Microsoft.Practices.Unity;

namespace EbaySalesTracker.Repository
{
    //This is a simplified version of the code shown in the videos
    //The instance of UnityContainer is created in the constructor 
    //rather than checking in the Instance property and performing a lock if needed
    public static class ModelContainer
    {
        private static IUnityContainer _Instance;

        static ModelContainer()
        {
            _Instance = new UnityContainer();
        }

        public static IUnityContainer Instance
        {
            get
            {
                //_Instance.RegisterTypes(
                //    AllClasses.FromLoadedAssemblies(),
                //    WithMappings.FromMatchingInterface,                   
                //    WithName.Default,
                //    WithLifetime.Hierarchical);
                _Instance.RegisterType<IListingRepository, ListingRepository>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IListingDetailRepository, ListingDetailRepository>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IListingTransactionRepository, ListingTransactionRepository>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IInventoryRepository, InventoryRepository>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<IWebHookRepository, WebHookRepository>(new HierarchicalLifetimeManager());                

                //_Instance.RegisterType<IMarketsAndNewsRepository, MarketsAndNewsRepository>(new HierarchicalLifetimeManager());
                return _Instance;
            }
        }
    }
}

