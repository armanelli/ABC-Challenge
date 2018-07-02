using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotoesScript : MonoBehaviour
{
    public GameController _gameController;

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
        _gameController.Responder(Letra);
    }
}
