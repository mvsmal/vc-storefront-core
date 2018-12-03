namespace VirtoCommerce.Storefront.Model.Modules
{
    public class Module
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string PlatformVersion { get; set; }
        public string Title { get; set; }
        public bool IsRemovable { get; set; }
        public bool IsInstalled { get; set; }
    }
}
