using System;
using System.Collections.Generic;
using COES.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

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
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the current <see cref="Customer"/>.
        /// </summary>
        public Customer Customer
        {
            get { return _customer; }
            set { Set(() => Customer, ref _customer, value); }
        }

        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Commands ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Called when the Create Order button is clicked.
        /// </summary>
        public RelayCommand CreateOrderCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Called when the Cancel button is clicked.
        /// </summary>
        public RelayCommand CancelCommand
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
            RegisterMessages();
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
            CreateOrderCommand = new RelayCommand(CreateOrder);
            CancelCommand = new RelayCommand(Cancel);
        }

        /// <summary>
        /// Registers the messages associated with this ViewModel.
        /// </summary>
        private void RegisterMessages()
        {
            // Registers a message to update the customer when it has been created.
            Messenger.Default.Register<Customer>(this, "CustomerCreated", m => this.Customer = m);
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOrUpdateCustomer()
        {

            Dictionary<String, String> cust = new Dictionary<string, string>();

            cust.Add("first_name",this.Customer.FirstName);
            cust.Add("last_name",this.Customer.LastName);
            cust.Add("phone_number",this.Customer.PhoneNumber);
            cust.Add("credit_card_name",this.Customer.CreditCard.Name);
            cust.Add("credit_card_number",this.Customer.CreditCard.Number);
            //cust.Add("credit_card_expiry",this.Customer.CreditCard.Expiry);
            cust.Add("street_no",this.Customer.Address.Number);
            cust.Add("street",this.Customer.Address.Street);
            cust.Add("suburb_post_code",this.Customer.Address.PostCode);
            cust.Add("status",this.Customer.Status);
            cust.Add("suburb", this.Customer.Address.Suburb);
            cust.Add("comments", this.Customer.Comments);
            

            if (this._customer.Id == 0)
            {
                DatabaseManager.insert("customer", cust);
            }
            else
            {
                DatabaseManager.update("customer", cust, String.Format(" customer_id ={0} LIMIT 1; ", this.Customer.Id.ToString()));
            }               
        }

        /// <summary>
        /// Creates a new <see cref="Order"/> then uses the <see cref="Messenger"/> to notify the main ViewModel that th
        /// </summary>
        private void CreateOrder()
        {
            if (CheckCustomerInfo())
            {
                this.AddOrUpdateCustomer();

                // Sends message with the customer id to create the new Order.
                Messenger.Default.Send<int>(Customer.Id, "CreateOrder");
                // Sends a message to navigate to the Order View.
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("NavigateOrder"), "Navigate");
            }
        }

        /// <summary>
        /// Checks if the appropriate <see cref="Customer"/> information is available.
        /// </summary>
        /// <returns>True if all information is available, else returns false.</returns>
        private bool CheckCustomerInfo()
        {
            if (Customer.FirstName == null || Customer.FirstName == String.Empty)
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ErrorFirstName"), "Error");
                return false;
            }
            else if (Customer.LastName == null || Customer.LastName == String.Empty)
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ErrorLastName"), "Error");
                return false;
            }
            // CHANGED TO CHECK FOR 
            else if (Customer.Address.Number == "")
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ErrorAddressNumber"), "Error");
                return false;
            }
            else if (Customer.Address.Street == null || Customer.Address.Street == String.Empty)
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ErrorAddressStreet"), "Error");
                return false;
            }
            else if (Customer.Address.Suburb == null || Customer.Address.Suburb == String.Empty)
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ErrorAddressSuburb"), "Error");
                return false;
            }
            //          CHANGED TO CHECK FOR LENGTH OF 4
            else if (Customer.Address.PostCode.ToString().Length<4)
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ErrorAddressPostCode"), "Error");
                return false;
            }
            else
                return true;
        }

        private void Cancel()
        {
            NavigatedFrom();
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Cancel"), "Navigate");
        }

        private void NavigatedFrom()
        {
            Customer = null;
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

    }
}