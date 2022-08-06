using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 3.5f;
    
    // Start is called before the first frame update
    void Start()
    {
       //take the current position = new position (0, 0, 0)
       transform.position = new Vector3(0, 0, 0);  
    }

    // Update is called once per frame
    void Update()
    {  
        float horizontalInput = Input.GetAxis("Horizontal");

        float verticalInput = Input.GetAxis("Vertical");    
        
        //new Vector3(-5, 0, 0) * 5 * real time
        transform.Translate(Vector3.left * horizontalInput * _speed * Time.deltaTime);

        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        //if player position on the y is greater than 0
        //y position = 0
        //else if position on the y is less than -3.8f
        //y pos = -3.8f


       if (transform.position.y >= 0)
       {
            transform.position = new Vector3(transform.position.x, 0, 0);
       }
        else if(transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
            


        }
        
    }
}
