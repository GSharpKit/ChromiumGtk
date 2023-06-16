using System;
using Xilium.CefGlue;

namespace Lunixo.ChromiumGtk.Core
{
    public sealed class WebBrowser : IDisposable
    {
        private readonly CefBrowserSettings _settings;

        public WebBrowser(CefBrowserSettings settings)
        {
            _settings = settings;
            Client = new WebClient(this);
        }
        
        public CefBrowser CefBrowser { get; private set; }
        public WebClient Client { get; }

        public void Create(CefWindowInfo windowInfo, string startUrl)
        {
            CefBrowserHost.CreateBrowserSync(windowInfo, Client, _settings, startUrl);
        }

        public event EventHandler Created;

        internal void OnCreated(CefBrowser browser)
        {
            CefBrowser = browser;
            Created?.Invoke(this, EventArgs.Empty);
        }
        
        public void Dispose()
        {
            if (CefBrowser != null)
            {
                var host = CefBrowser.GetHost();
                host.CloseBrowser(true);
                host.Dispose();
                CefBrowser.Dispose();
                CefBrowser = null;
            }
        }
    }
}
