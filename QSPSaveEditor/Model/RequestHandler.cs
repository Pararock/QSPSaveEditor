
namespace QSPSaveEditor.Model
{
    using CefSharp;
    using QSPNETWrapper;
    using System;

    // https://github.com/cefsharp/CefSharp/blob/397ffb515897a27e478d517e4d7b57e0cac17c78/CefSharp.Example/RequestHandler.cs

    public class RequestHandler : IRequestHandler
    {
        private IQSPGameDataService _dataService;
        private QSPGame _game;
        public RequestHandler(IQSPGameDataService dataService)
        {
            _dataService = dataService;
            _game = dataService.Game;
        }
        bool IRequestHandler.GetAuthCredentials( IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback )
        {
            throw new NotImplementedException();
        }

        IResponseFilter IRequestHandler.GetResourceResponseFilter( IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response )
        {
            return null;
        }

        bool IRequestHandler.OnBeforeBrowse( IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect )
        {
            //throw new NotImplementedException();
            var b = "";
            return false;
        }

        CefReturnValue IRequestHandler.OnBeforeResourceLoad( IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback )
        {
            return CefReturnValue.Continue;
        }

        bool IRequestHandler.OnCertificateError( IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback )
        {
            throw new NotImplementedException();
        }

        bool IRequestHandler.OnOpenUrlFromTab( IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture )
        {
            throw new NotImplementedException();
        }

        void IRequestHandler.OnPluginCrashed( IWebBrowser browserControl, IBrowser browser, string pluginPath )
        {
            throw new NotImplementedException();
        }

        bool IRequestHandler.OnProtocolExecution( IWebBrowser browserControl, IBrowser browser, string url )
        {
            //handle click url here
            if(url.StartsWith("exec:"))
            {
                var command = url.Substring(5);
                //var commandExcaped = command.Replace("'", "''");
                //var arrayCommand = command.Split('&');
                //foreach ( var singleCommand in arrayCommand )
                {
                    
                    _game.ExecCommand(System.Web.HttpUtility.HtmlDecode(command));
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        bool IRequestHandler.OnQuotaRequest( IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback )
        {
            throw new NotImplementedException();
        }

        void IRequestHandler.OnRenderProcessTerminated( IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status )
        {
            throw new NotImplementedException();
        }

        void IRequestHandler.OnRenderViewReady( IWebBrowser browserControl, IBrowser browser )
        {
            //throw new NotImplementedException();
        }

        void IRequestHandler.OnResourceLoadComplete( IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength )
        {
            //throw new NotImplementedException();
        }

        void IRequestHandler.OnResourceRedirect( IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, ref string newUrl )
        {
            throw new NotImplementedException();
        }

        bool IRequestHandler.OnResourceResponse( IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response )
        {
            return false;
        }
    }
}
