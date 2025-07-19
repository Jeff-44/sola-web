using System;
using System.Runtime.InteropServices;

namespace Sola_Web.Helpers
{
    public static class NativeLibraryLoader
    {
        public static IntPtr Load(string path)
        {
            return NativeLibrary.Load(path);
        }
    }
}
