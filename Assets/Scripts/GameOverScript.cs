using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{

    public AudioClip SomGameOver;

    private AudioSource _audioSource;

    // Use this for initialization
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.PlayOneShot(SomGameOver);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (gameObject.tag == "OpcaoJogar")
        {
            SceneManager.LoadScene("Principal");
        }
    }
}
