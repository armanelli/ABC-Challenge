using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public AudioClip[] Sons; 

    private AudioSource _audioSource;

    private string _alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private string PalavraSelecionada;
    private List<string> Palavras;    
    private List<string> PalavrasUtilizadas;    

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        Palavras = ObterPalavras();


    }

    void Update()
    {

    }

    private List<string> ObterPalavras()
    {
        return new List<string>
        {
            "cow",
            "dog",
            "cat",
            "kanguru",
            "giraffe",
            "elephant"
        };
    }

    public void OuvirLetra(char letra)
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        var posicaoLetra = _alfabeto.IndexOf(letra);

        _audioSource.PlayOneShot(Sons[posicaoLetra]);
    }
}
