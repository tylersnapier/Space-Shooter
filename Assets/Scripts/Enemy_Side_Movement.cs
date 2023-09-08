using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Side_Movement : MonoBehaviour
{
    [SerializeField]
    private float _random;
    private float _speed = 4.0f;
    [SerializeField]
    private float _canFireLaser = -1f;
    private float _fireRate = 0.5f;
    [SerializeField]
    private GameObject _laserPrefab;

    void Start()
    {
        transform.position = new Vector3(Random.Range(-8.5f, 8.5f), 8, 0);

        _random = Random.Range(1f, 7f);
    }

    
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        transform.position = new Vector3(Mathf.PingPong(Time.time, _random), transform.position.y, transform.position.z);

        if (transform.position.y < -6f)
        {
            transform.position = new Vector3(Random.Range(-8.5f, 8.5f), 8, 0);
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8.5f, 8.5f), transform.position.y, 0);

       
    }

    void FireLaser()
    {
        if (Time.time > _canFireLaser)
        {
            _fireRate = Random.Range(1.5f, 4f);
            _canFireLaser = Time.time + _fireRate;
        }
        GameObject _enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] _laser = _enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i > _laser.Length; i++)
        {
            _laser[i].AssignEnemyLaser();
        }
    }

}
