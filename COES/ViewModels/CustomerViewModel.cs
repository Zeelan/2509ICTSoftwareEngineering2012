using System;
using COES.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace COES.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class CustomerViewModel : ViewModelBase
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private Customer _customer;
        private bool _phoneNumberSearchSuccessful = true;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        public Customer Customer
        {
            get { return _customer; }
            set { Set(() => Customer, ref _customer, value); }
        }

        public bool PhoneNumberSearchSuccessful
        {
            get { return _phoneNumberSearchSuccessful; }
            set { Set(() => PhoneNumberSearchSuccessful, ref _phoneNumberSearchSuccessful, value); }
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
        /// Initializes a new instance of the CustomerViewModel class.
        /// </summary>
        public CustomerViewModel()
        {
            InitializeCommands();

            Customer = new Customer();
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Methods ---
        //----------------------------------------------------------------------
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
            if (long.TryParse(Customer.PhoneNumber, out result))
            {
                // DATABASE LOGIC GOES HERE
                // Search database for phone number entered, if a result is returned the Customer object will be filled
                // If nothing exists, a new customer is created and the appropriate textboxes are filled.
                PhoneNumberSearchSuccessful = true;

                // TEST CUSTOMER OBJECT
                Customer = new Customer
                {
                    FirstName = "Michael",
                    LastName = "Cripps",
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
                        Name = Customer.Name
                    },
                    Status = "Y"
                };
            }
            else
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("PhoneNumber"));
            }
            
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

    }
}