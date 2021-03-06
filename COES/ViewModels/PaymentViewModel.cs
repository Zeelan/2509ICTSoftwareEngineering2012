﻿using COES.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Registers the messages associated with this ViewModel.
        /// </summary>
        private void RegisterMessages()
        {
            Messenger.Default.Register<Order>(this, "PaymentReady", m => Order = m);
            Messenger.Default.Register<Customer>(this, "PaymentReady", m => Customer = m);
        }

        private void PayNow()
        {
            Order.Paid = true;
            Dictionary<String, String> items = new Dictionary<String, String>();
            items.Add("paid_status", "Y");
            DatabaseManager.Update2("customer_order",items,String.Format(" customer_order_id={0} ",Order.Id.ToString() ));


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