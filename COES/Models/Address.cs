using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COES.Models
{
    public class Address
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private string _number;
        private string _street;
        private string _suburb;
        private string _postCode;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the street number.
        /// </summary>
        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        /// <summary>
        /// Gets or sets the street name.
        /// </summary>
        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        /// <summary>
        /// Gets or sets the suburb name.
        /// </summary>
        public string Suburb
        {
            get { return _suburb; }
            set { _suburb = value; }
        }

        /// <summary>
        /// Gets or sets the post code.
        /// </summary>
        public string PostCode
        {
            get { return _postCode; }
            set { _postCode = value; }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Constructor ---
        //----------------------------------------------------------------------
        public Address()
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
