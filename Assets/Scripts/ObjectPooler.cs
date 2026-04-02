using UnityEngine;
using System.Collections.Generic;
public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _poolSize=10;
    private List<GameObject> _pool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pool =new List<GameObject>();
        for(int i=0;i<_poolSize;i++){
            CreateNewObject();
        }
    }
    private GameObject CreateNewObject(){
        GameObject obj=GameObject.Instantiate(_prefab,transform);
        obj.SetActive(false);
        _pool.Add(obj);
        return obj;
    }
    public GameObject GetPooledObject(){
        foreach(GameObject obj in _pool){
               if(!obj.activeSelf){
                return obj;
               }
        }
        return CreateNewObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
