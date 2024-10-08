using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField]
    private float _speedDebuff = 2;
    //private float _refuelSpeed = 20f;
    [SerializeField]
    private GameObject  _reinforcementsPrefab;
    private bool _isReinforcementsActive = false;
    private GameObject _reinforcementsLaserPrefab;
    
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private bool _ishyperLaserEnabled = false;
    [SerializeField]
    private GameObject _hyperLaserPrefab;
    private Vector3 _hyperLaserOffset = new Vector3(0, 7f, 0);
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    private int _ammo = 15;
    private int _maxAmmo = 15;
    private bool _noAmmo = false;
    /*[SerializeField]
    private int _lives = 3;*/
    [SerializeField]
    private CameraShake _cameraShake;
    [SerializeField]
    private int _maxHealth = 100;
    private int _currentHealth;
    [SerializeField]
    private HealthBar _healthBar;
    [SerializeField]
    private int _damageAmount = 20;

    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    
    private bool _isShieldActive = false;
   [SerializeField]
   private bool _isHyperLaserActive = false;

    [SerializeField]
    private bool _isSpeedDebuffActive = false;

    

    

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
        _uiManager.SetAmmoEnabled();
        
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
        //HealthBar
        _healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();

        if (_healthBar == null)
        {
            Debug.Log("The HealthBar is NULL");
        }
        else
        {
            _healthBar.SetMaxHealth(_maxHealth);
        }

        _currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
    }



    void Update()
        //Fire Laser
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_ammo > 0)
            {
                FireLaser();

                _uiManager.UpdateAmmoDisplay(_ammo, _maxAmmo);
            }
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

           
            
            _speedBoostVisualizer.SetActive(true);
            _thruster.SetActive(false);
            

            
            

            
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);

            

           
            _speedBoostVisualizer.SetActive(false);
            _thruster.SetActive(true);
            

        }
        _uiManager.SpeedBoostGauge(_boostAmount);

        //Speed Decrease Debuff
        //When debuff is collected, cut speed in half
        //When _isSpeedDebuffActive = true, set _speed to 1.75f


    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;



        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        /*else if (_isReinforcementsActive == true)
        {
            Instantiate(_reinforcementsLaserPrefab, transform.position, Quaternion.identity);
        }*/
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
        }

        if (_isHyperLaserActive == true)
        {
            if (GameObject.Find("Hyper_Laser(Clone)") != null)
            {
                return;
            }
            else
            {
                GameObject _hyperLas = Instantiate(_hyperLaserPrefab, transform.position + _hyperLaserOffset, Quaternion.identity);
                _hyperLas.transform.parent = transform;
                Destroy(_hyperLas, 5.0f);
            }
                

        }
         _audioSource.Play();
        _ammo = Mathf.Max(_ammo - 1, 0);

        //play the laser audio clip
    }



    public void Damage(int _damageAmount)
    { 
        _cameraShake.CameraShaking();

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
        _currentHealth -= _damageAmount;

        _healthBar.UpdateHealthBar(_currentHealth, _maxHealth);
        _cameraShake.CameraShaking();

        if (_currentHealth <= 60)
        {
            _rightEngine.SetActive(true);
        }
        else if (_currentHealth <= 20)
        {
            _leftEngine.SetActive(true);
        }
        if (_currentHealth <= 0)

        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            _uiManager.GameOverSequence();
        }
        
       
    }
    public void HealthRestore(int _restoreAmount = 20)
    {
        _currentHealth += _restoreAmount;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        _healthBar.UpdateHealthBar(_currentHealth, _maxHealth);
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

    public void SpeedDecrease()
    {
        _isSpeedDebuffActive = true;
    }

    IEnumerator SpeedDebuffCooldown()
    {
        yield return new WaitForSeconds(10);
        _isSpeedDebuffActive = false;
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
    

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

      
    }

    public void AmmoRefill()
    {
        _ammo = _maxAmmo;

        _uiManager.UpdateAmmoDisplay(_ammo, _maxAmmo);

    }

   /* public void SpawnReinforcements()
    {
        Vector3 _reinforcementsSpawnPosition = new Vector3(transform.position.x, transform.position.y, 0f);
        GameObject reinforcements = Instantiate(_reinforcementsPrefab, _reinforcementsSpawnPosition, Quaternion.identity);
        //Assign the target to the reinforcements
       

        reinforcements.transform.parent = transform;
    } */

    public void CameraShake()
    {
        _cameraShake.CameraShaking();
    }

    public void HyperLaserActive()
    {
        _isHyperLaserActive = true;
        StartCoroutine(HyperLaserPowerupRoutine());
    }

    IEnumerator HyperLaserPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        _isHyperLaserActive = false;
    }






}

