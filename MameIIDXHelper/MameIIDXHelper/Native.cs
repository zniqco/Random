using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace MameIIDXHelper
{
    public class Native
    {
        private delegate IntPtr WndProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpText, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, WndProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const uint KEYEVENTF_SCANCODE = 0x0008;

        private static IntPtr currentKeyHook = IntPtr.Zero;
        private static WndProc hookProcDelegate = KeyboardHookProc;
        private static Dictionary<int, Queue<int>> remapKeycodes = new Dictionary<int, Queue<int>>();

        public static string GetForegroundWindowText()
        {
            var buffer = new StringBuilder(256);
            var handle = GetForegroundWindow();

            if (GetWindowText(handle, buffer, buffer.Capacity) > 0)
                return buffer.ToString();

            return null;
        }

        public static string GetForegroundWindowClass()
        {
            var buffer = new StringBuilder(256);
            var handle = GetForegroundWindow();

            if (GetClassName(handle, buffer, buffer.Capacity) > 0)
                return buffer.ToString();

            return null;
        }

        public static void ClearRemapKeycode()
        {
            remapKeycodes.Clear();
        }

        public static void AddRemapKeycode(int keycode, int remapKeycode)
        {
            Queue<int> remapQueue;

            if (!remapKeycodes.TryGetValue(keycode, out remapQueue))
                remapQueue = remapKeycodes[keycode] = new Queue<int>();

            remapQueue.Enqueue(remapKeycode);
        }

        public static void SetRemapKeycodeEnabled(bool enabled)
        {
            if (enabled)
            {
                if (currentKeyHook == IntPtr.Zero)
                {
                    using (Process process = Process.GetCurrentProcess())
                    using (ProcessModule module = process.MainModule)
                    {
                        currentKeyHook = SetWindowsHookEx(WH_KEYBOARD_LL, hookProcDelegate, GetModuleHandle(module.ModuleName), 0);
                    }
                }
            }
            else
            {
                if (currentKeyHook != IntPtr.Zero)
                {
                    UnhookWindowsHookEx(currentKeyHook);
                    currentKeyHook = IntPtr.Zero;
                }
            }
        }        

        private static IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                if ((int)wParam == WM_KEYDOWN || (int)wParam == WM_KEYUP)
                {
                    var keycode = Marshal.ReadInt32(lParam);

                    if (remapKeycodes.TryGetValue(keycode, out var remapKeyCodes))
                    {
                        if (remapKeyCodes.Peek() >= 0)
                        {
                            if ((int)wParam == WM_KEYDOWN)
                            {
                                var remapKeycode = remapKeyCodes.Peek();

                                keybd_event(0, (byte)remapKeycode, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_SCANCODE, UIntPtr.Zero);
                            }
                            else
                            {
                                var remapKeycode = remapKeyCodes.Dequeue();

                                keybd_event(0, (byte)remapKeycode, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP, UIntPtr.Zero);

                                remapKeyCodes.Enqueue(remapKeycode);
                            }
                        }

                        return (IntPtr)1;
                    }
                }
            }

            return CallNextHookEx(currentKeyHook, code, wParam, lParam);
        }
    }
}
