using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperLaser : MonoBehaviour
   
{
    [SerializeField]
    private GameObject _hyperLaser;
    
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

}

