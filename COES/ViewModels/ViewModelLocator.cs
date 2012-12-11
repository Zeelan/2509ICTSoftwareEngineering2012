/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:COES.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using COES.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace COES.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        private static ApplicationViewModel _application;
        private static HomeViewModel _home;
        private static CustomerViewModel _customer;
        private static OrderViewModel _order;
        private static PaymentViewModel _payment;
        private static ReportingViewModel _reporting;
        private static EditMenuViewModel _editMenu;

        static ViewModelLocator()
        {
            //ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            //}
            //else
            //{
            //    SimpleIoc.Default.Register<IDataService, DataService>();
            //}
            //SimpleIoc.Default.Register<ApplicationViewModel>();
            //SimpleIoc.Default.Register<HomeViewModel>();
            //SimpleIoc.Default.Register<CustomerViewModel>();
            //SimpleIoc.Default.Register<OrderViewModel>();
            
        }

        public ApplicationViewModel Application
        {
            get { return ApplicationStatic; }
        }

        public static ApplicationViewModel ApplicationStatic
        {
            get { return _application ?? (_application = new ApplicationViewModel()); }
        }

        public HomeViewModel Home
        {
            get { return HomeStatic; }
        }

        public static HomeViewModel HomeStatic
        {
            get { return _home ?? (_home = new HomeViewModel()); }
        }

        public CustomerViewModel Customer
        {
            get { return CustomerStatic; }
        }

        public static CustomerViewModel CustomerStatic
        {
            get { return _customer ?? (_customer = new CustomerViewModel()); }
        }

        public OrderViewModel Order
        {
            get { return OrderStatic; }
        }

        public static OrderViewModel OrderStatic
        {
            get { return _order ?? (_order = new OrderViewModel()); }
        }

        public PaymentViewModel Payment
        {
            get { return PaymentStatic; }
        }

        public static PaymentViewModel PaymentStatic
        {
            get { return _payment ?? (_payment = new PaymentViewModel()); }
        }

        public ReportingViewModel Reporting
        {
            get { return ReportingStatic; }
        }

        public static ReportingViewModel ReportingStatic
        {
            get { return _reporting ?? (_reporting = new ReportingViewModel()); }
        }

        public EditMenuViewModel EditMenu
        {
            get { return EditMenuStatic; }
        }

        public static EditMenuViewModel EditMenuStatic
        {
            get { return _editMenu ?? (_editMenu = new EditMenuViewModel()); }
        }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        //"CA1822:MarkMembersAsStatic",
        //Justification = "This non-static member is needed for data binding purposes.")]
        //public ApplicationViewModel Application
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<ApplicationViewModel>();
        //    }
        //}

        ///// <summary>
        ///// Gets the Home property.
        ///// </summary>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        //    "CA1822:MarkMembersAsStatic",
        //    Justification = "This non-static member is needed for data binding purposes.")]
        //public HomeViewModel Home
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<HomeViewModel>();
        //    }
        //}

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        //"CA1822:MarkMembersAsStatic",
        //Justification = "This non-static member is needed for data binding purposes.")]
        //public CustomerViewModel Customer
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<CustomerViewModel>();
        //    }
        //}

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        //"CA1822:MarkMembersAsStatic",
        //Justification = "This non-static member is needed for data binding purposes.")]
        //public OrderViewModel CreateOrder
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<OrderViewModel>();
        //    }
        //}


        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}