// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;
using Xilium.CefGlue.Interop;

namespace Lunixo.ChromiumGtk.Interop
{
    public class InteropLinux
    {
        [DllImport(libcef.DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr cef_get_xdisplay();

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gtk_widget_get_window(IntPtr widget);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_widget_set_visual(IntPtr widget, IntPtr visual);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_x11_window_get_xid(IntPtr raw);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gtk_widget_get_display(IntPtr window);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_x11_display_get_xdisplay(IntPtr gdkDisplay);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_x11_visual_get_xvisual(IntPtr handle);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_screen_get_default();

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_x11_screen_lookup_visual(IntPtr screen, IntPtr xvisualid);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_screen_list_visuals(IntPtr raw);
        
        public static void SetDefaultWindowVisual(IntPtr widget)
        {
            // *** https://stackoverrun.com/hi/q/11294280
            try
            {
                // https://github.com/cztomczak/cefcapi/issues/9

                // GTK+ > 3.15.1 uses an X11 visual optimized for GTK+'s OpenGL stuff
                // since revid dae447728d: https://github.com/GNOME/gtk/commit/dae447728d
                // However, it breaks CEF: https://github.com/cztomczak/cefcapi/issues/9
                // Let's use the default X11 visual instead the GTK's blessed one.

                var xDisplay = XOpenDisplay(IntPtr.Zero);
                var screenNumber = XDefaultScreen(xDisplay);
                var xVisual = XDefaultVisual(xDisplay, screenNumber);
                var visualId = XVisualIDFromVisual(xVisual);

                var gdkScreen = gdk_screen_get_default();
                var gdkVisualList = gdk_screen_list_visuals(gdkScreen);

                if (gdkVisualList == IntPtr.Zero)
                {
                    Console.WriteLine("Warning in LinuxNativeMethods::SetDefaultWindowVisual: List of visuals is invalid.");
                    return;
                }

                var glistUtil = new GListUtil(gdkVisualList);
                int length = glistUtil.Length;

                for (int i = 0; i < length; i++)
                {
                    var currItem = glistUtil.GetItem(i);
                    if (currItem != IntPtr.Zero)
                    {
                        var currVisual = gdk_x11_visual_get_xvisual(currItem);
                        var currVisualId = XVisualIDFromVisual(currVisual);
                        if (visualId == currVisualId)
                        {
                            var gdkVisual = gdk_x11_screen_lookup_visual(gdkScreen, currVisualId);
                            gtk_widget_set_visual(widget, gdkVisual);
                            break;
                        }
                    }
                }

                glistUtil.Free();
                XCloseDisplay(xDisplay);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct XWindowChanges
        {
            public int x, y;
            public int width, height;
            public int border_width;
            public int sibling;
            public int stack_mode;
        }

        [Flags()]
        public enum XConfigureWindowMask
        {
            CWX = (1 << 0),
            CWY = (1 << 1),
            CWWidth = (1 << 2),
            CWHeight = (1 << 3),
            CWBorderWidth = (1 << 4),
            CWSibling = (1 << 5),
            CWStackMode = (1 << 6)
        }

        [DllImport(Library.X11Lib)]
        public static extern void XConfigureWindow(IntPtr display, IntPtr w, int value_mask, XWindowChanges values);

        [DllImport(Library.X11Lib)]
        public static extern int XMoveResizeWindow(IntPtr display, IntPtr w, int x, int y, int width, int height);

        [DllImport(Library.X11Lib)]
        public static extern IntPtr XOpenDisplay(IntPtr display);

        [DllImport(Library.X11Lib)]
        public static extern IntPtr XFlush(IntPtr display);

        [DllImport(Library.X11Lib)]
        public static extern int XCloseDisplay(IntPtr display);

        [DllImport(Library.X11Lib)]
        public static extern int XDefaultScreen(IntPtr display);

        [DllImport(Library.X11Lib)]
        public static extern IntPtr XDefaultVisual(IntPtr display, int screen);

        [DllImport(Library.X11Lib)]
        public static extern IntPtr XVisualIDFromVisual(IntPtr visual);
    }
}
