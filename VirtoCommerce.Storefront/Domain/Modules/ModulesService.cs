using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi;
using VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi.Models;
using VirtoCommerce.Storefront.Extensions;
using VirtoCommerce.Storefront.Infrastructure;
using VirtoCommerce.Storefront.Model.Caching;
using VirtoCommerce.Storefront.Model.Common.Caching;
using VirtoCommerce.Storefront.Model.Modules;
using VirtoCommerce.Storefront.Model.Modules.Services;

namespace VirtoCommerce.Storefront.Domain.Modules
{
    public class ModulesService : IModulesService
    {
        private readonly IModules _modulesApi;
        private readonly IStorefrontMemoryCache _memoryCache;
        private readonly IApiChangesWatcher _apiChangesWatcher;

        public ModulesService(
            IModules modulesApi,
            IStorefrontMemoryCache memoryCache,
            IApiChangesWatcher apiChangesWatcher)
        {
            _modulesApi = modulesApi;
            _memoryCache = memoryCache;
            _apiChangesWatcher = apiChangesWatcher;
        }

        /// <inheritdoc />
        public async Task<Module> GetModuleAsync(string id)
        {
            var modules = await GetModulesAsync();
            if (modules.TryGetValue(id, out var module))
            {
                return module.ToModule();
            }

            return null;
        }

        private Task<Dictionary<string, ModuleDescriptor>> GetModulesAsync()
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetModulesAsync));
            return _memoryCache.GetOrCreateExclusiveAsync(cacheKey,
                async cacheEntry =>
                {
                    cacheEntry.AddExpirationToken(ModuleCacheRegion.CreateChangeToken());
                    cacheEntry.AddExpirationToken(_apiChangesWatcher.CreateChangeToken());

                    try
                    {
                        var response = await _modulesApi.GetModulesAsync();
                        return response.Where(m => m.IsInstalled ?? false).ToDictionary(k => k.Id, v => v);
                    }
                    catch
                    {
                        // TODO Log exception
                        return new Dictionary<string, ModuleDescriptor>();
                    }
                });
        }
    }
}
