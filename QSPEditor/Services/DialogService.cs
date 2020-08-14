using QSPEditor.Views;
using QSPLib_CppWinrt;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QSPEditor.Services
{
    public class DialogService : IDialogService
    {
        private readonly Engine _engine;

        public DialogService(IEngineService engineService)
        {
            _engine = engineService.Engine;
        }
        public async Task ShowAsync(QSPLib_CppWinrt.Result result, string ok = "Ok")
        {
            var locationInfo = result.Location;
            var length = _engine.Locations.Count;
            if (locationInfo.Location >= length)
            {
                // so yeah, wtf.
                // changing from singleton to scoped for the dialogservice fixed this.
                // I hope
                await ShowAsync(result.Message, "Dialogservice can't access all location data", ok);
            }
            if (locationInfo.Location == -1)
            { 
                await ShowAsync(result.Message, "No Location Error message", ok);
            }
            else
            {
                var locationObject = _engine.Locations[locationInfo.Location];
                var errorMessage = $"Error # {result.Code} \nOn location ${locationObject.Name} line: {locationInfo.Line} action index: {locationInfo.ActionIndex}";
                if (locationInfo.Line >= 0)
                {
                    var panel = new StackPanel();
                    var textBlock = new RichTextBlock();
                    var indent = LocationsDetailControl.FormatLines(0, locationObject.OnVisitLines.Skip(locationInfo.Line - 2).Take(5).ToList(), textBlock.Blocks, locationInfo.Line);
                    var location = locationObject.OnVisitLines[locationInfo.Line];

                    var errorBlock = new TextBlock();
                    errorBlock.Text = errorMessage;

                    panel.Children.Add(errorBlock);
                    panel.Children.Add(textBlock);

                    await ShowCustomAsync(result.Message, panel, ok);
                }
                else
                {
                    await ShowAsync(result.Message, errorMessage, ok);
                }

            }

        }

        private async Task<bool> ShowCustomAsync(string title, FrameworkElement element, string ok = "Ok", string cancel = null)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = element,
                PrimaryButtonText = ok
            };
            if (cancel != null)
            {
                dialog.SecondaryButtonText = cancel;
            }
            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        public async Task<bool> ShowAsync(string title, string content, string ok = "Ok", string cancel = null)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = ok
            };
            if (cancel != null)
            {
                dialog.SecondaryButtonText = cancel;
            }
            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }
    }

    public interface IDialogService
    {
        Task ShowAsync(QSPLib_CppWinrt.Result result, string ok = "Ok");
        Task<bool> ShowAsync(string title, string content, string ok = "Ok", string cancel = null);
    }
}
