namespace QSPSaveEditor.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    using MahApps.Metro.Controls.Dialogs;
    using Microsoft.Practices.ServiceLocation;
    using Model;

    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if ( ViewModelBase.IsInDesignModeStatic )
            {
                SimpleIoc.Default.Register<IQSPGameDataService, Design.DesignQSPGameDataService>();
                SimpleIoc.Default.Register<IDialogCoordinator, DialogCoordinator>();
            }
            else
            {
                SimpleIoc.Default.Register<IQSPGameDataService, QSPGameDataService>();
                SimpleIoc.Default.Register<IDialogCoordinator, DialogCoordinator>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}