using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongratulationsScript : MonoBehaviour
{

    public AudioClip SomSucesso;

    private AudioSource _audioSource;

    // Use this for initialization
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.PlayOneShot(SomSucesso);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
