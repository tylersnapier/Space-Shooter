using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 3.5f;
    [SerializeField]
    public GameObject _laserPrefab;
  
    void Start()
    {
       transform.position = new Vector3(0, 0, 0);  
    }

    
    void Update()
    {  

        CalculateMovement();

        //if i hit the space key
        //spawn gameObject
    
        if (Input.GetKeyDown("space"))
        {
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
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
}
