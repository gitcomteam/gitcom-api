using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// TODO: replace or extend with Redis
namespace App.DL.Module.Cache {
    public static class ModelCache {
        private const int CacheSize = 100;
        
        private static Dictionary<string, Dictionary<int, object>> _idCache = new Dictionary<string, Dictionary<int, object>>();
        
        private static Dictionary<string, Dictionary<string, object>> _guidCache = new Dictionary<string, Dictionary<string, object>>();

        public static async void Store(string cacheName, string guid, object newObject) {
            if (!_guidCache.ContainsKey(cacheName)) {
                _guidCache.Add(cacheName, new Dictionary<string, object>());
            }
            if (_guidCache[cacheName].ContainsKey(guid)) {
                return;
            }
            _guidCache[cacheName][guid] = newObject;
            CleanUp(cacheName);
        }
        
        public static async void Store(string cacheName, int id, object newObject) {
            if (!_idCache.ContainsKey(cacheName)) {
                _idCache.Add(cacheName, new Dictionary<int, object>());
            }
            if (_idCache[cacheName].ContainsKey(id)) {
                return;
            }
            _idCache[cacheName].Add(id, newObject);
            CleanUp(cacheName);
        }
        
        public static object Get(string cacheName, int id) {
            if (!_idCache.ContainsKey(cacheName)) {
                _idCache.Add(cacheName, new Dictionary<int, object>());
            }
            if (!_idCache[cacheName].ContainsKey(id)) {
                return null;
            }

            return _idCache[cacheName][id];
        }

        public static object Get(string cacheName, string guid) {
            if (!_guidCache.ContainsKey(cacheName)) {
                _guidCache.Add(cacheName, new Dictionary<string, object>());
            }
            if (!_guidCache[cacheName].ContainsKey(guid)) {
                return null;
            }
            return _guidCache[cacheName][guid];
        }

        public static void CleanUp(string cacheName) {
            if (_idCache.ContainsKey(cacheName) && _idCache[cacheName].Count > CacheSize) {
                foreach (int key in _idCache[cacheName].Keys) {
                    _idCache[cacheName].Remove(key);
                    return;
                }
            }
            
            if (_guidCache.ContainsKey(cacheName) && _guidCache[cacheName].Count > CacheSize) {
                foreach (string key in _guidCache[cacheName].Keys) {
                    _guidCache[cacheName].Remove(key);
                    return;
                }
            }
        }
        
        public static void CleanUp(string cacheName, int id) {
            if (_idCache.ContainsKey(cacheName) && _idCache[cacheName].ContainsKey(id)) {
                _idCache[cacheName].Remove(id);
            }
        }

        public static void CleanUp(string cacheName, string guid) {
            if (_guidCache.ContainsKey(cacheName) && _guidCache[cacheName].ContainsKey(guid)) {
                _guidCache[cacheName].Remove(guid);
            }
        }

        public static void Reset() {
            _idCache = new Dictionary<string, Dictionary<int, object>>();
            _guidCache = new Dictionary<string, Dictionary<string, object>>();
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