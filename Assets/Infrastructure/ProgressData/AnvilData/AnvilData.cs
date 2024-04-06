using System;

namespace Infrastructure.ProgressData.AnvilData
{
    [Serializable]
    public class AnvilData : IProgressData
    {
        public int MaxCharges;
        public int ChargesLeft;
        public int CraftingItemLevel;
    }
}