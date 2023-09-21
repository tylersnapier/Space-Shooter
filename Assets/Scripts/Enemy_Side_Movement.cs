using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy_Side_Movement : MonoBehaviour
{
    [SerializeField]
    private float _random;
    private float _ping, _pong;
    private int _direction = -1;
    private float _speed = 4.0f;
    [SerializeField]
    private float _canFire = -1f;
    private float _fireRate = 0.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private bool _alive = true;

    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }
        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }
        transform.position = new Vector3(Random.Range(-8.5f, 8.5f), 8, 0);

        _random = Random.Range(1f, 7f);

        _ping = transform.position.x;
        _ping = _ping + _random;

        _pong = transform.position.x;
        _pong = _pong - _random;
    }


    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.x > _ping || transform.position.x >= 8.75f)
        {
            _direction = -1;

        }
        else if (transform.position.x < _pong || transform.position.x <= -8.75f)
        {
            _direction = 1;
        }

        transform.Translate(Vector3.right * _speed * _direction * Time.deltaTime);


        if (Time.time > _canFire && _alive)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();


            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();

            }
        }

    }

    void FireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(1.5f, 4f);
            _canFire = Time.time + _fireRate;
        }
        GameObject _enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] _laser = _enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i > _laser.Length; i++)
        {
            _laser[i].AssignEnemyLaser();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_alive == false)
        {
            return;
        }
        if (other.tag == "Player")
        {
            //damage player
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage(20);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.5f);
            _alive = false;

        }

        else if (other.tag == "Laser")
        {
            if (other.transform.GetComponent<Laser>().IsEnemyLaser() == false)
            {
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScore(10);
                }

                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _audioSource.Play();
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.5f);
                _alive = false;
            }
        }

        else if (other.tag == "HyperLaser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
            _alive = false;

        }
    }
}
