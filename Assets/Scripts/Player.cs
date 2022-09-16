using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;

    //variable for isTripleShotActive
  
    void Start()
    {
       transform.position = new Vector3(0, 0, 0);  
       _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>(); //Find the Object. Get the Component 

       if (_spawnManager == null)
       {
        Debug.LogError("The Spawn Manager is NULL.");
       }
    }

    
    void Update()
    {  

        CalculateMovement();

       if (Input.GetKeyDown (KeyCode.Space) && Time.time > _canFire)
       {
            FireLaser();
       }
    
       
        
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        float verticalInput = Input.GetAxis("Vertical");    
        
        
        transform.Translate(Vector3.left * horizontalInput * _speed * Time.deltaTime);

        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

       

       if (transform.position.y >= 0)
       {
            transform.position = new Vector3(transform.position.x, 0, 0);
       }
        else if(transform.position.y <= -3.8f)
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

            //if space key press, 
            //if tripleshotActive is true
                //fire 3 lasers (triple shot prefab)
             //else fire 1 laser   

            //instantiate 3 lasers (triple shot prefab)

    }

    public void Damage()
    {
        _lives--;

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

    //IENumerator TripleShotPowerDownRoutine
    //wait 5 seconds
    //set the triple shot to false
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
}

