using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GerenciadorPerguntas : MonoBehaviour
{
    [Header("Referências da UI")]
    public TextMeshProUGUI txtPergunta;    // Texto da pergunta na tela
    public Image imgPergunta;              // Imagem da pergunta na tela
    public Button[] btnAlternativas;       // Array com os botões (o script usará os 2 primeiros)

    [Header("Dados do Quiz")]
    public Questionarios questionarioAtual; // Questionário carregado

    private List<Perguntas> listaPerguntas;
    private Perguntas perguntaAtual;
    private MecanicasQuiz mecanicas;

    void Start()
    {
        mecanicas = GetComponent<MecanicasQuiz>();
        IniciarQuiz();
    }

    public void IniciarQuiz()
    {
        // Copia as perguntas para a lista interna
        listaPerguntas = new List<Perguntas>(questionarioAtual.listaPerguntas);
        CarregarPergunta();
    }

    public void CarregarPergunta()
    {
        if (listaPerguntas.Count == 0)
        {
            mecanicas.GameOver();
            return;
        }

        // Sorteia uma pergunta aleatória
        int indice = Random.Range(0, listaPerguntas.Count);
        perguntaAtual = listaPerguntas[indice];
        listaPerguntas.RemoveAt(indice);

        MostrarPergunta();
        mecanicas.ReiniciarTimer();
    }

    void MostrarPergunta()
    {
        // 1. Atualiza o enunciado
        txtPergunta.text = perguntaAtual.enunciado;

        // 2. Lógica da Imagem
        if (perguntaAtual.imagemPergunta != null)
        {
            imgPergunta.sprite = perguntaAtual.imagemPergunta;
            imgPergunta.gameObject.SetActive(true);
        }
        else
        {
            imgPergunta.gameObject.SetActive(false);
        }

        // 3. Configura exatamente 2 botões, sem embaralhar
        for (int i = 0; i < btnAlternativas.Length; i++)
        {
            // Ativa apenas os dois primeiros botões e se houver alternativas para eles
            if (i < 2 && i < perguntaAtual.alternativas.Length)
            {
                btnAlternativas[i].gameObject.SetActive(true);
                
                TextMeshProUGUI textoBotao = btnAlternativas[i].GetComponentInChildren<TextMeshProUGUI>();
                textoBotao.text = perguntaAtual.alternativas[i];

                // A resposta correta será comparada diretamente com o índice 0 ou 1
                bool acertou = (i == perguntaAtual.respostaCorreta);

                btnAlternativas[i].onClick.RemoveAllListeners();
                btnAlternativas[i].onClick.AddListener(() => mecanicas.Responder(acertou));
            }
            else
            {
                // Desativa botões extras (3º, 4º...)
                btnAlternativas[i].gameObject.SetActive(false);
            }
        }
    }
}