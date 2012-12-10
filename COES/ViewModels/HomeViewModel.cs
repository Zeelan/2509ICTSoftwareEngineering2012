using System;
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
        private string _phoneNumber;
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

        public string PhoneNumber
        {
            get { return _phoneNumber;}
            set { Set(() => PhoneNumber, ref _phoneNumber, value);}
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        #region --- Commands ---
        //----------------------------------------------------------------------
        public RelayCommand SearchPhoneNumberCommand
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
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Methods ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Initializes the commands associated with this ViewModel.
        /// </summary>
        private void InitializeCommands()
        {
            SearchPhoneNumberCommand = new RelayCommand(SearchPhoneNumber);
        }

        /// <summary>
        /// Searches the given phone number to see if a <see cref="Customer"/> already exists.
        /// </summary>
        private void SearchPhoneNumber()
        {
            long result;
            if (long.TryParse(PhoneNumber, out result))
            {
                // TODO: Database logic, search for existing phone number.
 
                // If exists
                //

                // else
                //



                //
                // Testing
                //
                RestaurantManager.CurrentCustomer = new Customer
                {
                    FirstName = "Michael",
                    LastName = "Cripps",
                    PhoneNumber = result.ToString(),
                    Address = new Address
                    {
                        Number = 83,
                        PostCode = 4164,
                        Street = "Morris Circuit",
                        Suburb = "Thornlands"
                    },
                    Comments = "Test comment",
                    Id = 1,
                    CreditCard = new CreditCard
                    {
                        Number = 21438124,
                    },
                    Status = "Y"
                };
                RestaurantManager.CurrentCustomer.CreditCard.Name = RestaurantManager.CurrentCustomer.Name;
                //
                // Testing
                //
                
                // Sends a message notifying that the current customer has changed (been created).
                Messenger.Default.Send<Customer>(RestaurantManager.CurrentCustomer, "CreateCustomer");
                // Sends a message to navigate to the Customer View.
                Messenger.Default.Send <NotificationMessage>(new NotificationMessage("NavigateCustomer"), "Navigate");
                NavigatedFrom();
            }
            else // Incorect number format.
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ErrorPhoneNumber"), "Error");
            }
        }

        private void NavigatedFrom()
        {
            PhoneNumber = String.Empty;
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