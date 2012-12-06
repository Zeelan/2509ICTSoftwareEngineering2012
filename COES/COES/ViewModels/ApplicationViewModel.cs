using GalaSoft.MvvmLight;

namespace COES.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ApplicationViewModel : ViewModelBase
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private ViewModelBase _currentViewModel;

        readonly static HomeViewModel _homeViewModel = new HomeViewModel();
        readonly static CreateOrderViewModel _createOrderViewModel = new CreateOrderViewModel();
        readonly static EditOrderViewModel _editOrderViewModel = new EditOrderViewModel();
        
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the current ViewModel associated with the current View.
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { Set(() => CurrentViewModel, ref _currentViewModel, value); }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Constructor ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the ApplicationViewModel class.
        /// </summary>
        public ApplicationViewModel()
        {
            CurrentViewModel = _createOrderViewModel;
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