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
        private ObservableCollection<MenuItem> _menuItems;
        private MenuItem _currentMenuMenuItem;
        private MenuItem _currentMenuItem;
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

        public ObservableCollection<MenuItem> MenuItems
        {
            get { return _menuItems; }
            set { Set(() => MenuItems, ref _menuItems, value); }
        }

        public MenuItem CurrentMenuMenuItem
        {
            get { return _currentMenuMenuItem; }
            set { Set(() => CurrentMenuMenuItem, ref _currentMenuMenuItem, value); }
        }

        public MenuItem CurrentMenuItem
        {
            get { return _currentMenuItem; }
            set { Set(() => CurrentMenuItem, ref _currentMenuItem, value); }
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
            Messenger.Default.Register<ObservableCollection<MenuItem>>(this, "EditMenu", m => MenuItems = m);
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
            if (CurrentMenuItem != null)
            {
                String sql = String.Format("DELETE FROM menu_item WHERE menu_item_id = {0} ; ",CurrentMenuItem.Id.ToString());
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

            if (CurrentMenuItem != null)
            {
                
                if ((CurrentMenuItem.Name.Length > 0) && (CurrentMenuItem.Cost >= 0))
                {
                    Save();
                }
            }
            else
            {
                CurrentMenuItem = new MenuItem();
               
            }

        }


        /// <summary>
        /// SAVE new MENU item to database
        /// </summary>
        private void Save()
        {
            //ItemSaved = true;
            // TODO: update db, return the menuitem id

            Dictionary<String, String> item = new Dictionary<string, string>();
            item.Add("menu_item_name",CurrentMenuItem.Name.ToString());
            item.Add("item_cost",CurrentMenuItem.Cost.ToString());

            MessageBox.Show(CurrentMenuItem.Name.ToString() + " " + CurrentMenuItem.Cost.ToString());


            RefreshMenuItems();
        }


        /// <summary>
        /// Save MENU to database
        /// 
        /// </summary>
        private void SaveMenuToDatabase()
        {
            Dictionary<String, String> items = new Dictionary<string, string>();
            foreach(MenuItem mi in Menu.MenuItems)
            {
                items.Add("menu_item_id",mi.Id.ToString());
            }

            // Clear the menu in the database
            String sql = "DELETE FROM menu WHERE 1=1 ; ";
            DatabaseManager.QuickQuery(sql);

            //now save the new list
            DatabaseManager.Insert("menu", items);

        }


        private void RefreshMenuItems()
        {
            MenuItems.Clear();
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
                MenuItems.Add(mi);
            }
        }

        private void AddItem()
        {
            foreach (MenuItem menuItem in Menu.MenuItems)
            {

            }
            if (!Menu.MenuItems.Contains(CurrentMenuItem))
                Menu.MenuItems.Add(CurrentMenuItem);
            else
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ErrorMenuItemAlreadyExists", "Error"));
        }
        private void RemoveItem()
        {
            if (CurrentMenuMenuItem != null)
                Menu.MenuItems.Remove(CurrentMenuMenuItem);
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------
    }
}