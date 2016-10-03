using QSPEditor.Configuration;
using QSPEditor.ViewModels;
using System.Web;
using Windows.UI.Xaml.Controls;

namespace QSPEditor.Views
{
    public sealed partial class MainPage : Page
    {
        private readonly MainViewModel _mainViewModel;

        private MainViewModel ViewModel
        {
            get { return _mainViewModel; }
        }

        public MainPage()
        {
            _mainViewModel = ServiceLocator.Current.GetService<MainViewModel>();
            InitializeComponent();
            ViewModel.Initialize(webView);
        }

        private void OnUnsupportedUriSchemeIdentified(WebView sender, WebViewUnsupportedUriSchemeIdentifiedEventArgs e)
        {
            // Send exec scheme to QSP engine to be use as command
            if (e.Uri.Scheme == "exec")
            {
                var command = HttpUtility.UrlDecode(e.Uri.PathAndQuery);
                _mainViewModel.ExecuteCommand(command);
                e.Handled = true;
            }
        }

        private void webView_WebResourceRequested(WebView sender, WebViewWebResourceRequestedEventArgs args)
        {
            //// This is to intercept call to the web..
            //var path = args.Request.RequestUri.LocalPath;
            //var maybeObjPath = Path.GetRelativePath("/", path);
            //if (int.TryParse(maybeObjPath, out var objIndex))
            //{
            //    //_engine.SelectObject(objIndex, false);
            //}
        }
    }
}
