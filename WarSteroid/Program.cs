using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

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

            [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_BASIC_INFORMATION64
    {
        public ulong BaseAddress;
        public ulong AllocationBase;
        public int AllocationProtect;
        public int __alignment1;
        public ulong RegionSize;
        public int State;
        public int Protect;
        public int Type;
        public int __alignment2;
    }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION64 lpBuffer, uint dwLength);

        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;
        static void Main()
        {

            var wc3baseaddr = ProcessManagement.GetModuleBaseAddress("Warcraft III","Warcraft III.exe");

            Process process = Process.GetProcessesByName("Warcraft III")[0];
            IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);
            //RAMReader(processHandle);
            TestVritualQEx();



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

        public static void TestVritualQEx()
        {
            Process process = Process.GetProcessesByName("Warcraft III")[0];


            long MaxAddress = 0x7fffffff;
            long address = 0;

            do
            {
          
                int result = VirtualQueryEx(process.Handle, (IntPtr)address, out m, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION64)));
                Console.WriteLine("{0}-{1} : {2} bytes result={3}", m.BaseAddress, (uint)m.BaseAddress + (uint)m.RegionSize - 1, m.RegionSize, result);
                if (address == (long)m.BaseAddress + (long)m.RegionSize)
                    break;
                address = (long)m.BaseAddress + (long)m.RegionSize;
            } while (address <= MaxAddress);
        }

        
    }
}
