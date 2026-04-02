using UnityEngine;
using TMPro;
public class UIController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text resourcesText;
    private void OnEnable() {
        Spawner.OnWaveChanged += UpdateWaveText;
        GameManager.OnLivesChanged += UpdateLivesText;
        GameManager.OnResourcesChanged += UpdateResourcesText;  
    }
    private void OnDisable() {
        Spawner.OnWaveChanged -= UpdateWaveText;
        GameManager.OnLivesChanged -= UpdateLivesText;
        GameManager.OnResourcesChanged -= UpdateResourcesText;
    }

    private void Start() {
        if (resourcesText != null) {
            resourcesText.textWrappingMode = TextWrappingModes.NoWrap;
            resourcesText.overflowMode = TextOverflowModes.Overflow;
            resourcesText.text = "Resources: 0";
        }
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
}
