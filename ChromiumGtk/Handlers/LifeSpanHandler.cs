using Lunixo.ChromiumGtk.Core;
using Xilium.CefGlue;

namespace Lunixo.ChromiumGtk.Handlers
{
    internal sealed class LifeSpanHandler : CefLifeSpanHandler
    {
        private readonly WebBrowser _core;

        public LifeSpanHandler(WebBrowser core)
        {
            _core = core;
        }

        protected override bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName,
            CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures,
            CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref CefDictionaryValue extraInfo,
            ref bool noJavascriptAccess)
        {
            return true;
        }

        protected override void OnAfterCreated(CefBrowser browser)
        {
            base.OnAfterCreated(browser);
            _core.OnCreated(browser);
        }

        protected override bool DoClose(CefBrowser browser)
        {
            return false;
        }

        protected override void OnBeforeClose(CefBrowser browser)
        {
        }
    }
}
