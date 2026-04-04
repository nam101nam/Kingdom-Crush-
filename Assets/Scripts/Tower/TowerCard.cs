using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerCard : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Image towerImage;
    
    [SerializeField] private TMP_Text costText;
    public static event Action<TowerData> OnTowerSelected;
    private TowerData _towerData;
    public void Initialize(TowerData data){
        _towerData = data;
        towerImage.sprite=data.sprite;
        costText.text=data.cost.ToString();
    }
    public void PlaceTower(){
        
        OnTowerSelected?.Invoke(_towerData);
    }
}
