using System;
using App.DL.Module.Cache;

namespace App.AL {
    public static class AppBoot {
        public static void Boot() {
            Console.WriteLine("Booting...");
            
            ModelCache.StartAutoResetTask();
            
            Console.WriteLine("Boot finished.");
        }
    }
}