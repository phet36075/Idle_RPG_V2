using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;

using System;
public class UpgradeUI : MonoBehaviour
{
    public PlayerData _PlayerData;
    private CurrencyManager _currencyManager;
    public TextMeshProUGUI UpgradeCostTxT;
    public TextMeshProUGUI WeaponDmg;

    //public int upgradeCost;
    private float level = 1;
    private bool isVisible = false;
    // Start is called before the first frame update
    void Start()
    {
        _currencyManager = FindObjectOfType<CurrencyManager>();
       // _PlayerData.upgradeCost = 100;
    }

    // Update is called once per frame
    void Update()
    {
        UpgradeCostTxT.text = _PlayerData.upgradeCost.ToString();
        WeaponDmg.text = _PlayerData.weaponDamage.ToString();
    }

    public void UpgradeWeapon()
    {
      
        if (_currencyManager.CurrentMoney >= _PlayerData.upgradeCost)
        {
            _PlayerData.level++;
            _currencyManager.SpendMoney(_PlayerData.upgradeCost);
            _PlayerData.weaponDamage += 5;
            _PlayerData.upgradeCost = (int)Math.Round(100 * ( _PlayerData.level * 1.5f));
        }
    
        
        
    }
    public void ToggleUpgradeUI()
    {
        isVisible = !isVisible;
        gameObject.SetActive(isVisible);
    }
}
