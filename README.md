
Similar active projects:

* https://github.com/lunixo/ChromiumGtk (Fork)
* https://github.com/OutSystems/CefGlue
* https://github.com/chromelyapps/Chromely


# ChromiumGtk

ChromiumGtk is a WebView for GtkSharp using Chromium Embedded Framework (CEF) and CefGlue.


## Supported Platforms

* .NET6 on Fedora 38 (AMD64)
* .NET6 on Windows 10/11 (AMD64)

## Dependencies

* [GtkSharp](https://github.com/GtkSharp/GtkSharp)
* [CEF](https://bitbucket.org/chromiumembedded/cef/)

* https://cef-builds.spotifycdn.com/cef_binary_112.3.0%2Bgb09c4ca%2Bchromium-112.0.5615.165_linux64_client.tar.bz2 install in /usr/lib/cef

## Usage

```C#
static void Main(string[] args)
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
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)

This repository uses code from [CefGlue](https://gitlab.com/xiliumhq/chromiumembedded/cefglue) (MIT) and [Chromely](https://github.com/chromelyapps/Chromely) (MIT)
