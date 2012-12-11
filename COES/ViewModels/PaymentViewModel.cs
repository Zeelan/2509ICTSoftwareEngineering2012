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
    public class PaymentViewModel : ViewModelBase
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private Order _order;
        private Customer _customer;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the <see cref="Order"/> to be paid.
        /// </summary>
        public Order Order
        {
            get { return _order; }
            set { Set(() => Order, ref _order, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="Customer"/>.
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
        public RelayCommand PayNowCommand
        {
            get;
            private set;
        }

        public RelayCommand PayLaterCommand
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
        /// Initializes a new instance of the PaymentViewModel class.
        /// </summary>
        public PaymentViewModel()
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
        /// Initalizes the commands associated with this ViewModel.
        /// </summary>
        private void InitializeCommands()
        {
            PayNowCommand = new RelayCommand(PayNow);
            PayLaterCommand = new RelayCommand(PayLater);
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<Order>(this, "PaymentReady", m => Order = m);
            Messenger.Default.Register<Customer>(this, "PaymentReady", m => Customer = m);
        }

        private void PayNow()
        {
            Order.Paid = true;
            // TODO: db logic, update order in db
            Messenger.Default.Send<Order>(Order, "OrderComplete");
            PaymentComplete();
        }

        private void PayLater()
        {
            Order.Paid = false;
            // TODO: db logic, update order in db
            PaymentComplete();
        }

        private void PaymentComplete()
        { 
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("PaymentComplete"), "Navigate");
            NavigatedFrom();
        }

        /// <summary>
        /// Called when the ViewModel is no longer the current ViewModel.
        /// </summary>
        private void NavigatedFrom()
        {
            Order = null;
            Customer = null;
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

    }
}