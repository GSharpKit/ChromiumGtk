using System;
using Gtk;
using Lunixo.ChromiumGtk.Core;
using Xilium.CefGlue;
using InteropLinux = Lunixo.ChromiumGtk.Interop.InteropLinux;

namespace Lunixo.ChromiumGtk
{
    public class WebView : Bin
    {
        private bool _initialized;
        private bool _created;
        private string _startUrl;
        private readonly Bin _container;

        static Runtime runtime;

#if LINUX
        const string subprocessName = "cefsimple";
        const string cefPath = "/usr/lib/cef";
#endif
#if WINDOWS
        const string subprocessName = "cefclient.exe";
        const string cefPath = @"C:\Program Files\GSharpKit\bin\cef";
#endif

        public WebView(CefBrowserSettings browserSettings = null)
        {
            _container = new EventBox()
            {
                Halign = Align.Fill,
                Valign = Align.Fill,
            };
            
            _container.Realized += OnRealized;
            
            Add(_container);
            
            Browser = new WebBrowser(browserSettings ?? CreateDefaultBrowserSettings());
            Browser.Created += BrowserOnCreated;
            SizeAllocated += OnSizeAllocated;
        }

        /// <summary>
        /// Initialize CEF Runtime
        /// </summary>
        public static void Initialize ()
        {
            //mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            runtime = new Runtime(new CefSettings
            {
                MultiThreadedMessageLoop = false,
                LogSeverity = CefLogSeverity.Disable,
                LocalesDirPath = System.IO.Path.Combine(cefPath, "locales"),
                BrowserSubprocessPath = System.IO.Path.Combine(cefPath, subprocessName), // Must have chmod 755
                ResourcesDirPath = System.IO.Path.Combine(cefPath), // Must have chmod 755
                NoSandbox = true,
                BackgroundColor = new CefColor(0, 0, 0, 0),
                WindowlessRenderingEnabled = true,
                ExternalMessagePump = true,
                Locale = "en-US",
                CommandLineArgsDisabled = false,
            }, new[]
            {
                "--headless",
                "--disable-gpu",
            });
            runtime.Initialize();
            runtime.DoMessageLoopWork();
        }

        public static void Run ()
        {
            GLib.Timeout.Add(10, OnIdlePump);
        }

        static bool OnIdlePump()
        {
            //Console.WriteLine ("OnIdlePump: " + IsMainThread);
            runtime.DoMessageLoopWork();
            return true;
        }

        public static void Quit ()
        {
            runtime.Shutdown();
        }

        public WebBrowser Browser { get; }

        public static CefBrowserSettings CreateDefaultBrowserSettings()
        {
            return new CefBrowserSettings
            {
                DefaultEncoding = "UTF-8",
            };
        }

        private void BrowserOnCreated(object sender, EventArgs e)
        {
            Browser.Created -= BrowserOnCreated;
            _created = true;

            if (_startUrl != null)
            {
                LoadUrl(_startUrl);
            }
        }

        private void OnSizeAllocated(object o, SizeAllocatedArgs args)
        {
            if (Handle != IntPtr.Zero && _initialized)
            {
                ResizeBrowser(args.Allocation.Width, args.Allocation.Height);
            }
        }


        protected virtual void ResizeBrowser (int width, int height)
        {
            try
            {
                if (!_created) return;

                var browserWindow = Browser.CefBrowser.GetHost ().GetWindowHandle ();

#if LINUX
                var gdkDisplay = InteropLinux.gtk_widget_get_display (_container.Handle);
                var x11Display = InteropLinux.gdk_x11_display_get_xdisplay (gdkDisplay);
                InteropLinux.XMoveResizeWindow (x11Display, browserWindow, 0, 0, width, height);
#endif

#if WINDOWS
				InteropWindows.SetWindowPos(browserWindow, 0, 0, 0, width, height, 0x0002 | 0x0004 | 0x0010); //SWP_NOMOVE|SWP_NOZORDER|SWP_NOACTIVATE);
#endif

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
            var windowHandle = InteropLinux.gtk_widget_get_window(_container.Handle);
            var xid = InteropLinux.gdk_x11_window_get_xid(windowHandle);

            var rect = new CefRectangle(0, 0, AllocatedWidth, AllocatedHeight);
            windowInfo.SetAsChild(xid, rect);
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
