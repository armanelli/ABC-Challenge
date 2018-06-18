using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotoesScript : MonoBehaviour
{
    public GameScript _gameScript;

    public char Letra;

    void Start()
    {
        Letra = Convert.ToChar(this.gameObject.name);
    }

    void Update()
    {

    }

    void OnMouseDown()
    {
        _gameScript.OuvirLetra(Letra);

    }
}
