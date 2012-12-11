using System;
using System.Data;
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


        /// <summary>
        /// Used to load a customer from the database by ID
        /// </summary>
        /// <param name="id">int representing customer id</param>
        /// <returns>false if load fails, true if load successful</returns>
        public bool loadID(int id)
        {
              // connect to the database

                // CHeck for a customer
                String sqlcheck = String.Format(" SELECT * from customer WHERE  customer_id = {0} ; ",id.ToString());
                int customerMatches = DatabaseManager.QuickQuery(sqlcheck);

                if (customerMatches > 0)
                {
                    String sql =  String.Format(" SELECT * from customer WHERE  customer_id = {0} ; ",id.ToString());

                    DataTable dt = DatabaseManager.Query(sql);

                    //assume only 1 valid result
                    DataRow dr = dt.Rows[0];
                  
                    // populate data.
                    this._firstName = dr["first_name"].ToString();
                    this._lastName = dr["last_name"].ToString();
                    this._phoneNumber = dr["phone_number"].ToString();
                    this._address = new Address
                    {
                        Number = dr["street_no"].ToString(),
                        PostCode = dr["suburb_post_code"].ToString(),
                        Street = dr["street"].ToString(),
                        Suburb = dr["suburb"].ToString()
                    };

                    this._id = id;
                    this._creditCard = new CreditCard
                    {
                        Number = dr["credit_card_number"].ToString(),
                        Name = dr["credit_card_name"].ToString(),
                        Expiry = dr["credit_card_expiry"].ToString()
                    };

                    this._status = dr["status"].ToString();
                    
                    return true;

                }else{
                    return false;
                }
        }

        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------
    }
}
