using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Jupiter;
using Microsoft.Win32.SafeHandles;
namespace WarSteroid
{
    class Program
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(long dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory2(IntPtr hProcess,
            long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess,
            Int64 lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress,
            byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;
        static void Main()
        {

            var wc3baseaddr = ProcessManagement.GetModuleBaseAddress("Warcraft III","Warcraft III.exe");

            Process process = Process.GetProcessesByName("Warcraft III")[0];
            IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);
            RAMReader(processHandle); 
           
            
        }

        public static void RAMReader(IntPtr processHandle)
        {

            int bytesRead = 0;
            byte[] buffer = new byte[1024];

            ReadProcessMemory(processHandle, 0x167652EFAB4, buffer, buffer.Length, ref bytesRead);

            string data = Encoding.ASCII.GetString(buffer);
            string hex_str = BitConverter.ToString(buffer);


            Console.WriteLine(Encoding.ASCII.GetString(buffer) + " (" + bytesRead.ToString() + "bytes)");
            Console.WriteLine(BitConverter.ToString(buffer) + " (" + bytesRead.ToString() + "bytes)");

        }



        public static void MapProcessedData(string data, string hex_data)
        {

        }


        
    }
}
