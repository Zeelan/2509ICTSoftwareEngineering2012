using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COES.Models
{
    /// <summary>
    /// Contains information for a Customer's credit card.
    /// </summary>
    public class CreditCard
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private string _number;
        private string _name;
        private string _expiry;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the expiry date.
        /// </summary>
        public string Expiry
        {
            get { return _expiry; }
            set { _expiry = value; }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Constructor ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCard"/> class.
        /// </summary>
        public CreditCard()
        {
        }

        public CreditCard(string number, string name, string expiry)
        {
            this.Number = number;
            this.Name = name;
            this.Expiry = expiry;
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------
    }
}
