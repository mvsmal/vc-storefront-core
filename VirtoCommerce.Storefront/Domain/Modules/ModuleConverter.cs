using VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi.Models;
using VirtoCommerce.Storefront.Model.Modules;

namespace VirtoCommerce.Storefront.Domain.Modules
{
    public static class ModuleConverter
    {
        public static Module ToModule(this ModuleDescriptor descriptor)
        {
            var result = new Module
            {
                Id = descriptor.Id,
                Title = descriptor.Title,
                Version = descriptor.Version,
                PlatformVersion = descriptor.PlatformVersion,
                IsInstalled = descriptor.IsInstalled ?? false,
                IsRemovable = descriptor.IsRemovable ?? false
            };

            return result;
        }
    }
}
