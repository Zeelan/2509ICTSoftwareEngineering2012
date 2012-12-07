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
        private void InitializeCommands()
        {
            CreateOrderCommand = new RelayCommand(CreateOrder);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void RegisterMessages()
        {
            // Registers a message to update the customer when it has been created.
            Messenger.Default.Register<Customer>(this, "CustomerCreated", m => this.Customer = m);
        }

        /// <summary>
        /// Creates a new <see cref="Order"/> then uses the <see cref="Messenger"/> to notify the main ViewModel that th
        /// </summary>
        private void CreateOrder()
        {
            if (CheckCustomerInfo())
            {
                // Sends message with the customer id to create the new Order.
                Messenger.Default.Send<int>(Customer.Id, "CreateOrder");
                // Sends a message to navigate to the Order View.
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("NavigateOrder"), "Navigate");
                //this.Cleanup();
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
            // NOTE: FIX THIS SO IT ERROR CHECKS FOR NON INT
            else if (Customer.Address.Number == 0)
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
            // NOTE: FIX THIS SO IT ERRO CHECKS FOR NON INT
            else if (Customer.Address.PostCode == 0)
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