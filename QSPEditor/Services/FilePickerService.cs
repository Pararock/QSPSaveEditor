using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace QSPEditor.Services
{
    public class FilePickerResult
    {
        public StorageFile File { get; set; }
    }

    public interface IFilePickerService
    {
        Task<FilePickerResult> OpenQSPGameAsync();
        Task<FilePickerResult> OpenSaveAsync();
    }

    public class FilePickerService : IFilePickerService
    {
        public async Task<FilePickerResult> OpenQSPGameAsync()
        {
            return await OpenFile(".qsp");
        }

        public async Task<FilePickerResult> OpenSaveAsync()
        {
            return await OpenFile(".sav");
        }

        private static async Task<FilePickerResult> OpenFile(string extension)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add(extension);

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                return new FilePickerResult
                {
                    File = file,
                };
            }
            return null;
        }
    }
}
