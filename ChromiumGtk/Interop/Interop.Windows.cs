// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;

namespace Lunixo.ChromiumGtk.Interop
{
    public class InteropWindows
    {
        [DllImport(Library.GdkLib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_win32_window_get_handle(IntPtr window);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    }
}
