using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforcementsBehavior : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    
   

   
    private void Update()
    {
       if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

   
}
