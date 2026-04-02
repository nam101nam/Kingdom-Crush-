using UnityEngine;
using System.Collections.Generic;
using System;
public class Spawner : MonoBehaviour
{
    [SerializeField] private WaveData[] _waves;
    private int _currentWaveIndex=0;
    private WaveData _currentWave=> _waves[_currentWaveIndex];
    private float _spawnTimer;
    private int _spawnedCounter; 
    private int _enemiesRemoved;
    private int _waveCounter=0;
    public static event Action<int> OnWaveChanged;

    private float _timeBetweenWaves=1f;
    private float _waveCoolDown;
    private bool _isBetweenWaves=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [SerializeField] private ObjectPooler orcPool;
    [SerializeField] private ObjectPooler dragonPool;
    [SerializeField] private ObjectPooler kaijuPool;

    private Dictionary<EnemyType,ObjectPooler> _poolDictionary;
    private void Awake(){
        _poolDictionary=new Dictionary<EnemyType,ObjectPooler>(){
            {EnemyType.Orc,orcPool},
            {EnemyType.Dragon,dragonPool},
            {EnemyType.Kaiju,kaijuPool},
        };
    }
    private void OnEnable(){
        Enemy.OnEnemyReachedEnd+=handleEnemyReachedEnd;
        Enemy.OnEnemyDestroyed+=HandleEmemyDestroyed;
    }
    private void OnDisable(){
        Enemy.OnEnemyReachedEnd-=handleEnemyReachedEnd;
        Enemy.OnEnemyDestroyed-=HandleEmemyDestroyed;
    } 
    private void Start(){
        OnWaveChanged?.Invoke(_waveCounter);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(_isBetweenWaves){
            _waveCoolDown-=Time.deltaTime;
            if(_waveCoolDown<=0f){
                _currentWaveIndex=(_currentWaveIndex+1)%_waves.Length;
                _spawnedCounter=0;
                _waveCounter++;
                OnWaveChanged?.Invoke(_waveCounter);
                _enemiesRemoved=0;
                _spawnTimer=0f;
                _isBetweenWaves=false;
            }
        }
        else{
            _spawnTimer-=Time.deltaTime;
        if(_spawnTimer<=0 && _spawnedCounter<_currentWave.enemyPerWave){
            _spawnTimer=_currentWave.spawnInterval;
            spawnEnemy();
            _spawnedCounter++;
        }
        if(_spawnedCounter>=_currentWave.enemyPerWave && _enemiesRemoved>=_currentWave.enemyPerWave){
            _isBetweenWaves=true;
            _waveCoolDown = _timeBetweenWaves;
            
        }
        }
        
    }
    private void spawnEnemy(){
        if(_poolDictionary.TryGetValue(_currentWave.enemyType,out ObjectPooler pool)){
            GameObject spawnedObject=pool.GetPooledObject();
            spawnedObject.transform.position=transform.position;
            float healthMultiplier=1f+(_waveCounter*0.1f);
            Enemy enemy=spawnedObject.GetComponent<Enemy>();
            enemy.Initialize(healthMultiplier); 
            spawnedObject.SetActive(true);
        }
        
    }
    public void handleEnemyReachedEnd(EnemyData data){
        _enemiesRemoved++;
    }
    private void HandleEmemyDestroyed(Enemy enemy){
        _enemiesRemoved++;
    }
}
