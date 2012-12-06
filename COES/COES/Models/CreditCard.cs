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
        private int _number;
        private string _name;
        private int _expiry;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        public int Number
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
        public int Expiry
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

        public CreditCard(int number, string name, int expiry)
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
