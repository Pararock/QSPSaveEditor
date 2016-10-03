using QSPEditor.Helpers;
using QSPEditor.Services;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;

namespace QSPEditor.ViewModels
{
    public class HomeViewModel : Observable
    {
        private readonly IRecentFilesService _recentFilesService;

        public IList<IStorageItem> RecentGame => _recentFilesService.GameFiles.Values.ToList();
        public HomeViewModel(IRecentFilesService recentFileService)
        {
            _recentFilesService = recentFileService;
        }
    }
}
