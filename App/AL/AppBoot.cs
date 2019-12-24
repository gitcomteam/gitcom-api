using System;
using App.DL.Module.Cache;
using Micron.DL.Model;

namespace App.AL {
    public static class AppBoot {
        public static void Boot() {
            Console.WriteLine("Booting...");
            
            ModelCache.Reset();
            ModelCache.StartAutoResetTask();
            
            Console.WriteLine("Boot finished.");
        }
    }
}