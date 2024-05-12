using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Flank_Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    private GameObject _horizontalLaser;
    private float _fireRate = 1f;
    private float _canFire = -1f;

    [SerializeField]
    private bool _alive = true;

    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
