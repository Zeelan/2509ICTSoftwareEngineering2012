using System;
using System.Collections.ObjectModel;
using COES.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Data;

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
        private MenuItem _currentMenuItem;
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

        public MenuItem CurrentMenuItem
        {
            get { return _currentMenuItem; }
            set { Set(() => CurrentMenuItem, ref _currentMenuItem, value); }
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

        public RelayCommand RunReportCommand
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
            RunReportCommand = new RelayCommand(RunReport);



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

        private void RunReport()
        {

        }


        /// <summary>
        /// Used to save the current menu
        /// </summary>
        public void saveMenu()
        {
           

        }


        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------
    }
}