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
        private Order _order;
        
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

        public Order Order
        {
            get { return _order; }
            set { Set(() => Order, ref _order, value); }
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

            // TEST CUSTOMER OBJECT
            //Customer = new Customer
            //{
            //    FirstName = "Michael",
            //    LastName = "Cripps",
            //    Address = new Address
            //    {
            //        Number = 83,
            //        PostCode = 4164,
            //        Street = "Morris Circuit",
            //        Suburb = "Thornlands"
            //    },
            //    Comments = "Test comment",
            //    Id = 1,
            //    CreditCard = new CreditCard
            //    {
            //        Number = 21438124,
            //    },
            //    Status = "Y"
            //};
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
            Messenger.Default.Register<GenericMessage<Customer>>(this, "CustomerCreated", m => this.Customer = m.Content);
        }

        /// <summary>
        /// Creates a new <see cref="Order"/> then uses the <see cref="Messenger"/> to notify the main ViewModel that th
        /// </summary>
        private void CreateOrder()
        {
            if (CheckCustomerInfo())
            {
                Order = new Order
                {
                    CustomerId = Customer.Id
                };

                // Sends a message to navigate to the Order View.
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("NavigateOrder"), "Navigate");
                // Sends message with the newly created order, the Order ViewModel will listen for this message.
                Messenger.Default.Send<GenericMessage<Order>>(new GenericMessage<Order>(Order), "OrderCreated");
                this.Cleanup();
            }
        }

        /// <summary>
        /// Checks if the appropriate <see cref="Customer"/> information is available.
        /// </summary>
        /// <returns>True if all information is available, else returns false.</returns>
        private bool CheckCustomerInfo()
        {
            if (Customer.FirstName == null)
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ErrorFirstName"), "Error");
                return false;
            }
            else if (Customer.LastName == null)
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
            else if (Customer.Address.Street == null)
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ErrorAddressStreet"), "Error");
                return false;
            }
            else if (Customer.Address.Suburb == null)
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

        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

    }
}