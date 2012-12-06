using System;
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
        private ObservableCollection<Customer> _customers;
        private ObservableCollection<Order> _orders;
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
        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set { Set(() => Customers, ref _customers, value); }
        }
        /// <summary>
        /// Gets or sets the list orders.
        /// </summary>
        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set { Set(() => Orders, ref _orders, value); }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Constructor ---
        //----------------------------------------------------------------------
        public RestaurantManager() :base()
        {
            Customers = new ObservableCollection<Customer>();
            Orders = new ObservableCollection<Order>();
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Methods ---
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------
    }
}
