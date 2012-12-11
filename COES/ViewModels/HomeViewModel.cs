using System;
using COES.Models;
using System.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

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

        public RelayCommand ReportingCommand
        {
            get;
            private set;
        }

        public RelayCommand EditMenuCommand
        {
            get;
            private set;
        }

        public RelayCommand RestaurantManagerCommand
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
            ReportingCommand = new RelayCommand(OpenReporting);
            EditMenuCommand = new RelayCommand(OpenEditMenu);
            RestaurantManagerCommand = new RelayCommand(OpenRestaurantManager);
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


                
                // CHeck for a customer
                String sqlcheck = "phone_number like '" + this.PhoneNumber +"'; ";
                int customerMatches = DatabaseManager.CountQuery("customer", sqlcheck);

                if (customerMatches > 0)
                {
                    String sql = "SELECT * from customer WHERE  phone_number like '" + this.PhoneNumber.ToString() + "'; ";

                    DataTable dt = DatabaseManager.Query(sql);

                    //assume only 1 valid result
                    DataRow dr = dt.Rows[0];

                    RestaurantManager.CurrentCustomer = new Customer
                    {
                        FirstName = dr["first_name"].ToString(),
                        LastName = dr["last_name"].ToString(),
                        PhoneNumber = this.PhoneNumber,
                        Id = int.Parse(dr["customer_id"].ToString()),
                        Address = new Address
                        {
                            Number = dr["street_no"].ToString(),
                            PostCode = dr["suburb_post_code"].ToString(),
                            Street = dr["street"].ToString(),
                            Suburb = dr["suburb"].ToString()
                        },
                        Comments = dr["comments"].ToString(),          // no comments in schema
                        CreditCard = new CreditCard
                        {
                            Number = dr["credit_card_number"].ToString(),
                        },
                        Status = dr["status"].ToString()
                    };
                    RestaurantManager.CurrentCustomer.CreditCard.Name = RestaurantManager.CurrentCustomer.Name;

                    // Sends a message notifying that the current customer has changed (been created).
                    Messenger.Default.Send<Customer>(RestaurantManager.CurrentCustomer, "CreateCustomer");
                    // Sends a message to navigate to the Customer View.
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage("NavigateCustomer"), "Navigate");
                    NavigatedFrom();

                }
                else
                {

                    RestaurantManager.CurrentCustomer = new Customer
                    {
                        FirstName = "",
                        LastName = "",
                        PhoneNumber = this.PhoneNumber,
                        Address = new Address
                        {
                            Number = "",
                            PostCode = "",
                            Street = "",
                            Suburb = ""
                        },
                        Comments = "",
                        Id = 0,
                        CreditCard = new CreditCard
                        {
                            Number = "",
                        },
                        Status = "Y"
                    };
                    RestaurantManager.CurrentCustomer.CreditCard.Name = RestaurantManager.CurrentCustomer.Name;

                    // Sends a message notifying that the current customer has changed (been created).
                    Messenger.Default.Send<Customer>(RestaurantManager.CurrentCustomer, "CreateCustomer");
                    // Sends a message to navigate to the Customer View.
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage("NavigateCustomer"), "Navigate");
                    NavigatedFrom();

                }


 



            }
            else // Incorect number format.
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ErrorPhoneNumber"), "Error");
            }
        }

        private void OpenReporting()
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("NavigateReporting"), "Navigate");
            NavigatedFrom();
        }

        private void OpenEditMenu()
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("NavigateEditMenu"), "Navigate");
            NavigatedFrom();
        }

        private void OpenRestaurantManager()
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("NavigateRestaurantManager"), "Navigate");
            NavigatedFrom();
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