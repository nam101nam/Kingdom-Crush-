using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event Action<int> OnLivesChanged; 
    public static event Action<int> OnResourcesChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int _lives=5;
    private int _resources=400;
    public int Resources => _resources;
    private float _gameSpeed=1f;
    public float GameSpeed => _gameSpeed;


    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else if (Instance != this){
            Destroy(gameObject);
        }
    }
    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
        Enemy.OnEnemyReachedEnd += HandleEnemyReachedEnd;
        Enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
        
    }

    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Enemy.OnEnemyReachedEnd -= HandleEnemyReachedEnd;
        Enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
    }

    private void Start(){
        OnLivesChanged?.Invoke(_lives);
        OnResourcesChanged?.Invoke(_resources);
    }

    private void HandleEnemyReachedEnd(EnemyData data){
        _lives =Mathf.Max(0,_lives-data.damage);
        OnLivesChanged?.Invoke(_lives);
    }
    private void HandleEnemyDestroyed(Enemy enemy){
        AddResources(Mathf.RoundToInt(enemy.Data.resourceReward));
    }
    private void AddResources(int amount){
        _resources+=amount;
        OnResourcesChanged?.Invoke(_resources);
    }        
    public void SetTimeScale(float scale){
        Time.timeScale=scale;
    }
    public void SetGameSpeed(float newSpeed){
        _gameSpeed=newSpeed;
        SetTimeScale(_gameSpeed);
    }
    public void SpendResources(int amount)
    {
        if (_resources >= amount)
        {
            _resources -= amount;
            OnResourcesChanged?.Invoke(_resources);
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    // Reset dữ liệu mỗi khi scene được load lại
    _lives = 5;
    _resources = 400;
    OnLivesChanged?.Invoke(_lives);
    OnResourcesChanged?.Invoke(_resources);
    }   
}

