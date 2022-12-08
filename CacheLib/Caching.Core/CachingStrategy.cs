using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Medrio.Caching.Abstraction
{
    //This class is not in use for now and excluded from the project
    public class CachingStrategy
    {
        private List<CachingTier> _tiers; //Using List not IList because AsReadOnly<T>(IList<T>) only available in .NET 7 +

        public ReadOnlyCollection<CachingTier> Tiers =>
            this._tiers?.AsReadOnly() ?? new ReadOnlyCollection<CachingTier>(new List<CachingTier>());

        public CachingStrategy(ICollection<CachingTier> tiers)
        {
            _tiers = tiers?.ToList() ?? throw new ArgumentNullException(nameof(tiers));  
            SortAndVerifyTiers();
        }


        private void SortAndVerifyTiers()
        {
            int c = _tiers?.Count ?? 0;

            if (c == 0) throw new CachingSettingException("CachingStrategy must contains at least 1 caching tier.");
            if (c == 1) return;

            _tiers = _tiers!.OrderBy(tier => tier.TierType).ToList();

            for (int i = 1; i < c; i++)
            {
                if (_tiers[i].TierType == _tiers[i - 1].TierType)
                    throw new CachingSettingException($"CachingStrategy contains duplicate tier of type {_tiers[i].TierType}");
            }
        }
    }
}
