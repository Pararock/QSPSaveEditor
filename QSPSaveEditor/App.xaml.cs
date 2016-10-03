namespace QSPSaveEditor
{
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

                //CA3075: Insecure DTD Processing
                XmlReaderSettings settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Prohibit,
                    ValidationType = ValidationType.DTD
                };

                using ( XmlReader reader = XmlReader.Create(contentStream.Stream, settings))
                {
                    customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }

                // and register it in the HighlightingManager
                HighlightingManager.Instance.RegisterHighlighting("QSP", new string[] { ".qsp" }, customHighlighting);
            }

            DispatcherHelper.Initialize();
        }
    }
}
