using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public AudioClip[] Letras;
    public AudioClip[] Palavras;
    public AudioClip[] Falas;

    public Text TextoRodada;
    public Text TextoErros;

    public GameObject Interrogacao;
    public GameObject[] LetrasRespostas;

    private List<GameObject> Respostas;

    private Dictionary<string, AudioClip> DictionarySons;

    private AudioSource _audioSource;

    private string _alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private UnityEngine.Random _random;

    public bool ModoDesafio = false;

    private List<string> _palavrasUtilizadas;
    private string _palavraSelecionada;
    private int _posicaoAtual = 0;
    private int _posicoesResposta;
    private int _letrasCorretas = 0;
    private int _letrasErradas = 0;
    private int _palavrasCorretas = 0;
    private int _palavrasErradas = 0;
    private int _rodadaAtual = 1;
    private int _qtdeRodadas = 3;

    private bool primeiro = true;

    void Start()
    {
        Configurar();

        StartCoroutine(InstrucoesIniciais());

        IniciarRodada();
    }

    void Update()
    {

    }

    void Configurar()
    {
        _audioSource = GetComponent<AudioSource>();
        Respostas = new List<GameObject>();
        _palavrasUtilizadas = new List<string>();

        DictionarySons = new Dictionary<string, AudioClip>();

        ParametrizarSons();
    }

    void IniciarRodada()
    {
        var gameObjects = FindObjectsOfType<GameObject>();

        for (var i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i].name.Contains("interrogacao") || gameObjects[i].name.Contains("letra_resposta"))
            {
                Destroy(gameObjects[i]);
            }
        }

        do
        {
            _palavraSelecionada = Util.PalavrasFaceis[UnityEngine.Random.Range(0, Util.PalavrasFaceis.Count)].ToLower();

        } while (_palavrasUtilizadas.Contains(_palavraSelecionada));

        _posicoesResposta = _palavraSelecionada.Length;
        _posicaoAtual = 0;
        _letrasCorretas = 0;
        _letrasCorretas = 0;

        ParametrizarSlots(_palavraSelecionada);
        ParametrizarSlotsResposta(_palavraSelecionada);

        StartCoroutine(FalarProximaPalavra());
    }

    void Reiniciar()
    {
        Respostas = new List<GameObject>();
        _palavrasUtilizadas = new List<string>();

        StartCoroutine(InstrucoesIniciais());

        IniciarRodada();
    }

    #region Parâmetros

    void ParametrizarSons()
    {
        DictionarySons.Add("areyoureadytobegin", Falas[0]);
        DictionarySons.Add("congratulations", Falas[1]);
        DictionarySons.Add("letsgetstarted", Falas[2]);
        DictionarySons.Add("thenextwordis", Falas[3]);

        DictionarySons.Add("errou_palavra_1", Falas[4]);
        DictionarySons.Add("acertou_palavra_1", Falas[5]);
        DictionarySons.Add("acertou_palavra_2", Falas[6]);
        DictionarySons.Add("acertou_letra_1", Falas[7]);
        DictionarySons.Add("acertou_letra_2", Falas[8]);
        DictionarySons.Add("errou_letra_1", Falas[9]);
        DictionarySons.Add("errou_letra_2", Falas[10]);

        DictionarySons.Add("ant", Palavras[0]);
        DictionarySons.Add("cat", Palavras[1]);
        DictionarySons.Add("cow", Palavras[2]);
        DictionarySons.Add("dog", Palavras[3]);
        DictionarySons.Add("duck", Palavras[4]);
        DictionarySons.Add("fish", Palavras[5]);
        DictionarySons.Add("frog", Palavras[6]);
        DictionarySons.Add("kangaroo", Palavras[7]);
        DictionarySons.Add("owl", Palavras[8]);
    }

    void ParametrizarSlots(string palavra)
    {
        var qtdeLetras = palavra.Length;

        var posicao = Interrogacao.transform.position;

        var contador = 0;

        for (int i = 0; i < qtdeLetras; i++)
        {
            var prefabInstanciado = (GameObject)Instantiate(Interrogacao, posicao, Quaternion.identity);

            prefabInstanciado.name = "interrogacao_" + contador;

            posicao = posicao + new Vector3(1, 0);
            contador++;
        }
    }

    void ParametrizarSlotsResposta(string palavra)
    {
        Respostas = new List<GameObject>();

        var qtdeLetras = palavra.Length;

        var posicao = Interrogacao.transform.position;

        var contador = 0;

        foreach (var letra in palavra)
        {
            var posicaoLetra = _alfabeto.IndexOf(letra.ToString().ToUpper());

            var letraResposta = LetrasRespostas[posicaoLetra];

            var prefabInstanciado = Instantiate(Resources.Load("LetrasRespostas/" + letra.ToString().ToUpper()) as GameObject, posicao, Quaternion.identity) as GameObject;

            prefabInstanciado.name = "letra_resposta_" + contador;
            prefabInstanciado.SetActive(false);

            Respostas.Add(prefabInstanciado);

            posicao = posicao + new Vector3(1, 0);

            contador++;
        }
    }

    #endregion

    IEnumerator InstrucoesIniciais()
    {
        _audioSource.PlayOneShot(DictionarySons["areyoureadytobegin"]);

        yield return new WaitForSeconds(DictionarySons["areyoureadytobegin"].length);

        _audioSource.PlayOneShot(DictionarySons["letsgetstarted"]);

        yield return new WaitForSeconds(DictionarySons["letsgetstarted"].length);

    }

    IEnumerator FalarProximaPalavra()
    {
        if (primeiro)
        {
            yield return new WaitForSeconds(DictionarySons["areyoureadytobegin"].length + DictionarySons["letsgetstarted"].length);
            primeiro = false;
        }

        _audioSource.PlayOneShot(DictionarySons["thenextwordis"]);

        yield return new WaitForSeconds(DictionarySons["thenextwordis"].length);

        _audioSource.PlayOneShot(DictionarySons[_palavraSelecionada]);

        yield return new WaitForSeconds(DictionarySons[_palavraSelecionada].length);

        if (!ModoDesafio)
            StartCoroutine(FalarLetraAtual());
    }

    public void Responder(char letra)
    {
        var posicaoLetra = _alfabeto.IndexOf(letra);

        if (_palavraSelecionada[_posicaoAtual] == Convert.ToChar(letra.ToString().ToLower()))
        {
            var interrogacao = GameObject.Find("interrogacao_" + _posicaoAtual);
            var letraResposta = Respostas.FirstOrDefault(r => r.name == "letra_resposta_" + _posicaoAtual);

            interrogacao.SetActive(false);
            letraResposta.SetActive(true);

            StartCoroutine(TocarSomAcerto());

            _posicaoAtual++;
            _letrasCorretas++;

            if (_letrasCorretas == _palavraSelecionada.Length)
            {
                _palavrasUtilizadas.Add(_palavraSelecionada);

                _palavrasCorretas++;

                if (_palavrasCorretas == _qtdeRodadas)
                {
                    MostrarMensagemSucesso();
                }
                else
                {
                    TextoRodada.text = (_palavrasCorretas + 1).ToString() + " de 3";
                    IniciarRodada();
                }
            }
            else
            {
                if (!ModoDesafio)
                    StartCoroutine(FalarLetraAtual());
            }
        }
        else
        {
            _letrasErradas++;
            StartCoroutine(TocarSomErro());

            TextoErros.text = _letrasErradas.ToString() + " de 3";

            if(_letrasErradas == 3)
            {
                MostrarMensagemGameOver();
            }
        }
    }

    void MostrarMensagemSucesso()
    {
        SceneManager.LoadScene("Congratulations");
    }

    void MostrarMensagemGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator FalarLetraAtual()
    {
        yield return new WaitForSeconds(1);

        var letra = _palavraSelecionada[_posicaoAtual];

        var posicaoLetra = _alfabeto.IndexOf(letra.ToString().ToUpper());

        _audioSource.PlayOneShot(Letras[posicaoLetra]);

        yield return new WaitForSeconds(Letras[posicaoLetra].length);
    }

    IEnumerator TocarSomAcerto()
    {
        _audioSource.PlayOneShot(DictionarySons["acertou_letra_1"]);

        yield return new WaitForSeconds(DictionarySons["acertou_letra_1"].length);
    }

    IEnumerator TocarSomErro()
    {
        _audioSource.PlayOneShot(DictionarySons["errou_letra_1"]);

        yield return new WaitForSeconds(DictionarySons["errou_letra_1"].length);
    }

    public static class Util
    {
        public static List<string> PalavrasFaceis = new List<string> { "ant", "cat", "dog", "duck", "fish", "frog", "kangaroo", "owl" };

        public static List<string> PalavrasIntermediarias = new List<string> { "scorpion", "elephant", "dolphin", "monkey", "kitten", "eagle", "squirrel", "shark", "snake", "deer" };

        public static List<string> PalavrasDificeis = new List<string> { "kangaroo", "hippopotamus", "alligator", "chimpanzee", "cheetah" };
    }
}
