using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Configurations.Config;
using Infrastructure.Extensions.DataExtensions;
using Infrastructure.ProgressData;
using Infrastructure.ProgressData.Hero;
using Infrastructure.Services.ConfigLoad;
using Infrastructure.Services.ProgressLoad.Connection;
using Sources.WalletBase;
using Zenject;

namespace Sources.HeroBase
{
    [Serializable]
    public class HeroesInventory : IProgressWriter, IConfigReader
    {
        private readonly IWallet _wallet;
        private readonly List<HeroData> _heroes = new List<HeroData>();
        private readonly List<HeroData> _availableHeroes = new List<HeroData>();

        public event Action ConfigLoaded;

        [Inject]
        public HeroesInventory(IWallet wallet)
        {
            _wallet = wallet;
        }

        public void LoadProgress(GameProgress progress)
        {
            LoadPurchasedHeroes(progress);
        }

        public void SaveProgress(GameProgress progress)
        {
            SavePurchasedHeroes(progress);
        }

        public void LoadConfiguration(ConfigContent configContainer)
        {
            _heroes.Clear();
            _heroes.AddRange(configContainer.Heroes);

            _availableHeroes.AddRange(_heroes.Where(hero => hero.IsInitial));

            ConfigLoaded?.Invoke();
        }

        public void PurchaseHeroByID(int heroID)
        {
            HeroData hero = _heroes.GetHeroByID(heroID);

            bool doesHeroExist = hero != null;
            bool isAlreadyAvailable = _availableHeroes.GetHeroByID(heroID) != null;

            if (!isAlreadyAvailable && doesHeroExist)
            {
                _wallet.TakeMoney(hero.Price, out bool isSucceeded);

                if (isSucceeded)
                    _availableHeroes.Add(hero);
            }
        }

        private void SavePurchasedHeroes(GameProgress progress)
        {
            int[] ids = new int[_availableHeroes.Count];
            for (int i = 0; i < _availableHeroes.Count; i++)
            {
                ids[i] = _availableHeroes[i].ID;
            }

            progress.HeroesData.PurchasedHeroesIDs = ids;
        }

        private void LoadPurchasedHeroes(GameProgress progress)
        {
            var purchasedHeroes = _heroes.Where(hero => progress.HeroesData.PurchasedHeroesIDs.Contains(hero.ID) || hero.IsInitial);
            _availableHeroes.AddRange(purchasedHeroes);
        }

        public bool IsHeroUnlocked(int heroID) =>
            _availableHeroes.FirstOrDefault(hero => hero.ID == heroID) != null;

        public HeroData[] Heroes => _heroes.ToArray();
    }
}