using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Jupiter;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Jupiter;
using Microsoft.Win32.SafeHandles;
using System.Linq;

namespace WarSteroid
{
    class ProcessManagement
    {
        [DllImport("kernel32.dll")]
        internal static extern bool IsWow64Process(SafeProcessHandle processHandle, out bool isWow64Process);



        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        public static IntPtr FindDMAAddy(IntPtr hProc, IntPtr ptr, int[] offsets)
        {
            var buffer = new byte[IntPtr.Size];

            foreach (int i in offsets)
            {
                ReadProcessMemory(hProc, ptr, buffer, buffer.Length, out var read);

                ptr = (IntPtr.Size == 4)
                ? IntPtr.Add(new IntPtr(BitConverter.ToInt32(buffer, 0)), i)
                : ptr = IntPtr.Add(new IntPtr(BitConverter.ToInt64(buffer, 0)), i);
            }
            return ptr;
        }

       
    public static IntPtr GetModuleBaseAddress(string processName, string moduleName)
        {
            // Get an instance of the specified process
            Process process;

            try
            {
                process = Process.GetProcessesByName(processName)[0];
            }

            catch (IndexOutOfRangeException)
            {
                // The process isn't currently running
                throw new ArgumentException($"No process with name {processName} is currently running");
            }

            // Get an instance of the specified module in the process
            // We use linq here to avoid unnecesary for loops
            Console.WriteLine("Searching....");
            var module = process.Modules.Cast<ProcessModule>().SingleOrDefault(m => string.Equals(m.ModuleName, moduleName, StringComparison.OrdinalIgnoreCase));

            // Attempt to get the base address of the module - Return IntPtr.Zero if the module doesn't exist in the process
            Console.WriteLine("Found!");
            return module?.BaseAddress ?? IntPtr.Zero;
        }
    }
}
