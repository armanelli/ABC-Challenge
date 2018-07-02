using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespostaController : MonoBehaviour
{
    public GameObject SlotRespostaVermelho;
    public GameObject SlotRespostaLaranja;

    private int QuantidadeLetras = 5;
    private int QuantidadeCriada = 0;
    private GameObject SlotClonado;
    private List<GameObject> Slots;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ParametrizarSlots(string palavra)
    {
        QuantidadeLetras = palavra.Length;
        var posicao = transform.position;

        for (int i = 0; i < QuantidadeLetras; i++)
        {
            posicao = posicao + new Vector3(1, 0);

            Instantiate(SlotRespostaLaranja, posicao, Quaternion.identity);
        }
    }
}
