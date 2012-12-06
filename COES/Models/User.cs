using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace COES.Models
{
    public class User : ObservableObject
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private int _id;
        private string _firstName;
        private string _lastName;
        private string _password;
        private string _status;
        private DateTime _dateCreated;
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
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set { Set(() => FirstName, ref _firstName, value); }
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// Gets or sets the account status.
        /// </summary>
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set
            {
                _dateCreated = value;
                RaisePropertyChanged("DateCreated");
            }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Constructor ---
        //----------------------------------------------------------------------
        public User()
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
