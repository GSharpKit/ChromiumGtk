using Lunixo.ChromiumGtk.Interop;

using Gtk;

namespace Lunixo.ChromiumGtk.Examples.Container
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WebView.Initialize();

            Application.Init();
            
            using var window = new Window("Chromium Gtk Example")
            {
                WidthRequest = 1200,
                HeightRequest = 800
            };
            
            window.Destroyed += (_, _) => WebView.Quit();
            #if LINUX
            InteropLinux.SetDefaultWindowVisual(window.Handle);
            #endif

            using var webView = new WebView();
            webView.LoadUrl("https://www.google.com/");
            
            window.Add(webView);
            window.ShowAll();

            WebView.Run();
            Application.Run();
        }
    }
}
