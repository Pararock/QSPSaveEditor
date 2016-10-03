using Microsoft.Toolkit.Uwp.Helpers;
using QSPLib_CppWinrt;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web;

namespace QSPEditor.Services
{
    public interface IQspStreamUriResolver : IUriToStreamResolver
    {
    }

    public sealed class QspStreamUriResolver : IQspStreamUriResolver
    {
        private readonly IDialogService _dialogService;
        private readonly Engine _engine;
        private string _baseFolder;

        private static StorageFile _notFoundFile;
        private static StorageFile _redirectFile;
        private IStorageFolder _folder;

        public QspStreamUriResolver(IEngineService engineService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _engine = engineService.Engine;
            _engine.PropertyChanged += _engine_PropertyChanged;
            if (_engine.isFileAccessSafe)
            {
                Task.Run(async () =>
                {
                    _folder = await _engine.CurrentGame.GetParentAsync();
                    _baseFolder = _folder.Path;
                });
            }
        }

        public static async Task StartupAsync()
        {
            Uri localUri = new Uri("ms-appx:///Assets/Image-Not-Found.svg");
            _notFoundFile = await StorageFile.GetFileFromApplicationUriAsync(localUri);

            localUri = new Uri("ms-appx:///Assets/301-Moved.txt");
            _redirectFile = await StorageFile.GetFileFromApplicationUriAsync(localUri);
        }

        private void _engine_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_engine.CurrentGame) && _engine.isFileAccessSafe)
            {
                Task.Run(async () =>
                {
                    _folder = await _engine.CurrentGame.GetParentAsync();
                    _baseFolder = _folder.Path;
                });
            }
        }


        public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri)
        {
            if (_engine.CurrentGame == null) return NoGameLoaded().AsAsyncOperation();

            if (uri.LocalPath == "/MainView")
            {
                return MainViewStringasync().AsAsyncOperation();
            }
            else
            {
                // Is this an object path?
                if (int.TryParse(Path.GetRelativePath("/", uri.LocalPath), out var objIndex))
                {
                    var result = _engine.SelectAction(objIndex + 50, true);

                    result = _engine.ExecuteSelectedAction(true);
                    // We are in a background thread here. We need to find a way to send the result to the ui thread
                    // still not sure how to update the view either??

                    return MainViewStringasync().AsAsyncOperation();
                }
            }

            return StreamFileContentAsync(uri.LocalPath).AsAsyncOperation();
        }

        private async Task<IInputStream> StreamFileContentAsync(string path)
        {
            // Check if we have broadFileSystemAccess Windows permissions.
            // https://docs.microsoft.com/en-us/windows/uwp/files/file-access-permissions
            if (!_engine.isFileAccessSafe) return await _notFoundFile.OpenAsync(FileAccessMode.Read);

            var cleanedRelativePath = path.Substring(1, path.Length - 1);
            var fileSystemPath = Path.GetFullPath(Path.Combine(_baseFolder, cleanedRelativePath));
            StorageFile file;
            try
            {
                file = await StorageFile.GetFileFromPathAsync(fileSystemPath);
            }
            catch (Exception e)
            {
                return await _notFoundFile.OpenAsync(FileAccessMode.Read);
            }

            return await file.OpenReadAsync();
        }

        private async Task<IInputStream> GetContent()
        {
            return await _engine.MainViewStream;
        }

        private async Task<IInputStream> MainViewStringasync()
        {
            using (var memoryStream = new InMemoryRandomAccessStream())
            {
                using (var dataWriter = new DataWriter(memoryStream))
                {
                    dataWriter.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                    dataWriter.ByteOrder = ByteOrder.LittleEndian;

                    var localUri = new Uri("ms-appx:///Assets/BackWardCompatibility.html");
                    StorageFile f = await StorageFile.GetFileFromApplicationUriAsync(localUri);
                    IRandomAccessStream stream = await f.OpenAsync(FileAccessMode.Read);
                    // TODO : encoding problem. There's a weird ??? at the top of the main description
                    // probably BOM corruption
                    dataWriter.WriteString(await stream.ReadTextAsync());
                    dataWriter.WriteString(_engine.MainView);
                    dataWriter.WriteString("</body>");
                    await dataWriter.StoreAsync();
                    await dataWriter.FlushAsync();
                    dataWriter.DetachStream();
                }
                return memoryStream.GetInputStreamAt(0);
            }
        }



        private async Task<IInputStream> NoGameLoaded()
        {
            Uri localUri = new Uri("ms-appx:///Hello.html");
            StorageFile f = await StorageFile.GetFileFromApplicationUriAsync(localUri);
            IRandomAccessStream stream = await f.OpenAsync(FileAccessMode.Read);
            return stream;
        }


    }
}
