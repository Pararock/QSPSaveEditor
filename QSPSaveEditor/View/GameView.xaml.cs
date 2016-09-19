namespace QSPSaveEditor.View
{
    using CefSharp;
    using Microsoft.Practices.ServiceLocation;
    using Model;
    using System.Windows;
    using System.Windows.Controls;
    using ViewModel;

    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public GameView()
        {
            InitializeComponent();

            var dataService = ServiceLocator.Current.GetInstance<IQSPGameDataService>();
            webBrowser.RequestHandler = new RequestHandler(dataService);
        }
    }
}
