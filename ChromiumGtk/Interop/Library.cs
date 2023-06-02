// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Xilium.CefGlue.Interop
{
    internal static partial class libcef
    {
#if LINUX
        internal const string DllName = "/usr/lib/cef/libcef.so";
#endif
#if MACOS
        internal const string DllName = "/Library/Frameworks/GSharpKit/lib/cef/libcef.dylib";
#endif
#if WINDOWS
        internal const string DllName = @"C:\Program Files\GSharpKit\bin\cef\libcef.dll";
#endif
    }
}

namespace Lunixo.ChromiumGtk.Interop
{
    public static class Library
    {
#if WINDOWS
        internal const string GtkLib = "Gtk-3-0.dll";
        internal const string GdkLib = "libgdk-3-0.dll";
        internal const string GlibLib = "libglib-2.0-0.dll";
#endif
#if LINUX
        internal const string GtkLib = "libgtk-3.so.0";
        internal const string GdkLib = "libgdk-3.so.0";
        internal const string GlibLib = "libglib-2.0.so.0";
        internal const string X11Lib = "libX11.so.6";
#endif
    }
}
