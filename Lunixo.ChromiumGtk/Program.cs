namespace Lunixo.ChromiumGtk.Examples.Container
{
    using System;
    using System.IO;
    using System.Reflection;

    using global::Lunixo.ChromiumGtk;
    using global::Lunixo.ChromiumGtk.Core;
    using global::Lunixo.ChromiumGtk.Interop;

    using Gtk;

    using Xilium.CefGlue;

    internal class Program
    {
        static string cefPath = "/usr/lib/cef";

        private static void Main(string[] args)
        {
            var runtime = new Runtime(new CefSettings
            {
                MultiThreadedMessageLoop = false,
                LogSeverity = CefLogSeverity.Disable,
                //LogFile = $"logs_{DateTime.Now:yyyyMMdd}.log",
                //RemoteDebuggingPort = 20480,
                LocalesDirPath = Path.Combine(cefPath, "locales"),
                BrowserSubprocessPath = Path.Combine(cefPath, "cefsimple"),
                ResourcesDirPath = Path.Combine(cefPath),
                NoSandbox = true,
                BackgroundColor = new CefColor(0, 0, 0, 0),
                WindowlessRenderingEnabled = true,
                ExternalMessagePump = false,
                Locale = "en-US",
                CommandLineArgsDisabled = false,
            }, new[]
            {
                "--headless", 
                "--disable-gpu",
            });
            runtime.Initialize();

            Application.Init();
            
            using var window = new Window("Lunixo Example")
            {
                WidthRequest = 1200,
                HeightRequest = 800
            };
            
            window.Destroyed += (sender, args) => runtime.QuitMessageLoop();
            InteropLinux.SetDefaultWindowVisual(window.Handle);

            using var webView = new Lunixo.ChromiumGtk.WebViewX();
            //webView.LoadUrl("https://dotnet.microsoft.com/");
            webView.LoadUrl("https://www.google.com/");
            
            window.Add(webView);
            window.ShowAll();

            runtime.RunMessageLoop();
        }
    }
}
