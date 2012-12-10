using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ApplicationViewModel : ViewModelBase
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private RestaurantManager _restaurantManager;
        private ViewModelBase _currentViewModel;

        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the <see cref="RestaurantManager"/> which handles some crap.
        /// </summary>
        public RestaurantManager RestaurantManager
        {
            get { return _restaurantManager ?? (_restaurantManager = new RestaurantManager()); }
            set { Set(() => RestaurantManager, ref _restaurantManager, value); }
        }

        /// <summary>
        /// Gets or sets the current ViewModel associated with the current View.
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { Set(() => CurrentViewModel, ref _currentViewModel, value); }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Constructor ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the ApplicationViewModel class.
        /// </summary>
        public ApplicationViewModel()
        {
            CurrentViewModel = ViewModelLocator.HomeStatic;

            RegisterMessages();

            RestaurantManager.Menu = LoadMenu();
            RestaurantManager.Menu.MenuItems = LoadMenuItems();

            //
            // Testing
            //
            RestaurantManager.Menu.MenuItems.Add(new MenuItem { Name = "Test item 1" });
            RestaurantManager.Menu.MenuItems.Add(new MenuItem { Name = "Test item 2" });
            //
            // Testing
            //
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Methods ---
        //----------------------------------------------------------------------
        private void RegisterMessages()
        {
            // Registers a message for when the current customer is changed.
            Messenger.Default.Register<Customer>(this, "CreateCustomer", m => RestaurantManager.CurrentCustomer = m);

            // Registers a message for when the current order is changed.
            //Messenger.Default.Register<GenericMessage<Order>>(this, m => RestaurantManager.CurrentOrder = m.Content);

            // Registers a message for when an order is to be created, using the customer's id.
            Messenger.Default.Register<int>(this, "CreateOrder", m => RestaurantManager.CurrentOrder = new Order(m));

            // Registers a message for when an order is confirmed it is added to the list of active orders.
            Messenger.Default.Register<Order>(this, "OrderConfirmed", m => OrderConfirmed(m));

            


            // Registers the notification messages (using this for changing the Views).
            Messenger.Default.Register<NotificationMessage>(this, "Navigate", m => Navigate(m));
        }

        /// <summary>
        /// Loads the <see cref="Menu"/> from the database.
        /// </summary>
        /// <returns>The menu.</returns>
        private Menu LoadMenu()
        {
            // TODO: Load menu from database.
            return null;
        }

        /// <summary>
        /// Loads the list of <see cref="MenuItem"/>s from the database.
        /// </summary>
        /// <returns>A list of menu items.</returns>
        private ObservableCollection<MenuItem> LoadMenuItems()
        {
            // TODO: Load menu items from database.
            return null;
        }

        /// <summary>
        /// Adds the order to the list of Orders in the RestaurantManager, then removes the current order and customer.
        /// </summary>
        /// <param name="order">The Order to be added.</param>
        private void OrderConfirmed(Order order)
        {
            RestaurantManager.Orders.Add(order);
            RestaurantManager.CurrentOrder = null;
            RestaurantManager.CurrentCustomer = null;
        }

        private void Navigate(NotificationMessage m)
        {
            switch (m.Notification)
            {
                case ("NavigateCustomer"):
                    {
                        CurrentViewModel = ViewModelLocator.CustomerStatic;
                        Messenger.Default.Send<Customer>(RestaurantManager.CurrentCustomer, "CustomerCreated");
                        break;
                    }
                case ("NavigateOrder"):
                    {
                        CurrentViewModel = ViewModelLocator.OrderStatic;
                        Messenger.Default.Send<Menu>(RestaurantManager.Menu, "OrderCreated");
                        Messenger.Default.Send<Order>(RestaurantManager.CurrentOrder, "OrderCreated");
                        break;
                    }
                case ("NavigatePayment"):
                    {

                        break;
                    }
                case ("Cancel"):
                    {
                        CurrentViewModel = ViewModelLocator.HomeStatic;
                        break;
                    }

            }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

    }
}