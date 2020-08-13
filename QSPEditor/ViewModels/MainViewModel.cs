﻿using QSPEditor.Helpers;
using QSPEditor.Services;
using QSPLib_CppWinrt;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QSPEditor.ViewModels
{
    public class MainViewModel : Observable
    {
        private const string DefaultUrl = "ms-appx-web:///Error.html";
        private readonly IDialogService _dialogService;
        private readonly Engine _engine;
        private readonly IQspStreamUriResolver _qspStreamUriResolver;

        private Uri _source;

        public Uri Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }

            set
            {
                if (value)
                {
                    IsShowingFailedMessage = false;
                }

                Set(ref _isLoading, value);
                IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        internal async void ExecuteCommand(string command)
        {
            var location = _engine.Locations[1];
            var name = location.Name;
            var result = _engine.ExecuteCommandString(command, false);
            if (result.Code == StatusCode.QSP_SUCCESS)
            {
            }
            else
            {
                await _dialogService.ShowAsync(result);
            }
        }

        private Visibility _isLoadingVisibility;

        public Visibility IsLoadingVisibility
        {
            get { return _isLoadingVisibility; }
            set { Set(ref _isLoadingVisibility, value); }
        }

        private bool _isShowingFailedMessage;

        public bool IsShowingFailedMessage
        {
            get
            {
                return _isShowingFailedMessage;
            }

            set
            {
                if (value)
                {
                    IsLoading = false;
                }

                Set(ref _isShowingFailedMessage, value);
                FailedMesageVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _failedMesageVisibility;

        public Visibility FailedMesageVisibility
        {
            get { return _failedMesageVisibility; }
            set { Set(ref _failedMesageVisibility, value); }
        }

        private ICommand _navCompleted;

        public ICommand NavCompletedCommand
        {
            get
            {
                if (_navCompleted == null)
                {
                    _navCompleted = new RelayCommand<WebViewNavigationCompletedEventArgs>(NavCompleted);
                }

                return _navCompleted;
            }
        }

        private void NavCompleted(WebViewNavigationCompletedEventArgs e)
        {
            IsLoading = false;
            OnPropertyChanged(nameof(BrowserBackCommand));
            OnPropertyChanged(nameof(BrowserForwardCommand));
        }

        private ICommand _navFailed;

        public ICommand NavFailedCommand
        {
            get
            {
                if (_navFailed == null)
                {
                    _navFailed = new RelayCommand<WebViewNavigationFailedEventArgs>(NavFailed);
                }

                return _navFailed;
            }
        }

        private void NavFailed(WebViewNavigationFailedEventArgs e)
        {
            // Use `e.WebErrorStatus` to vary the displayed message based on the error reason
            IsShowingFailedMessage = true;
        }

        private ICommand _retryCommand;

        public ICommand RetryCommand
        {
            get
            {
                if (_retryCommand == null)
                {
                    _retryCommand = new RelayCommand(Retry);
                }

                return _retryCommand;
            }
        }

        private void Retry()
        {
            IsShowingFailedMessage = false;
            IsLoading = true;

            _webView?.Refresh();
        }

        private ICommand _browserBackCommand;

        public ICommand BrowserBackCommand
        {
            get
            {
                if (_browserBackCommand == null)
                {
                    _browserBackCommand = new RelayCommand(() => _webView?.GoBack(), () => _webView?.CanGoBack ?? false);
                }

                return _browserBackCommand;
            }
        }

        private ICommand _browserForwardCommand;

        public ICommand BrowserForwardCommand
        {
            get
            {
                if (_browserForwardCommand == null)
                {
                    _browserForwardCommand = new RelayCommand(() => _webView?.GoForward(), () => _webView?.CanGoForward ?? false);
                }

                return _browserForwardCommand;
            }
        }

        private ICommand _refreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(() => _webView.NavigateToLocalStreamUri(BuildUri("MainView"), _qspStreamUriResolver));
                }

                return _refreshCommand;
            }
        }

        private ICommand _openInBrowserCommand;

        public ICommand OpenInBrowserCommand
        {
            get
            {
                if (_openInBrowserCommand == null)
                {
                    _openInBrowserCommand = new RelayCommand(async () =>
                    {
                        await Windows.System.Launcher.LaunchUriAsync(Source);
                    });
                }

                return _openInBrowserCommand;
            }
        }

        private Uri BuildUri(string path)
        {
            return _webView.BuildLocalStreamUri(nameof(MainViewModel), path);
        }

        private WebView _webView;

        public MainViewModel(IEngineService engineService, IQspStreamUriResolver requestService, IDialogService dialog)
        {
            _dialogService = dialog;
            _engine = engineService.Engine;
            _qspStreamUriResolver = requestService;
            IsLoading = true;
            Source = new Uri(DefaultUrl);
        }

        public void Initialize(WebView webView)
        {
            _webView = webView;
            _webView.NavigateToLocalStreamUri(BuildUri("MainView"), _qspStreamUriResolver);
        }
    }
}
