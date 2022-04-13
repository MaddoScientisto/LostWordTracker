using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordTracker.Core.Services
{
    public  interface IGenericLocalStorageService
    {
        ValueTask SetItemAsync<T>(string key, T data, CancellationToken? cancellationToken = null);

        ValueTask<T> GetItemAsync<T>(string key, CancellationToken? cancellationToken = null);
    }
}
