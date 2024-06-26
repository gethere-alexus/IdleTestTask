using System;
using Infrastructure.Configurations.Config;
using Infrastructure.ProgressData;
using Sources.ItemsBase.ItemFieldBase.Extensions.AutoMerge;
using Sources.Utils;
using Zenject;

namespace Infrastructure.Services.UpgradeRegistry.Upgrades.SlotMergeUpgrade
{
    public class SlotsMergeUpgrade : UpgradeBase
    {
        public const int UpgradeID = 3;
        
        private readonly AutoSlotsMerger _autoMerger;

        private float _stageMergeCdDecrease;
        private int _currentUpgradeStage;
        private int _upgradePrice;
        private UpgradeConfiguration _upgradeConfiguration;
        public override event Action ConfigLoaded;


        [Inject]
        public SlotsMergeUpgrade(AutoSlotsMerger autoMerger)
        {
            _autoMerger = autoMerger;
        }

        public override void Upgrade(out bool isSucceeded)
        {
            isSucceeded = false;
            if (IsUpgradable)
            {
                _autoMerger.DecreaseMergeCoolDown(_stageMergeCdDecrease);
                _currentUpgradeStage++;
                
                int initialPrice = _upgradeConfiguration.InitialPrice;
                float stageMultiplication = _upgradeConfiguration.UpgradePriceMultiplication;
                int startStage = _upgradeConfiguration.StartUpgradeStage;
                
                _upgradePrice = UpgradeUtility.GetStagePrice(initialPrice, stageMultiplication, startStage, _currentUpgradeStage);
                isSucceeded = true;
            }
        }

        public override void LoadUpgradeConfiguration(UpgradeConfiguration upgradeConfiguration) => 
            _upgradeConfiguration = upgradeConfiguration;

        public override void LoadConfiguration(ConfigContent configContainer)
        {
            _stageMergeCdDecrease = configContainer.SlotsMergeUpgrade.StageCooldownDecrease;
            ConfigLoaded?.Invoke();
        }

        public override void LoadProgress(GameProgress progress)
        {
            _currentUpgradeStage = progress.UpgradesData.SlotsMergeUpgradeData.CurrentUpgradeStage;

            int initialPrice = _upgradeConfiguration.InitialPrice; 
            float stageMultiplication = _upgradeConfiguration.UpgradePriceMultiplication;
            int startStage = _upgradeConfiguration.StartUpgradeStage;
            
            _upgradePrice = UpgradeUtility.GetStagePrice(initialPrice, stageMultiplication,startStage, _currentUpgradeStage);
        }

        public override void SaveProgress(GameProgress progress)
        {
            progress.UpgradesData.SlotsMergeUpgradeData.CurrentUpgradeStage = _currentUpgradeStage;
        }

        public override int RequiredUpgradeID => UpgradeID;
        public override UpgradeConfiguration UpgradeConfiguration => _upgradeConfiguration;

        public override int CurrentUpgradeStage => _currentUpgradeStage;
        public override int UpgradePrice => _upgradePrice;

        public override bool IsUpgradable => _currentUpgradeStage < _upgradeConfiguration.MaxUpgradeStage;
        public override bool IsCompletelyUpgraded => _currentUpgradeStage == _upgradeConfiguration.MaxUpgradeStage;
        
    }
}