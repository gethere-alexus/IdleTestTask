﻿using Infrastructure.Configurations;
using Infrastructure.Configurations.Anvil;
using Sources.SlotsHolderBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.AnvilBase
{
    public class AnvilDisplay : MonoBehaviour
    {
        [SerializeField] private Button _anvilButton;
        [SerializeField] private TMP_Text _chargesInformation;

        private Anvil _anvilInstance;

        public void Construct(SlotsHolder slotsHolder, AnvilConfig anvilConfig)
        {
            _anvilInstance = new Anvil(slotsHolder, anvilConfig);
            _anvilButton.onClick.AddListener(_anvilInstance.CraftItem);
            _anvilInstance.AnvilUsed += UpdateView;
            
            UpdateView();
        }

        private void UpdateView() => 
            _chargesInformation.text = $"{_anvilInstance.ChargesLeft}/{_anvilInstance.MaxCharges}";

        private void OnDisable()
        {
            _anvilInstance.AnvilUsed -= UpdateView;
            _anvilButton.onClick.RemoveAllListeners();
        }

        public Anvil AnvilInstance => _anvilInstance;
    }
}