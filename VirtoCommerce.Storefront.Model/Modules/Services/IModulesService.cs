using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Modules.Services
{
    public interface IModulesService
    {
        /// <summary>
        /// Gets module by id. Returns null if module not found
        /// </summary>
        /// <param name="id">Id of the module</param>
        /// <returns>Module if it's available, otherwise - null</returns>
        Task<Module> GetModuleAsync(string id);
    }
}
