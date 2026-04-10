using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.XR;
public class UIController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text resourcesText;
    [SerializeField] private GameObject towerPanel;
    [SerializeField] private TowerCard towerCardPrefab;
    [SerializeField] private Transform towerCardContainer;
    [SerializeField] private TowerData[] towers;
    [SerializeField] private GameObject notResourceText;
    private Platform _currentPlatform;
    private List<GameObject> activeCards=new List<GameObject>();
    private void OnEnable() {
        Spawner.OnWaveChanged += UpdateWaveText;
        GameManager.OnLivesChanged += UpdateLivesText;
        GameManager.OnResourcesChanged += UpdateResourcesText;  
        Platform.OnPlatformClicked += HandlePlatformClicked;
        TowerCard.OnTowerSelected += HandleTowerSelected;
    }
    private void OnDisable() {
        Spawner.OnWaveChanged -= UpdateWaveText;
        GameManager.OnLivesChanged -= UpdateLivesText;
        GameManager.OnResourcesChanged -= UpdateResourcesText;
        Platform.OnPlatformClicked -= HandlePlatformClicked;
        TowerCard.OnTowerSelected -= HandleTowerSelected;
    }

    private void Start() {
        if (resourcesText != null) {
            resourcesText.textWrappingMode = TextWrappingModes.NoWrap;
            resourcesText.overflowMode = TextOverflowModes.Overflow;
            resourcesText.text = "Resources: 0";
        }
    }
    private void HandlePlatformClicked(Platform platform){
        _currentPlatform=platform;
        ShowTowerPanel();
    }

    private void UpdateWaveText(int currentWave){
        waveText.text="Wave: "+(currentWave+1);
    }
    private void UpdateLivesText(int currentLives){
        livesText.text="Lives: "+currentLives;
    }
    private void UpdateResourcesText(int currentResources){
        resourcesText.text="Resources: "+currentResources;
    }
    private void ShowTowerPanel(){
        Platform.towerPanelOpen=true;
        towerPanel.SetActive(true);
        GameManager.Instance.SetTimeScale(0f);
        PopulateTowerCards();
    }
    public void HideTowerPanel(){
        Platform.towerPanelOpen = false;

        towerPanel.SetActive(false);        
        GameManager.Instance.SetTimeScale(1f);
    } 
    private void PopulateTowerCards(){
        foreach(var card in activeCards){
            Destroy(card);
        }
        activeCards.Clear();
        foreach(var data in towers){
            GameObject cardGameObject=Instantiate(towerCardPrefab.gameObject,towerCardContainer);
            TowerCard card=cardGameObject.GetComponent<TowerCard>();
            card.Initialize(data);
            activeCards.Add(cardGameObject);
        }
        
    } 
    private void HandleTowerSelected(TowerData towerData){
        if (GameManager.Instance.Resources >= towerData.cost)
        {
            GameManager.Instance.SpendResources(towerData.cost);
            _currentPlatform.PlaceTower(towerData);
        }
        else{
            StartCoroutine(ShowNoResourcesMessage());
        }

        HideTowerPanel();
    }
    private IEnumerator ShowNoResourcesMessage(){
        notResourceText.SetActive(true);
        yield return new WaitForSeconds(2f);
        notResourceText.SetActive(false);
    }
}
