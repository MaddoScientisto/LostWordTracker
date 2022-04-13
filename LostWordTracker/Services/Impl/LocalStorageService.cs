using LostWordTracker.Core.Services;
using System.Threading;
using System.Threading.Tasks;

namespace LostWordTracker.Services.Impl
{
    public class LocalStorageService : IGenericLocalStorageService
    {
        private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;
        public LocalStorageService(Blazored.LocalStorage.ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public ValueTask<T> GetItemAsync<T>(string key, CancellationToken? cancellationToken = null)
        {
            return _localStorage.GetItemAsync<T>(key, cancellationToken);
        }

        public ValueTask SetItemAsync<T>(string key, T data, CancellationToken? cancellationToken = null)
        {
            return (_localStorage.SetItemAsync<T>(key, data, cancellationToken));
        }
    }
}
