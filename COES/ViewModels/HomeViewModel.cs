﻿using System;
using COES.Models;
using System.Data;
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

                // connect to the database
                DatabaseManager dbm = new DatabaseManager();


                
                // CHeck for a customer
                String sqlcheck = " SELECT * from customer WHERE  phone_number = '" + result.ToString() +"'; ";
                int customerMatches=dbm.quickQuery(sqlcheck);

                if (customerMatches > 0)
                {
                    String sql = "SELECT customer_id, " +
                                "       first_name, " +
                                "	    last_name, " +
                                "	    credit_card_name, " +
                                "	    credit_card_number, " +
                                "	    credit_card_expiry, " +
                                "	    street_no, " +
                                "	    street, " +
                                "	    suburb_post_code, " +
                                "	    status " +
                                "FROM   customer " +
                                "WHERE  phone_number = '" + result.ToString() + "'; ";

                    DataTable dt = dbm.query(sql);

                    //assume only 1 valid result
                    DataRow dr = dt.Rows[0];


                    RestaurantManager.CurrentCustomer = new Customer
                    {
                        FirstName = dr["first_name"].ToString(),
                        LastName = dr["last_name"].ToString(),
                        PhoneNumber = result.ToString(),
                        Address = new Address
                        {
                            Number = dr["street_no"].ToString(),
                            PostCode = dr["suburb_post_code"].ToString(),
                            Street = dr["street"].ToString(),
                            Suburb = dr["suburb"].ToString()
                        },
                        Comments = "",          // no comments in schema
                       // Id = dr[""].ToString(),
                        CreditCard = new CreditCard
                        {
                            Number = dr["credit_card_number"].ToString(),
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
                else
                {

                    RestaurantManager.CurrentCustomer = new Customer
                    {
                        FirstName = "",
                        LastName = "",
                        PhoneNumber = result.ToString(),
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