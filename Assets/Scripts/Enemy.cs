using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
   
    void Start()
    {
        
    }

   
    void Update()
    {
     //move down at 4 meters per second
    transform.Translate(Vector3.down * _speed * Time.deltaTime);

    //if bottom of screen
    //respawn at top with a new random x position

    if (transform.position.y < -5f)
     {
     float randomX = Random.Range(-8f, 8f);
     transform.position = new Vector3(randomX, 7, 0);
     }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //damage player
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }
      
       if (other.tag == "Laser")
       {
        Destroy(other.gameObject);
        Destroy(this.gameObject);
       }
    }
}
