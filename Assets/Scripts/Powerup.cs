using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] //0 = Triple Shot 1 = Speed 2 = Shields 3 = Health
    private int powerupID;
    [SerializeField]
   private AudioClip _clip;
    
   
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }
      
    }

      
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);
            
            if (player != null)
            {

                switch(powerupID)
                {
                    case 0:
                    player.TripleShotActive();
                    break;
                    case 1:
                    player.AddSpeedBoost();
                    break;
                    case 2:
                    player.ShieldActive();
                    break;
                    case 3:
                    player.HealthRestore();
                    break;
                    case 4:
                    player.AmmoRefill();
                    break;
                    case 5:
                    player.HyperLaserActive();
                    Destroy(gameObject);
                    break;
                    default:
                    Debug.Log("Default Value");
                    break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
