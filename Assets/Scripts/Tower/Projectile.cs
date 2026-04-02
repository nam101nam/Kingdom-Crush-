using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private Vector3 _shootDirection;
    private TowerData _data;
    private float _projectileDuration;
    void Start()
    {
        transform.localScale = Vector3.one * _data.projectileSize;   
    }
    // Update is cassslled once per frame
    void Update()
    {
        if(_projectileDuration<=0){
            gameObject.SetActive(false);
        }
        else{
            transform.position +=  new Vector3(_shootDirection.x,_shootDirection.y,0)*_data.projectileSpeed*Time.deltaTime;
            _projectileDuration -= Time.deltaTime;
        } 
    }
    public void Shoot(TowerData data,Vector3 shootDirection)
    {
        _data = data;
        _shootDirection = shootDirection;
        _projectileDuration = data.projectileDuration;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Enemy")){
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(_data.damage);
            gameObject.SetActive(false);
        }
    }
}
