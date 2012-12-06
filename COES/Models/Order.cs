using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace COES.Models
{
    public class Order : ObservableObject
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private int _id;
        private int _customerId;
        private DateTime _dateCreated;
        private List<MenuItem> _menuItems;
        private double _cost;
        private bool _delivery;
        private string _status;
        private bool _paid;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the unique id of the <see cref="Order"/>.
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { Set(() => Id, ref _id, value); }
        }

        /// <summary>
        /// Gets or sets the id of the <see cref="Customer"/>.
        /// </summary>
        public int CustomerId
        {
            get { return _customerId; }
            set { Set(() => CustomerId, ref _customerId, value); }
        }

        /// <summary>
        /// Gets or sets the date the <see cref="Order"/> is created.
        /// </summary>
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { Set(() => DateCreated, ref _dateCreated, value); }
        }

        /// <summary>
        /// Gets or sets the items in the menu.
        /// </summary>
        public List<MenuItem> MenuItems
        {
            get { return _menuItems; }
            set { Set(() => MenuItems, ref _menuItems, value); }
        }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        public double Cost
        {
            get { return _cost; }
            set { Set(() => Cost, ref _cost, value); }
        }

        /// <summary>
        /// Gets or sets whether the <see cref="Order"/> is delivery or not.
        /// </summary>
        public bool Delivery
        {
            get { return _delivery;}
            set { Set(() => Delivery, ref _delivery, value);}
        }

        /// <summary>
        /// Gets or sets whether the <see cref="Order"/> is complete.
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { Set(() => Status, ref _status, value); }
        }

        /// <summary>
        /// Gets or sets whether the <see cref="Order"/> has been paid.
        /// </summary>
        public bool Paid
        {
            get { return _paid; }
            set { Set(() => Paid, ref _paid, value); }
        }


        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Constructor ---
        //----------------------------------------------------------------------
        public Order()
        {

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
