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
    public class OrderViewModel : ViewModelBase
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private Menu _menu;
        private Order _order;
        private MenuItem _currentMenuItem;
        private MenuItem _currentOrderItem;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the <see cref="Menu"/> for the <see cref="Order"/>.
        /// </summary>
        public Menu Menu
        {
            get { return _menu ?? (_menu = new Menu()); }
            set { Set(() => Menu, ref _menu, value); }
        }

        /// <summary>
        /// Gets or sets the current <see cref="Order"/>.
        /// </summary>
        public Order Order
        {
            get { return _order ?? (_order = new Order()); }
            set { Set(() => Order, ref _order, value); }
        }

        /// <summary>
        /// Gets or sets the current <see cref="MenuItem"/> on the menu.
        /// </summary>
        public MenuItem CurrentMenuItem
        {
            get { return _currentMenuItem ?? (_currentMenuItem = new MenuItem()); }
            set { Set(() => CurrentMenuItem, ref _currentMenuItem, value); }
        }

        /// <summary>
        /// Gets or sets the current <see cref="MenuItem"/> on the order.
        /// </summary>
        public MenuItem CurrentOrderItem
        {
            get { return _currentOrderItem ?? (_currentOrderItem = new MenuItem()); }
            set { Set(() => CurrentOrderItem, ref _currentOrderItem, value); }
        }

        
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Commands ---
        //----------------------------------------------------------------------
        public RelayCommand AddToOrderCommand
        {
            get;
            private set;
        }

        public RelayCommand RemoveFromOrderCommand
        {
            get;
            private set;
        }

        public RelayCommand ConfirmOrderCommand
        {
            get;
            private set;
        }

        public RelayCommand CancelOrderCommand
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
        /// Initializes a new instance of the OrderViewModel class.
        /// </summary>
        public OrderViewModel()
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
            AddToOrderCommand = new RelayCommand(AddToOrder);
            RemoveFromOrderCommand = new RelayCommand(RemoveFromOrder);
            ConfirmOrderCommand = new RelayCommand(ConfirmOrder);
            CancelOrderCommand = new RelayCommand(CancelOrder);
        }
        /// <summary>
        /// Registers the messages associated with this ViewModel.
        /// </summary>
        private void RegisterMessages()
        {
            Messenger.Default.Register<Menu>(this, "OrderCreated", m => this.Menu = m);
            Messenger.Default.Register<Order>(this, "OrderCreated", m => this.Order = m);
        }

        /// <summary>
        /// Adds a <see cref="MenuItem"/> to the <see cref="Order"/>.
        /// </summary>
        private void AddToOrder()
        {
            
            if (CurrentMenuItem != null)
            {
                if (Order.MenuItems.ContainsKey(CurrentMenuItem))
                {
                    Order.MenuItems[CurrentMenuItem]++;
                }
                else
                    Order.MenuItems.Add(CurrentMenuItem, 1);
                Order.Cost += CurrentMenuItem.Cost;
            }
        }

        /// <summary>
        /// Removes a <see cref="MenuItem"/> from the <see cref="Order"/>.
        /// </summary>
        private void RemoveFromOrder()
        {
            if (CurrentOrderItem != null)
                Order.MenuItems.Remove(CurrentOrderItem);
        }

        /// <summary>
        /// Confirms the order and updates the database.
        /// </summary>
        private void ConfirmOrder()
        {
            if (Order.MenuItems.Count == 0)
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ErrorOrder"), "Error");
            }
            else
            {
                NavigatedFrom();
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("NavigatePayment"), "Navigate");
            }
        }

        /// <summary>
        /// Cancels the order.
        /// </summary>
        private void CancelOrder()
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Cancel"), "Navigate");
            NavigatedFrom();
        }

        /// <summary>
        /// Called when the ViewModel is no longer the current ViewModel.
        /// </summary>
        private void NavigatedFrom()
        {
            Menu = null;
            Order = null;
            CurrentMenuItem = null;
            CurrentOrderItem = null;
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

    }
}