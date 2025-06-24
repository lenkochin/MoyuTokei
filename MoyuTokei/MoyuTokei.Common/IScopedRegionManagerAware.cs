using Prism.Regions;

namespace MoyuTokei.Common
{
    public interface IScopedRegionManagerAware
    {
        IRegionManager ScopedRegionManager { get; set; }
    }
}