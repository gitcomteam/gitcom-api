using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// TODO: replace or extend with Redis
// TODO: debug potential exceptions and remove try catch to improve performance (if it helps)
namespace App.DL.Module.Cache {
    public static class ModelCache {
        private const int CacheSize = 100;
        
        private static Dictionary<string, Dictionary<int, object>> _intCache = new Dictionary<string, Dictionary<int, object>>();
        
        private static Dictionary<string, Dictionary<string, object>> _strCache = new Dictionary<string, Dictionary<string, object>>();

        public static void Store(string cacheName, string guid, object newObject) {
            try {
                if (!_strCache.ContainsKey(cacheName)) {
                    _strCache.Add(cacheName, new Dictionary<string, object>());
                }

                if (_strCache[cacheName].ContainsKey(guid)) {
                    return;
                }

                _strCache[cacheName][guid] = newObject;
                CleanUp(cacheName);
            }
            catch (Exception e) {
                //
            }
        }
        
        public static void Store(string cacheName, int id, object newObject) {
            try {
                if (!_intCache.ContainsKey(cacheName)) {
                    _intCache.Add(cacheName, new Dictionary<int, object>());
                }

                if (_intCache[cacheName].ContainsKey(id)) {
                    return;
                }

                _intCache[cacheName].Add(id, newObject);
                CleanUp(cacheName);
            }
            catch (Exception e) {
                //
            }
        }
        
        public static object Get(string cacheName, int id) {
            try {
                if (!_intCache.ContainsKey(cacheName)) {
                    _intCache.Add(cacheName, new Dictionary<int, object>());
                }

                if (!_intCache[cacheName].ContainsKey(id)) {
                    return null;
                }

                return _intCache[cacheName][id];
            }
            catch (Exception e) {
                return null;
            }
        }

        public static object Get(string cacheName, string guid) {
            try {
                if (!_strCache.ContainsKey(cacheName)) {
                    _strCache.Add(cacheName, new Dictionary<string, object>());
                }

                if (!_strCache[cacheName].ContainsKey(guid)) {
                    return null;
                }

                return _strCache[cacheName][guid];
            }
            catch (Exception e) {
                return null;
            }
        }

        public static void CleanUp(string cacheName) {
            if (_intCache.ContainsKey(cacheName) && _intCache[cacheName].Count > CacheSize) {
                foreach (int key in _intCache[cacheName].Keys) {
                    _intCache[cacheName].Remove(key);
                    return;
                }
            }
            
            if (_strCache.ContainsKey(cacheName) && _strCache[cacheName].Count > CacheSize) {
                foreach (string key in _strCache[cacheName].Keys) {
                    _strCache[cacheName].Remove(key);
                    return;
                }
            }
        }
        
        public static void CleanUp(string cacheName, int id) {
            if (_intCache.ContainsKey(cacheName) && _intCache[cacheName].ContainsKey(id)) {
                _intCache[cacheName].Remove(id);
            }
        }

        public static void CleanUp(string cacheName, string guid) {
            if (_strCache.ContainsKey(cacheName) && _strCache[cacheName].ContainsKey(guid)) {
                _strCache[cacheName].Remove(guid);
            }
        }

        public static void Reset() {
            _intCache = new Dictionary<string, Dictionary<int, object>>();
            _strCache = new Dictionary<string, Dictionary<string, object>>();
        }

        public static void StartAutoResetTask() {
            Task.Run(async () => {
                const int delayMs = 600000; // 10 minutes
                while (true) {
                    Console.WriteLine("Cache cleaned up at: UTC " + DateTime.UtcNow);
                    await Task.Delay(delayMs);
                }
            });
        }
    }
}