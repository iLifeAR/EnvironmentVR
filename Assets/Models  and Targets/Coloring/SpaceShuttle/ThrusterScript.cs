using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterScript : MonoBehaviour
{
    Renderer _REN;
    ParticleSystem _PS;
    // Start is called before the first frame update
    void Start()
    {
        _REN =transform.parent.GetComponentInChildren<Renderer>();
        _PS=GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_REN.enabled && !_PS.isPlaying)
        {
            _PS.Play();
        }else if (!_REN.enabled && _PS.isPlaying)
        {
            _PS.Stop();
        }
    }
}
