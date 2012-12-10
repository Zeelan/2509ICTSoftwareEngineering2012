using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace COES.Models
{
    public class Menu : ObservableObject
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private int _id;
        private ObservableCollection<MenuItem> _menuItems;
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
        /// Gets or sets the list of <see cref="MenuItems"/>.
        /// </summary>
        public ObservableCollection<MenuItem> MenuItems
        {
            get { return _menuItems ?? (_menuItems = new ObservableCollection<MenuItem>()); }
            set { Set(() => MenuItems, ref _menuItems, value); }
        }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { Set(() => DateCreated, ref _dateCreated, value); }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Constructor ---
        //----------------------------------------------------------------------
        public Menu()
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
