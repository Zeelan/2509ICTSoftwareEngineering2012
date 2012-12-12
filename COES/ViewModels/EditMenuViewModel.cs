using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using COES.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Data;
using System.Windows;

namespace COES.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class EditMenuViewModel : ViewModelBase
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private Menu _menu;
        private ObservableCollection<MenuItem> _allMenuItems;
        private MenuItem _currentMenuItem;
        private MenuItem _currentAllMenuItem;
        private bool _itemSaved;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        public Menu Menu
        {
            get { return _menu; }
            set { Set(() => Menu, ref _menu, value); }
        }

        public ObservableCollection<MenuItem> AllMenuItems
        {
            get { return _allMenuItems; }
            set { Set(() => AllMenuItems, ref _allMenuItems, value); }
        }

        public MenuItem CurrentMenuItem
        {
            get { return _currentMenuItem; }
            set { Set(() => CurrentMenuItem, ref _currentMenuItem, value); }
        }

        public MenuItem CurrentAllMenuItem
        {
            get { return _currentAllMenuItem; }
            set { Set(() => CurrentAllMenuItem, ref _currentAllMenuItem, value); }
        }

        /// <summary>
        /// Gets or sets whether the current <see cref="MenuItem"/> has been saved.
        /// </summary>
        public bool ItemSaved
        {
            get { return _itemSaved; }
            set { Set(() => ItemSaved, ref _itemSaved, value); }
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

        public RelayCommand DeleteItemCommand
        {
            get;
            private set;
        }

        public RelayCommand CreateItemCommand
        {
            get;
            private set;
        }

        public RelayCommand SaveCommand
        {
            get;
            private set;
        }
        public RelayCommand AddItemCommand
        {
            get;
            private set;
        }
        public RelayCommand RemoveItemCommand
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
        /// Initializes a new instance of the ReportingViewModel class.
        /// </summary>
        public EditMenuViewModel()
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
        /// Initializes the commands associated with this ViewModel.
        /// </summary>
        private void InitializeCommands()
        {
            CancelCommand = new RelayCommand(Cancel);
            DeleteItemCommand = new RelayCommand(DeleteItem);
            CreateItemCommand = new RelayCommand(CreateItem);
            SaveCommand = new RelayCommand(Save);
            AddItemCommand = new RelayCommand(AddItem);
            RemoveItemCommand = new RelayCommand(RemoveItem);
        }

        /// <summary>
        /// Registers the messages associated with this ViewModel.
        /// </summary>
        private void RegisterMessages()
        {
            Messenger.Default.Register<Menu>(this, "EditMenu", m => Menu = m);
            Messenger.Default.Register<ObservableCollection<MenuItem>>(this, "EditMenu", m => AllMenuItems = m);
        }

        private void Cancel()
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Cancel"), "Navigate");
        }

        /// <summary>
        /// Deletes the current menu item
        /// </summary>
        private void DeleteItem()
        {
            if (CurrentAllMenuItem != null)
            {
                String sql = String.Format("DELETE FROM menu_item WHERE menu_item_id = {0} ; ",CurrentAllMenuItem.Id.ToString());
                DatabaseManager.QuickQuery(sql);
                RefreshMenuItems();
            }
            else
            {
                MessageBox.Show("Please select an item");
            }
        }



        private void CreateItem()
        {
            //if (!ItemSaved)
            //{

            if (CurrentAllMenuItem != null)
            {
                
                if ((CurrentAllMenuItem.Name.Length > 0) && (CurrentAllMenuItem.Cost >= 0))
                {
                    Save();
                }
            }
            else
            {
                CurrentAllMenuItem = new MenuItem();
               
            }

        }


        /// <summary>
        /// SAVE new MENU item to database
        /// </summary>
        private void Save()
        {
            //ItemSaved = true;
            // TODO: update db, return the menuitem id

            //Dictionary<String, String> item = new Dictionary<string, string>();
            //item.Add("menu_item_name",CurrentMenuItem.Name.ToString());
            //item.Add("item_cost",CurrentMenuItem.Cost.ToString());

            //MessageBox.Show(CurrentMenuItem.Name.ToString() + " " + CurrentMenuItem.Cost.ToString());

            SaveMenuToDatabase();
            RefreshMenuItems();
        }


        /// <summary>
        /// Save MENU to database
        /// </summary>
        private void SaveMenuToDatabase()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();
            foreach(MenuItem mi in Menu.MenuItems)
            {
                items.Add("menu_item_id", mi.Id.ToString());
            }

            // Clear the menu in the database
            String sql = "DELETE FROM menu WHERE 1=1 ; ";
            DatabaseManager.QuickQuery(sql);

            //now save the new list
            DatabaseManager.Insert("menu", items);

            MessageBox.Show("Menu updated.", "Menu", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }


        private void RefreshMenuItems()
        {
            AllMenuItems.Clear();
            String sql = "select * from menu_item ;";
            DataTable dt = DatabaseManager.Query(sql);

            // ok i'll use a foreach :-(
            foreach (DataRow dr in dt.Rows)
            {
                MenuItem mi = new MenuItem
                {
                    Cost = double.Parse(dr["item_cost"].ToString()),
                    Id = int.Parse(dr["menu_item_id"].ToString()),
                    Description = dr["description"].ToString(),
                    Name = dr["menu_item_name"].ToString()
                };
                AllMenuItems.Add(mi);
            }
        }

        private void AddItem()
        {
            // poor way of doing this but because of how we set it up i cant compare the object itself -_______-
            bool found = false;
            foreach (MenuItem mi in Menu.MenuItems)
            {
                if (CurrentAllMenuItem != null && mi.Name == CurrentAllMenuItem.Name)
                {
                    found = true;
                    MessageBox.Show("Item already exists in menu", "Menu", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                }
            }
            if (!found)
                Menu.MenuItems.Add(CurrentAllMenuItem);        
            else
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ErrorMenuItemAlreadyExists", "Error"));
        }
        private void RemoveItem()
        {
            if (CurrentMenuItem != null)
                Menu.MenuItems.Remove(CurrentMenuItem);
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------
    }
}