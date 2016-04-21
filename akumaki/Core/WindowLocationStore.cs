using System;
using System.Runtime.InteropServices;
using System.Text;

namespace akumaki.Core
{
    public class WindowLocationStore
    {
        public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.Dll")]
        static extern int IsWindowVisible(IntPtr hWnd);

        //        typedef struct _RECT 
        //                { 
        //                    LONG left; 
        //                    LONG top; 
        //                    LONG right; 
        //                    LONG bottom; 
        //                } RECT, *PRECT; 
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        //        BOOL GetWindowRect(
        //            HWND hWnd,
        //            LPRECT lpRect
        //            );        
        [DllImport("User32.Dll")]
        static extern int GetWindowRect(
            IntPtr hWnd,
            out RECT rect
            );

        //        HWND GetDesktopWindow(VOID);        
        [DllImport("User32.Dll")]
        static extern IntPtr GetDesktopWindow();

        public static void Test()
        {
            EnumWindows((hWnd, lpalam) =>
            {
                int textLen = GetWindowTextLength(hWnd);
                if (0 < textLen && IsWindowVisible(hWnd) > 0)
                {
                    StringBuilder tsb = new StringBuilder(textLen + 1);
                    GetWindowText(hWnd, tsb, tsb.Capacity);

                    StringBuilder csb = new StringBuilder(256);
                    GetClassName(hWnd, csb, csb.Capacity);

                    RECT rect;
                    GetWindowRect(hWnd, out rect);

                    Console.WriteLine("Class Name: " + csb.ToString());
                    Console.WriteLine("Title: " + tsb.ToString());
                    Console.WriteLine("Rect: " + rect.left + ", " + rect.top + ", " + rect.right + ", " + rect.bottom);
                }

                return true;
            },
            IntPtr.Zero);
        }
    }
}
