using COES.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace COES.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class OrderViewModel : ViewModelBase
    {
        //----------------------------------------------------------------------
        #region --- Fields ---
        //----------------------------------------------------------------------
        private Order _order;
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Properties ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the current <see cref="Order"/>.
        /// </summary>
        public Order Order
        {
            get { return _order; }
            set { Set(() => Order, ref _order, value); }
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Constructor ---
        //----------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the AddOrderViewModel class.
        /// </summary>
        public OrderViewModel()
        {
            RegisterMessages();
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------


        //----------------------------------------------------------------------
        #region --- Methods ---
        //----------------------------------------------------------------------
        private void RegisterMessages()
        {
            Messenger.Default.Register<GenericMessage<Order>>(this, m => this.Order = m.Content);
        }
        //----------------------------------------------------------------------
        #endregion
        //----------------------------------------------------------------------

    }
}