using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using COES.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Data;
using System.Windows;

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
           // RestaurantManager.MenuItems.Add(new MenuItem { Name = "Test item 1", Cost = 1 });
            //RestaurantManager.MenuItems.Add(new MenuItem { Name = "Test item 2", Cost = 2 });

           // foreach (MenuItem menuItem in RestaurantManager.MenuItems)
            //    RestaurantManager.Menu.MenuItems.Add(menuItem);
          //  RestaurantManager.MenuItems.Add(new MenuItem { Name = "Test item 3", Cost = 3 });
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

            // Registers a message for when an order is to be created, using the customer's id.
            Messenger.Default.Register<int>(this, "CreateOrder", m => OrderCreated(m));

            //Messenger.Default.Register<Order>(this, "PaymentComplete", 

            


            // Registers the notification messages (using this for changing the Views).
            Messenger.Default.Register<NotificationMessage>(this, "Navigate", m => Navigate(m));
        }

        /// <summary>
        /// Loads the <see cref="Menu"/> from the database.
        /// </summary>
        /// <returns>The menu.</returns>
        private Menu LoadMenu()
        {
            DatabaseManager dbm = new DatabaseManager();

            String sql = "select * from menu;";
            DataTable dt = dbm.query(sql);

            // ok i'll use a foreach :-(
            foreach (DataRow dr in dt.Rows)
            {
                MenuItem mi = new MenuItem();
                mi.loadID(int.Parse(dr["menu_id"].ToString()));

                MessageBox.Show(mi.Name.ToString());
                RestaurantManager.Menu.MenuItems.Add(mi);
            }



            return null;
        }


        /// <summary>
        /// Loads the list of <see cref="MenuItem"/>s from the database.
        /// </summary>
        /// <returns>A list of menu items.</returns>
        private ObservableCollection<MenuItem> LoadMenuItems()
        {
            // TODO: Load menu items from database.

            //RestaurantManager.MenuItems.Add(new MenuItem { Name = "Test item 1", Cost = 1 });


            //populate the menu items

            DatabaseManager dbm = new DatabaseManager();

            String sql = "select * from menu_item ;";
            DataTable dt = dbm.query(sql);

            // ok i'll use a foreach :-(
            foreach (DataRow dr in dt.Rows)
            {

                MenuItem mi = new MenuItem
                {
                    Cost=double.Parse(dr["item_cost"].ToString()),
                    Id= int.Parse(dr["menu_item_id"].ToString()),
                    Description= dr["description"].ToString(),
                    Name=dr["menu_item_name"].ToString()
                };

                this.RestaurantManager.MenuItems.Add(mi);

            }
            
            return null;
        }

        private void OrderCreated(int customerId)
        {
            RestaurantManager.CurrentOrder = new Order(customerId);
            RestaurantManager.Orders.Add(RestaurantManager.CurrentOrder);
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
                        CurrentViewModel = ViewModelLocator.PaymentStatic;
                        Messenger.Default.Send<Order>(RestaurantManager.CurrentOrder, "PaymentReady");
                        Messenger.Default.Send<Customer>(RestaurantManager.CurrentCustomer, "PaymentReady");
                        break;
                    }
                case ("PaymentComplete"):
                    {
                        CurrentViewModel = ViewModelLocator.HomeStatic;
                        break;
                    }
                case ("NavigateReporting"):
                    {
                        CurrentViewModel = ViewModelLocator.ReportingStatic;
                        break;
                    }
                case ("NavigateEditMenu"):
                    {
                        CurrentViewModel = ViewModelLocator.EditMenuStatic;
                        Messenger.Default.Send<Menu>(RestaurantManager.Menu, "EditMenu");
                        Messenger.Default.Send<ObservableCollection<MenuItem>>(RestaurantManager.MenuItems, "EditMenu");
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