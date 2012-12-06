using COES.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace COES.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class HomeViewModel : ViewModelBase
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private RestaurantManager _restaurantManager;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        public RestaurantManager RestaurantManager
        {
            get { return _restaurantManager; }
            set { Set(() => RestaurantManager, ref _restaurantManager, value); }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        #region --- Commands ---
        //----------------------------------------------------------------------
        public RelayCommand TestCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        #region --- Constructor ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public HomeViewModel()
        {
            InitializeCommands();
            RestaurantManager = new RestaurantManager();
            
            RestaurantManager.Customers.Add(new Customer { FirstName = "Test1", LastName = "Test1LastName" });
            RestaurantManager.Customers.Add(new Customer { FirstName = "Test2", LastName = "Test2LastName" });
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Methods ---
        //----------------------------------------------------------------------
        private void InitializeCommands()
        {
            
            TestCommand = new RelayCommand(Test);
        }

        private void Test()
        {
            //RestaurantManager.Customers.Add(new Customer { FirstName = "Mike", LastName = "Cripps" });
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("EditOrder"));
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}