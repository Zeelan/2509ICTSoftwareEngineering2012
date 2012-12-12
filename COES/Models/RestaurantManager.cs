using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace COES.Models
{
    public class RestaurantManager : ObservableObject
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        //private ObservableCollection<Customer> _customers;
        private ObservableCollection<Order> _orders;
        private Menu _menu;
        private ObservableCollection<MenuItem> _menuItems;
        private Customer _currentCustomer;
        private Order _currentOrder;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the list of customers. When a customer has an active order they will be added to the list, 
        /// when their order is no longer active they will be removed.
        /// </summary>
        //public ObservableCollection<Customer> Customers
        //{
        //    get { return _customers; }
        //    set { Set(() => Customers, ref _customers, value); }
        //}

        /// <summary>
        /// Gets or sets the list orders.
        /// </summary>
        public ObservableCollection<Order> Orders
        {
            get { return _orders ?? (_orders = new ObservableCollection<Order>()); }
            set { Set(() => Orders, ref _orders, value); }
        }

        /// <summary>
        /// Gets or sets the current <see cref="Menu"/>.
        /// </summary>
        public Menu Menu
        {
            get { return _menu ?? (_menu = new Menu()); }
            set { Set(() => Menu, ref _menu, value); }
        }

        /// <summary>
        /// Gets or sets all the available <see cref="MenuItem"/>s.
        /// </summary>
        public ObservableCollection<MenuItem> MenuItems
        {
            get { return _menuItems ?? (_menuItems = new ObservableCollection<MenuItem>()); }
            set { Set(() => MenuItems, ref _menuItems, value); }
        }

        /// <summary>
        /// Gets or sets the current <see cref="Customer"/>.
        /// </summary>
        public Customer CurrentCustomer
        {
            get { return _currentCustomer ?? (_currentCustomer = new Customer()); }
            set { Set(() => CurrentCustomer, ref _currentCustomer, value); }
        }

        /// <summary>
        /// Gets or sets the current <see cref="Order"/>.
        /// </summary>
        public Order CurrentOrder
        {
            get { return _currentOrder ?? (_currentOrder = new Order()); }
            set { Set(() => CurrentOrder, ref _currentOrder, value); }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Constructor ---
        //----------------------------------------------------------------------
        public RestaurantManager()
        {
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Methods ---
        //----------------------------------------------------------------------
        public void Populate()
        {
            String sql = "select * from customer_order where paid_status like 'N' ; ";
            DataTable dt = DatabaseManager.Query(sql);

            //clear current
            Orders.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                Order ot = new Order();
                ot.Id = int.Parse(dr["customer_order_id"].ToString());
                ot.Cost = double.Parse(dr["total_cost"].ToString());
                ot.CustomerId = int.Parse(dr["customer_id"].ToString());

                if (dr["delivery_flag"].ToString() == "Y") { ot.Delivery = true; } else { ot.Delivery = false; }
                if (dr["paid_status"].ToString() == "Y") { ot.Paid = true; } else { ot.Paid = false; }
                ot.DateCreated = DateTime.Parse(dr["created_date"].ToString());
                Orders.Add(ot);

            }
        }


        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------
    }
}
