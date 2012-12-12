using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Data;
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
           // Populate();

          
        }


        public void Populate()
        {
            String sql="select * from customer_order where paid_status like 'N' ; ";
            DataTable dt = DatabaseManager.Query(sql);

            //clear current
            Orders.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                Order ot = new Order();
                ot.Id = int.Parse(dr["customer_order_id"].ToString());
                ot.Cost = double.Parse(dr["total_cost"].ToString());
                ot.CustomerId= int.Parse(dr["customer_id"].ToString());

                if (dr["delivery_flag"].ToString() == "Y") { ot.Delivery = true; } else { ot.Delivery = false; }
                if (dr["paid_status"].ToString() == "Y") { ot.Paid = true; } else { ot.Paid = false; }
                ot.DateCreated = DateTime.Parse(dr["created_date"].ToString());
                Orders.Add(ot);

            }


        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<ObservableCollection<Order>>(this, "RestaurantManagerOpened", (m => Orders = m));
        }

        private void PayOrder()
        {
            if (!SelectedOrder.Paid)
            {
                MessageBoxResult result = MessageBox.Show("Do you wish to finalize payment for this order?", "Payment", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.OK)
                {

                    Dictionary<String, String> items = new Dictionary<String, String>();
                    items.Add("paid_status", "Y");
                    DatabaseManager.Update2("customer_order", items, String.Format(" customer_order_id={0} ", SelectedOrder.Id.ToString()));


                    CompleteOrder();
                }
            }


            Populate();

            
        }

        private void CompleteOrder()
        {
            Dictionary<String, String> items = new Dictionary<String, String>();
            items.Add("status", "Y");
            DatabaseManager.Update2("customer_order", items, String.Format(" customer_order_id={0} ", SelectedOrder.Id.ToString()));


            // TODO: update db
         
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