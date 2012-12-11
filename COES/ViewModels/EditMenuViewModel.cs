using System.Collections.ObjectModel;
using COES.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

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

        private void DeleteItem()
        {
            // TODO: remove item from db.
        }

        private void CreateItem()
        {
            if (!ItemSaved)
            {
                Save();
                CurrentMenuItem = new MenuItem();
            }
        }

        private void Save()
        {
            ItemSaved = true;
            // TODO: update db, return the menuitem id
        }

        private void AddItem()
        {
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