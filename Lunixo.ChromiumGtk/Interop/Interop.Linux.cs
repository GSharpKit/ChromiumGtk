﻿// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Lunixo.ChromiumGtk.Interop
{
    public delegate void GClosureNotify();

    [Flags]
    public enum GApplicationFlags
    {
        None
    }

    [Flags]
    public enum GtkWindowType
    {
        GtkWindowToplevel,
        GtkWindowPopup
    }

    [Flags]
    public enum GtkEvent
    {
        GdkDestroy = 1,
        GdkExpose = 2,
        GdkMotionNotify = 3,
        GdkButtonPress = 4,
        Gdk_2ButtonPress = 5,
        Gdk_3ButtonPress = 6,
        GdkButtonRelease = 7,
        GdkKeyPress = 8,
        GdkKeyRelease = 9,
        GdkEnterNotify = 10,
        GdkLeaveNotify = 11,
        GdkFocusChange = 12,
        GdkConfigure = 13,
    }

    [Flags]
    public enum GtkWindowPosition
    {
        GtkWinPosNone,
        GtkWinPosCenter,
        GtkWinPosMouse,
        GtkWinPosCenterAlways,
        GtkWinPosCenterOnParent
    }

    [Flags]
    public enum GConnectFlags
    {
        /// <summary>
        /// whether the handler should be called before or after the default handler of the signal.
        /// </summary>
        GConnectAfter,

        /// <summary>
        /// whether the instance and data should be swapped when calling the handler; see g_signal_connect_swapped() for an example.
        /// </summary>
        GConnectSwapped
    }

    public partial class InteropLinux
    {
        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gdk_set_allowed_backends(string backend);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_init(int argc, string[] argv);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gtk_window_new(GtkWindowType type);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
        public static extern void gtk_main();

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

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_widget_show_all(IntPtr window);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_window_get_size(IntPtr window, out int width, out int height);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gtk_window_set_title(IntPtr window, string title);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_window_set_default_size(IntPtr window, int width, int height);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gdk_window_resize(IntPtr window, int width, int height);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gdk_window_move_resize(IntPtr window, int x, int y, int width, int height);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_x11_visual_get_xvisual(IntPtr handle);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = false)]
        public static extern bool gtk_window_set_icon_from_file(IntPtr raw, string filename, out IntPtr err);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool gtk_window_set_position(IntPtr window, GtkWindowPosition position);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool gtk_window_maximize(IntPtr window);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool gtk_window_fullscreen(IntPtr window);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_main_quit();

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_screen_get_default();

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_x11_screen_lookup_visual(IntPtr screen, IntPtr xvisualid);

        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_screen_list_visuals(IntPtr raw);
        
        
        
        // Signals
        [DllImport(Library.GObjLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint g_signal_connect_data(IntPtr instance, string detailedSignal, IntPtr handler,
            IntPtr data, GClosureNotify destroyData, GConnectFlags connectFlags);

        // MessageBox

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern int gtk_dialog_run(IntPtr raw);

        [DllImport(Library.GtkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_widget_destroy(IntPtr widget);
        
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

                var gdkScreen = InteropLinux.gdk_screen_get_default();
                var gdkVisualList = InteropLinux.gdk_screen_list_visuals(gdkScreen);

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
                        var currVisual = InteropLinux.gdk_x11_visual_get_xvisual(currItem);
                        var currVisualId = XVisualIDFromVisual(currVisual);
                        if (visualId == currVisualId)
                        {
                            var gdkVisual = InteropLinux.gdk_x11_screen_lookup_visual(gdkScreen, currVisualId);
                            InteropLinux.gtk_widget_set_visual(widget, gdkVisual);
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

        #region X11

        [StructLayout(LayoutKind.Sequential)]
        public struct XVisualInfo
        {
            public IntPtr visual;
            public IntPtr visualid;
            public int screen;
            public uint depth;
            public int klass;
            public IntPtr red_mask;
            public IntPtr green_mask;
            public IntPtr blue_mask;
            public int colormap_size;
            public int bits_per_rgb;
        }

        public delegate short XHandleXError(IntPtr display, ref XErrorEvent error_event);
        public delegate short XHandleXIOError(IntPtr display);

        [DllImport(Library.X11Lib)]
        public static extern int XMoveWindow(IntPtr display, IntPtr w, int x, int y);

        [DllImport(Library.X11Lib)]
        public static extern int XResizeWindow(IntPtr display, IntPtr w, int width, int height);

        [DllImport(Library.X11Lib)]
        public static extern int XMoveResizeWindow(IntPtr display, IntPtr w, int x, int y, int width, int height);

        [DllImport(Library.X11Lib)]
        public static extern IntPtr XOpenDisplay(IntPtr display);

        [DllImport(Library.X11Lib)]
        public static extern int XCloseDisplay(IntPtr display);

        [DllImport(Library.X11Lib)]
        public static extern int XDefaultScreen(IntPtr display);

        [DllImport(Library.X11Lib)]
        public static extern IntPtr XDefaultVisual(IntPtr display, int screen);

        [DllImport(Library.X11Lib)]
        public static extern IntPtr XVisualIDFromVisual(IntPtr visual);

        [DllImport(Library.X11Lib)]
        public static extern short XSetErrorHandler(XHandleXError err);

        [DllImport(Library.X11Lib)]
        public static extern short XSetIOErrorHandler(XHandleXIOError err);

        [DllImport(Library.X11Lib)]
        public extern static IntPtr XGetErrorText(IntPtr display, byte code, StringBuilder buffer, int length);

        [StructLayout(LayoutKind.Sequential)]
        public struct XErrorEvent
        {
            public int type;
            public IntPtr display;
            public int resourceid;
            public int serial;
            public byte error_code;
            public byte request_code;
            public byte minor_code;
        }

        public static string GetRequestType(int requestId)
        {
            switch (requestId)
            {
                case 1: return "X_CreateWindow";
                case 2: return "X_ChangeWindowAttributes";
                case 3: return "X_GetWindowAttributes";
                case 4: return "X_DestroyWindow";
                case 5: return "X_DestroySubwindows";
                case 6: return "X_ChangeSaveSet";
                case 7: return "X_ReparentWindow";
                case 8: return "X_MapWindow";
                case 9: return "X_MapSubwindows";
                case 10: return "X_UnmapWindow";
                case 11: return "X_UnmapSubwindows";
                case 12: return "X_ConfigureWindow";
                case 13: return "X_CirculateWindow";
                case 14: return "X_GetGeometry";
                case 15: return "X_QueryTree";
                case 16: return "X_InternAtom";
                case 17: return "X_GetAtomName";
                case 18: return "X_ChangeProperty";
                case 19: return "X_DeleteProperty";
                case 20: return "X_GetProperty";
                case 21: return "X_ListProperties";
                case 22: return "X_SetSelectionOwner";
                case 23: return "X_GetSelectionOwner";
                case 24: return "X_ConvertSelection";
                case 25: return "X_SendEvent";
                case 26: return "X_GrabPointer";
                case 27: return "X_UngrabPointer";
                case 28: return "X_GrabButton";
                case 29: return "X_UngrabButton";
                case 30: return "X_ChangeActivePointerGrab";
                case 31: return "X_GrabKeyboard";
                case 32: return "X_UngrabKeyboard";
                case 33: return "X_GrabKey";
                case 34: return "X_UngrabKey";
                case 35: return "X_AllowEvents";
                case 36: return "X_GrabServer";
                case 37: return "X_UngrabServer";
                case 38: return "X_QueryPointer";
                case 39: return "X_GetMotionEvents";
                case 40: return "X_TranslateCoords";
                case 41: return "X_WarpPointer";
                case 42: return "X_SetInputFocus";
                case 43: return "X_GetInputFocus";
                case 44: return "X_QueryKeymap";
                case 45: return "X_OpenFont";
                case 46: return "X_CloseFont";
                case 47: return "X_QueryFont";
                case 48: return "X_QueryTextExtents";
                case 49: return "X_ListFonts";
                case 50: return "X_ListFontsWithInfo";
                case 51: return "X_SetFontPath";
                case 52: return "X_GetFontPath";
                case 53: return "X_CreatePixmap";
                case 54: return "X_FreePixmap";
                case 55: return "X_CreateGC";
                case 56: return "X_ChangeGC";
                case 57: return "X_CopyGC";
                case 58: return "X_SetDashes";
                case 59: return "X_SetClipRectangles";
                case 60: return "X_FreeGC";
                case 61: return "X_ClearArea";
                case 62: return "X_CopyArea";
                case 63: return "X_CopyPlane";
                case 64: return "X_PolyPoint";
                case 65: return "X_PolyLine";
                case 66: return "X_PolySegment";
                case 67: return "X_PolyRectangle";
                case 68: return "X_PolyArc";
                case 69: return "X_FillPoly";
                case 70: return "X_PolyFillRectangle";
                case 71: return "X_PolyFillArc";
                case 72: return "X_PutImage";
                case 73: return "X_GetImage";
                case 74: return "X_PolyText8";
                case 75: return "X_PolyText16";
                case 76: return "X_ImageText8";
                case 77: return "X_ImageText16";
                case 78: return "X_CreateColormap";
                case 79: return "X_FreeColormap";
                case 80: return "X_CopyColormapAndFree";
                case 81: return "X_InstallColormap";
                case 82: return "X_UninstallColormap";
                case 83: return "X_ListInstalledColormaps";
                case 84: return "X_AllocColor";
                case 85: return "X_AllocNamedColor";
                case 86: return "X_AllocColorCells";
                case 87: return "X_AllocColorPlanes";
                case 88: return "X_FreeColors";
                case 89: return "X_StoreColors";
                case 90: return "X_StoreNamedColor";
                case 91: return "X_QueryColors";
                case 92: return "X_LookupColor";
                case 93: return "X_CreateCursor";
                case 94: return "X_CreateGlyphCursor";
                case 95: return "X_FreeCursor";
                case 96: return "X_RecolorCursor";
                case 97: return "X_QueryBestSize";
                case 98: return "X_QueryExtension";
                case 99: return "X_ListExtensions";
                case 100: return "X_ChangeKeyboardMapping";
                case 101: return "X_GetKeyboardMapping";
                case 102: return "X_ChangeKeyboardControl";
                case 103: return "X_GetKeyboardControl";
                case 104: return "X_Bell";
                case 105: return "X_ChangePointerControl";
                case 106: return "X_GetPointerControl";
                case 107: return "X_SetScreenSaver";
                case 108: return "X_GetScreenSaver";
                case 109: return "X_ChangeHosts";
                case 110: return "X_ListHosts";
                case 111: return "X_SetAccessControl";
                case 112: return "X_SetCloseDownMode";
                case 113: return "X_KillClient";
                case 114: return "X_RotateProperties";
                case 115: return "X_ForceScreenSaver";
                case 116: return "X_SetPointerMapping";
                case 117: return "X_GetPointerMapping";
                case 118: return "X_SetModifierMapping";
                case 119: return "X_GetModifierMapping";
                case 127: return "X_NoOperation";
            }

            return "NotFound";
        }

        #endregion
        
    }
}
