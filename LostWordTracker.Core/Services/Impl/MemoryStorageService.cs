﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordTracker.Core.Services.Impl
{
    public class MemoryStorageService : IGenericLocalStorageService
    {

        private Dictionary<string, object> _storage = new Dictionary<string, object>();

        public ValueTask<T> GetItemAsync<T>(string key, CancellationToken? cancellationToken = null)
        {
            if (_storage.ContainsKey(key))
                return new ValueTask<T>((T)_storage[key]);
            else
                return new ValueTask<T>();
        }

        public ValueTask SetItemAsync<T>(string key, T data, CancellationToken? cancellationToken = null)
        {
            _storage.Add(key, data);
            return new ValueTask();
        }
    }
}
