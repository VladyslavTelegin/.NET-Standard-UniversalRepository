namespace UniversalRepository.Wrappers
{
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;

    using System;

    using UniversalRepository.Exceptions;
    using UniversalRepository.Infrastructure;

    public class MemoryCacheWrapper : IDisposable
    {
        #region PrivateFields 

        private IMemoryCache _memoryCache;
        private IOptions<MemoryCacheOptions> _memoryCacheOptions;

        private bool _isEnabled = true;

        #endregion

        #region Constructors

        public MemoryCacheWrapper(IOptions<MemoryCacheOptions> options = null)
        {
            _memoryCacheOptions = options ?? new MemoryCacheOptions();
            _memoryCache = new MemoryCache(_memoryCacheOptions);
        }

        #endregion

        #region Properties

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                if (_isEnabled && _memoryCache != null)
                {
                    _memoryCache = new MemoryCache(_memoryCacheOptions);
                }
                else if (!_isEnabled && _memoryCache != null)
                {
                    try
                    {
                        _memoryCache.Dispose();
                        _memoryCache = null;
                    } 
                    catch (Exception ex) when (ex is ObjectDisposedException || ex is NullReferenceException)
                    {
                        var logMessage = $"{DateTime.UtcNow}: {nameof(MemoryCacheWrapper)}.{IsEnabled} -> {ex.Message};";
                        Logger.Instance.CreateLog(logMessage);
                    }
                }
            }
        }

        #endregion

        #region PublicMethods

        public bool TryGetValue<T>(string key, out T @object)
        {
            if (this.IsEnabled)
            {
                try
                {
                    return _memoryCache.TryGetValue<T>(key, out @object);
                }
                catch (Exception ex)
                {
                    throw new MemoryCacheWrapperException(ex.Message);
                }
            }

            @object = default(T);

            return false;
        }

        public T Get<T>(string key)
        {
            if (this.IsEnabled)
            {
                return _memoryCache.Get<T>(key);
            }

            return default(T);
        }

        public void Set<T>(string key, T @object)
        {
            if (!this.IsEnabled) return;

            try
            {
                _memoryCache.Set(key, @object);
            }
            catch (Exception ex)
            {
                throw new MemoryCacheWrapperException(ex.Message);
            }
        }

        public void Remove(string key)
        {
            if (!this.IsEnabled) return;

            try
            {
                _memoryCache.Remove(key);
            }
            catch (Exception ex)
            {
                throw new MemoryCacheWrapperException(ex.Message);
            }
        }

        #endregion

        #region Disposing

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    _memoryCache?.Dispose();
                }
                catch (Exception ex) when (ex is NullReferenceException || ex is ObjectDisposedException)
                {
                    /* Ignored. (When requested resource has already been disposed). */
                }
            }
        }

        #endregion
    }
}