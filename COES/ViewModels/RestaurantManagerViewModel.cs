using System.Collections.ObjectModel;
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
    public class RestaurantManagerViewModel : ViewModelBase
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private ObservableCollection<Order> _orders;
        private Order _selectedOrder;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set { Set(() => Orders, ref _orders, value); }
        }
        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set { Set(() => SelectedOrder, ref _selectedOrder, value); }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Commands ---
        //----------------------------------------------------------------------
        public RelayCommand CancelCommand
        {
            get;
            private set;
        }

        public RelayCommand CompleteOrderCommand
        {
            get;
            private set;
        }

        public RelayCommand PayOrderCommand
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
        /// Initializes a new instance of the RestaurantManagerViewModel class.
        /// </summary>
        public RestaurantManagerViewModel()
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
            CancelCommand = new RelayCommand(Cancel);
            CompleteOrderCommand = new RelayCommand(CompleteOrder);
            PayOrderCommand = new RelayCommand(PayOrder);
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<ObservableCollection<Order>>(this, "RestaurantManagerOpened", (m => Orders = m));
        }

        private void PayOrder()
        {
            if (!SelectedOrder.Paid)
            {
                Messenger.Default.Send<Order>(SelectedOrder, "RestaurantManagerPayOrder");
            }
        }

        private void CompleteOrder()
        {
            SelectedOrder.Status = "Y";
            // TODO: update db
            Orders.Remove(SelectedOrder);
        }

        private void Cancel()
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Cancel"), "Navigate");
            NavigatedFrom();
        }

        private void NavigatedFrom()
        {
            Orders = null;
            SelectedOrder = null;
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

    }
}