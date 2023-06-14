using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
    
{
    [SerializeField]
    private float _shakeDuration;
    private float _xShake;
    private float _yShake;
   
    void Start()
    {
        transform.position = new Vector3(0, 1, -10);
    }

  
    void Update()
    {
        
    }

    public void CameraShaking()
    {
        StartCoroutine(CameraShakeTime());
    }
    IEnumerator CameraShakeTime()
    {
        Vector3 _originalPos = transform.position;
        _shakeDuration = Time.time + 0.2f;

        while (_shakeDuration > Time.time)
        {
            _xShake = Random.Range(-0.1f, 0.1f);
            _yShake = Random.Range(-0.9f, 1.1f);
            transform.position = new Vector3(_xShake, _yShake, transform.position.z);
            yield return new WaitForEndOfFrame();
        }
        transform.position = _originalPos;
    }

}
