using System;
using Gtk;
using Lunixo.ChromiumGtk.Core;
using Xilium.CefGlue;
using InteropLinux = Lunixo.ChromiumGtk.Interop.InteropLinux;

namespace Lunixo.ChromiumGtk
{
    class FocusHandler : CefFocusHandler
    {
        protected override bool OnSetFocus(CefBrowser browser, CefFocusSource source)
        {
            return true;
        }

        protected override void OnGotFocus(CefBrowser browser)
        {
            base.OnGotFocus(browser);
        }

        protected override void OnTakeFocus(CefBrowser browser, bool next)
        {
            base.OnTakeFocus(browser, next);
        }
    }

    class PopupHandler : CefContextMenuHandler
    {
        protected override void OnBeforeContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams state, CefMenuModel model)
        {
            base.OnBeforeContextMenu(browser, frame, state, model);
        }

        protected override bool RunContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams parameters, CefMenuModel model, CefRunContextMenuCallback callback)
        {
            return true;
        }
    }

    class RequestHandler : CefRequestHandler
    {
        protected override CefResourceRequestHandler GetResourceRequestHandler (CefBrowser browser, CefFrame frame, CefRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return null;
        }

        protected override bool OnOpenUrlFromTab (CefBrowser browser, CefFrame frame, string targetUrl, CefWindowOpenDisposition targetDisposition, bool userGesture)
        {
            return userGesture;
        }

        protected override bool OnBeforeBrowse (CefBrowser browser, CefFrame frame, CefRequest request, bool userGesture, bool isRedirect)
        {
            if ((request.TransitionType & CefTransitionType.ForwardBackFlag) != 0)
            {
                return true;
            }

            return userGesture;
        }
    }

    public class WebView : Bin
    {
        private bool _initialized;
        private bool _created;
        private string _startUrl;

        private static bool running;
        private static Runtime runtime;

#if LINUX
        const string cefPath = "/usr/lib/cef";
#endif
#if WINDOWS
        const string cefPath = @"C:\Program Files\GSharpKit\bin\cef";
#endif

        public static CefBrowserSettings CreateDefaultBrowserSettings()
        {
            return new CefBrowserSettings
            {
                DefaultEncoding = "UTF-8",
                WebGL = CefState.Disabled
            };
        }

        public WebView(CefBrowserSettings browserSettings = null)
        {
            Realized += OnRealized;

            Browser = new WebBrowser(browserSettings ?? CreateDefaultBrowserSettings());
            Browser.Created += BrowserOnCreated;

            SizeAllocated += OnSizeAllocated;
            ConfigureEvent += OnConfigureEvent;
            FocusInEvent += OnFocusIn;
        }

        /// <summary>
        /// Initialize CEF Runtime
        /// </summary>
        public static void Initialize ()
        {
            runtime = new Runtime(new CefSettings
            {
                MultiThreadedMessageLoop = false,
                LogSeverity = CefLogSeverity.Debug,
                LogFile = $"logs_{DateTime.Now:yyyyMMdd}.log",
                CachePath = null,
                RemoteDebuggingPort = 32423,
                LocalesDirPath = System.IO.Path.Combine(cefPath, "locales"),
                //BrowserSubprocessPath = System.IO.Path.Combine(cefPath, subprocessName), // NOT USED with --no-zygote
                ResourcesDirPath = System.IO.Path.Combine(cefPath), // Must have chmod 755
                NoSandbox = true,
                BackgroundColor = new CefColor(0, 0, 0, 0),
                WindowlessRenderingEnabled = true,
                ExternalMessagePump = true,
                Locale = "en-US",
                CommandLineArgsDisabled = false,
            }, new[]
            {
                "--disable-gpu",
                "--no-zygote",
                "--single-process"
            });
            runtime.Initialize();
            runtime.DoMessageLoopWork();
        }

        public static void Run ()
        {
            running = true;
            GLib.Timeout.Add(10, OnIdlePump);
        }

        static bool OnIdlePump()
        {
            runtime.DoMessageLoopWork();
            return running;
        }

        public static void Quit ()
        {
            running = false;
            runtime.Shutdown();
        }

        public WebBrowser Browser { get; }

        private void BrowserOnCreated(object sender, EventArgs e)
        {
            Browser.Created -= BrowserOnCreated;
            _created = true;

            Browser.Client.FocusHandler = new FocusHandler ();
            Browser.Client.ContextMenuHandler = new PopupHandler();
            Browser.Client.RequestHandler = new RequestHandler();

            if (_startUrl != null)
            {
                LoadUrl(_startUrl);
            }
        }

        private void OnFocusIn(object o, FocusInEventArgs args)
        {
            Browser.CefBrowser.GetHost().SetFocus(true);

            args.RetVal = false;
        }

        private void OnConfigureEvent(object o, ConfigureEventArgs args)
        {
            Browser.CefBrowser.GetHost().NotifyMoveOrResizeStarted();

            args.RetVal = false;
        }

        private void OnSizeAllocated(object o, SizeAllocatedArgs args)
        {
            if (Handle != IntPtr.Zero && _initialized)
            {
                ResizeBrowser(args.Allocation.Width, args.Allocation.Height);
            }

            args.RetVal = true;
        }


        protected virtual void ResizeBrowser (int width, int height)
        {
            try
            {
                if (!_created) return;

                var browserWindow = Browser.CefBrowser.GetHost ().GetWindowHandle ();

#if LINUX
                var gdkDisplay = InteropLinux.gtk_widget_get_display (Handle);
                var x11Display = InteropLinux.gdk_x11_display_get_xdisplay (gdkDisplay);
                InteropLinux.XMoveResizeWindow (x11Display, browserWindow, 0, 0, width, height);
#endif

#if WINDOWS
				InteropWindows.SetWindowPos(browserWindow, 0, 0, 0, width, height, 0x0002 | 0x0004 | 0x0010); //SWP_NOMOVE|SWP_NOZORDER|SWP_NOACTIVATE);
#endif

                Browser.CefBrowser.GetHost ().WasResized();
                runtime.DoMessageLoopWork ();
            }
            catch (Exception e)
            {
                Console.WriteLine (e.Message);
                Console.WriteLine (e.StackTrace);
            }
        }

        private void OnRealized(object sender, EventArgs e)
        {
            CreateBrowser();
        }

        private void CreateBrowser()
        {
            var windowInfo = CefWindowInfo.Create();

#if LINUX
            var windowHandle = InteropLinux.gtk_widget_get_window(Handle);
            var xid = InteropLinux.gdk_x11_window_get_xid(windowHandle);

            var rect = new CefRectangle(0, 0, AllocatedWidth, AllocatedHeight);
            windowInfo.SetAsChild(xid, rect);
#endif

            Browser.Create(windowInfo, _startUrl);
            _initialized = true;
            _startUrl = null;
        }

        public void LoadUrl(string url)
        {
            if (_created)
            {
                Browser.CefBrowser.StopLoad();
                Browser.CefBrowser.GetMainFrame().LoadUrl(url);
            }
            else
            {
                _startUrl = url;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Browser.Dispose();
        }
    }
}
