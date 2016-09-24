namespace QSPSaveEditor
{
    using CefSharp;
    using GalaSoft.MvvmLight.Threading;
    using ICSharpCode.AvalonEdit.Highlighting;
    using System;
    using System.Windows;
    using System.Xml;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            // Load the custom QSP style for AvalonEdit
            var uri = new Uri("Resources/QSP-Mode.xshd", UriKind.Relative);
            var contentStream = Application.GetContentStream(uri);

            if ( contentStream != null )
            {
                IHighlightingDefinition customHighlighting;

                using ( XmlReader reader = new XmlTextReader(contentStream.Stream) )
                {
                    customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }

                // and register it in the HighlightingManager
                HighlightingManager.Instance.RegisterHighlighting("QSP", new string[] { ".qsp" }, customHighlighting);
            }

            var settings = new CefSettings { RemoteDebuggingPort = 8088 };
            Cef.Initialize(settings);

            DispatcherHelper.Initialize();
        }
    }
}
