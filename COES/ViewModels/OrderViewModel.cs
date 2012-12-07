using COES.Models;
using GalaSoft.MvvmLight;
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
        #region --- Constructor ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the AddOrderViewModel class.
        /// </summary>
        public OrderViewModel()
        {
            RegisterMessages();
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Methods ---
        //----------------------------------------------------------------------
        private void RegisterMessages()
        {
            Messenger.Default.Register<Menu>(this, "OrderCreated", m => this.Menu = m);
            Messenger.Default.Register<Order>(this, "OrderCreated", m => this.Order = m);
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

    }
}