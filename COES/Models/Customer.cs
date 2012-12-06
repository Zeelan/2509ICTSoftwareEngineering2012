using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace COES.Models
{
    /// <summary>
    /// Contains information of a customer.
    /// </summary>
    public class Customer : ObservableObject
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private int _id;
        private string _firstName;
        private string _lastName;
        private Address _address;
        private CreditCard _creditCard;
        private string _phoneNumber;
        private string _status;
        private string _comments;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { Set(() => Id, ref _id, value); }
        }

        /// <summary>
        /// Gets or sets the given name.
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set { Set(() => FirstName, ref _firstName, value); }
        }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set { Set(() => LastName, ref _lastName, value); }
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        public string Name
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public Address Address
        {
            get { return _address; }
            set { Set(() => Address, ref _address, value); }
        }

        /// <summary>
        /// Gets or sets the the credit card details.
        /// </summary>
        public CreditCard CreditCard
        {
            get { return _creditCard; }
            set { Set(() => CreditCard, ref _creditCard, value); }
        }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { Set(() => PhoneNumber, ref _phoneNumber, value); }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { Set(() => Status, ref _status, value); }
        }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        public string Comments
        {
            get { return _comments;}
            set { Set(() => Comments, ref _comments, value); }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Constructor ---
        //----------------------------------------------------------------------
        public Customer()
        {
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Methods ---
        //----------------------------------------------------------------------
        public override string ToString()
        {
            return Name;
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------
    }
}
