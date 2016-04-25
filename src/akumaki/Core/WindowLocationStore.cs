using akumaki.Properties;
using Microsoft.WindowsAPICodePack.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace akumaki.Core
{
    /// <summary>
    /// Saves and restores windows's location.
    /// 
    /// In some cases using DisplayPort to connect Monitors,
    /// windows' location are changed unintentionally when monitor turns off/on.
    /// 
    /// To avoid above problem,
    /// saves windows's location when monitor turns off,
    /// and restores windows's location when monitor turns on.
    /// </summary>
    public class WindowLocationStore : IDisposable
    {
        #region Win32APIs for windows' location management. (Win32APIs are required since WPF doesn't provide such APIs.)

        public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

        [DllImport("user32.Dll")]
        static extern int IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd,
            StringBuilder lpString, int nMaxCount);

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("User32.Dll")]
        static extern int GetWindowRect(IntPtr hWnd, out RECT rect);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, int bRepaint);

        #endregion Win32APIs for windows' location management. (Win32APIs are required since WPF doesn't provide such APIs.)

        #region Singleton (in order to dispose static resources)

        /// <summary>
        /// a WindowLocationStore instance.
        /// Uses singleton pattern in order to dispose static resources.
        /// </summary>
        protected static WindowLocationStore Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates single instance
        /// </summary>
        static WindowLocationStore()
        {
            Instance = new WindowLocationStore();
        }

        #endregion Singleton (in order to dispose static resources)

        /// <summary>
        /// Initializes this class
        /// </summary>
        public static void Initialize()
        {
            ResigterEventHandlers();
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        public void Dispose()
        {
            UnregisterEventHandlers();
        }

        /// <summary>
        /// Registers event handlers
        /// </summary>
        private static void ResigterEventHandlers()
        {
            PowerManager.IsMonitorOnChanged += PowerManager_IsMonitorOnChanged;
        }

        /// <summary>
        /// Unregisters event handlers
        /// </summary>
        private static void UnregisterEventHandlers()
        {
            PowerManager.IsMonitorOnChanged -= PowerManager_IsMonitorOnChanged;
        }

        /// <summary>
        /// IsMonitorOnChanged event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void PowerManager_IsMonitorOnChanged(object sender, System.EventArgs e)
        {
            if (PowerManager.IsMonitorOn)
            {
                Task.Run(() =>
                {
                    // Windows' location cannot be moved immediately after monitor turns on.
                    // It needs to wait for a few second.
                    //
                    // FIXME It depends on hard-ware settings that how long needs to wait for.
                    // Taking other ways (i.e. not using IsMonitorOnChanged event) is better,
                    // however, I couldn't find other good solutions.
                    //
                    // TODO add setting window to change the wait time
                    Thread.Sleep(Settings.Default.WaitTimeAfterMonitorOn);
                    RestoreWindowLocation();
                });
                return;
            }

            SaveWindowLocation();
        }

        #region Save and restore windows' location

        /// <summary>
        /// Dictionary to save pairs (window's handle, window's location)
        /// </summary>
        private static Dictionary<IntPtr, RECT> windowLocationDictionary = new Dictionary<IntPtr, RECT>();

        /// <summary>
        /// Saves window location
        /// </summary>
        private static void SaveWindowLocation()
        {
            windowLocationDictionary.Clear();

            EnumWindows((hWnd, lpalam) =>
            {
                if (IsWindowVisible(hWnd) > 0)
                {
                    RECT rect;
                    GetWindowRect(hWnd, out rect);
                    windowLocationDictionary.Add(hWnd, rect);

                    #region debug
                    StringBuilder tsb = new StringBuilder();
                    GetWindowText(hWnd, tsb, tsb.Capacity);
                    Debug.WriteLine("Title: " + tsb.ToString());
                    Debug.WriteLine("Rect: " + rect.left + ", " + rect.top + ", " + rect.right + ", " + rect.bottom);
                    #endregion debug
                }

                return true;
            },
            IntPtr.Zero);
        }

        /// <summary>
        /// Restores windows' location
        /// </summary>
        private static void RestoreWindowLocation()
        {
            foreach (var windowLocationKV in windowLocationDictionary)
            {
                var windowRect = windowLocationKV.Value;
                MoveWindow(windowLocationKV.Key, windowRect.left, windowRect.top, windowRect.right - windowRect.left, windowRect.bottom - windowRect.top, 1);
            }

            // release the window handles
            windowLocationDictionary.Clear();
        }

        #endregion Save and restore windows' location
    }
}
