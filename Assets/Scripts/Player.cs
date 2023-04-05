using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    private float _boostAmount = 0f; //100 = full boost //each boost collected = 25
    private const float _boostPerSecond = 25f;
    private float _boostPickupAmount = 25f;
    private float _boostToAdd = 0;
    //private float _refuelSpeed = 20f;
    
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    /*[SerializeField]
    private float _playerHealth = 100f;*/

    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    
    private bool _isShieldActive = false;

    private bool _isThrusterActive = true;

    private bool _isSpeedBoostVisualizerActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;
    private int _shieldLives = 3;

    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private GameObject _speedBoostVisualizer;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSoundClip;

    private AudioSource _audioSource;
    
    //Speed boost on Key press
    //Return to normal speed when key released
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>(); //Find the Object. Get the Component

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        
        _audioSource = GetComponent<AudioSource>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on the Player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }



    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);


        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }


        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

        //SpeedBoost

        //when the speedpowerup collected store in boost gaurge
        //boost amount increases by boostPerSecond
        //when player uses boost decrease by boostPerSecond

        //if boostToAdd > 0 then check against boost left to add
        //implement to speed boost gauge
        if (_boostToAdd > 0)
        {
            float boostAdded = _boostPerSecond * Time.deltaTime;
            boostAdded = Mathf.Clamp(boostAdded, 0, _boostToAdd);
            _boostToAdd -= boostAdded;
            _boostAmount = Mathf.Clamp(boostAdded + _boostAmount, 0, 100);
        }

        
        if (Input.GetKey(KeyCode.LeftShift) && _boostAmount > 0)
        {
            _boostAmount = Mathf.Clamp(_boostAmount - _boostPerSecond * Time.deltaTime, 0, 100);

            transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);

            _isThrusterActive = false;
            _isSpeedBoostVisualizerActive = true;
            _speedBoostVisualizer.SetActive(true);
            _thruster.SetActive(false);
            

            
            

            
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);

            _isSpeedBoostVisualizerActive = false;

            _isThrusterActive = true;
            _speedBoostVisualizer.SetActive(false);
            _thruster.SetActive(true);
            

        }
        _uiManager.SpeedBoostGauge(_boostAmount);


    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }

        _audioSource.Play();

        //play the laser audio clip
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _shieldLives--;

            if(_shieldLives == 2)
            {
                _shieldVisualizer.transform.localScale = new Vector3(4, 4, 4);
                _shieldVisualizer.GetComponent<SpriteRenderer>().material.color = Color.yellow;
            }
            if(_shieldLives == 1)
            {
                _shieldVisualizer.transform.localScale = new Vector3(3, 3, 3);
                _shieldVisualizer.GetComponent<SpriteRenderer>().material.color = Color.red;
            }
            if (_shieldLives == 0)
            {
                _isShieldActive = false;
                //disable shield visualizer
                _shieldVisualizer.SetActive(false);
                
            }
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }
       

       _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }


    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

       
    public void AddSpeedBoost()
    {
        _boostToAdd = Mathf.Clamp(_boostAmount + _boostPickupAmount, 0, 100);
        
    }
    
    public void ShieldActive()
    {
        _isShieldActive = true;
        //enable shield visualizer
        _shieldVisualizer.SetActive(true);
        _shieldLives = 3;
        _shieldVisualizer.transform.localScale = new Vector3(6, 6, 6);
        _shieldVisualizer.GetComponent<SpriteRenderer>().material.color = Color.blue;
    }

    public void HealthRestore()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
        }
        if (_lives == 2)
        {
            _leftEngine.SetActive(false);
        }
        else if (_lives == 3)
        {
            _rightEngine.SetActive(false);
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

       
    }

   
    


    
}

