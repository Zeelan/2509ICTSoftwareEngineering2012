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
    public class ReportingViewModel : ViewModelBase
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private string[] _time;
        private string _selectedTime;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        public string[] Time
        {
            get { return _time ?? (_time = new string[] { "Day", "Week", "Month", "Year", "All Time" }); }
            set { Set(() => Time, ref _time, value); }
        }

        public string SelectedTime
        {
            get { return _selectedTime; }
            set { Set(() => SelectedTime, ref _selectedTime, value); }
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
        public ReportingViewModel()
        {
            InitializeCommands();
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

        private void Cancel()
        {
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Cancel"), "Navigate");
        }

        private void RunReport()
        {
            // TODO: db logic to run report.
            switch (SelectedTime)
            {
                case "Day":
                    {
                        //
                        break;
                    }
                case "Week":
                    {
                        //
                        break;
                    }
                case "Month":
                    {
                        //
                        break;
                    }
                case "Year":
                    {
                        //
                        break;
                    }
                case "All Time":
                    {
                        break;
                    }
            }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

    }
}