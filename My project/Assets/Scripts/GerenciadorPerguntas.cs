using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GerenciadorPerguntas : MonoBehaviour
{
    [Header("Referências da UI")]
    public TextMeshProUGUI txtPergunta;    // Texto da pergunta na tela
    public Image imgPergunta;              // Imagem da pergunta na tela
    public Button[] btnAlternativas;       // Array com os 4 botões

    [Header("Dados do Quiz")]
    public Questionarios questionarioAtual; // Questionário carregado

    // Lista interna de perguntas (cópia para não alterar o original)
    private List<Perguntas> listaPerguntas;
    // Pergunta sendo exibida agora
    private Perguntas perguntaAtual;

    // Referência para o script de mecânicas
    private MecanicasQuiz mecanicas;

    void Start()
    {
        mecanicas = GetComponent<MecanicasQuiz>();
        IniciarQuiz();
    }

    // Método público para (re)iniciar o quiz
    public void IniciarQuiz()
    {
        // Copia as perguntas do ScriptableObject para a lista
        listaPerguntas = new List<Perguntas>(questionarioAtual.listaPerguntas);
        CarregarPergunta();
    }

    // Carrega a próxima pergunta aleatória
    public void CarregarPergunta()
    {
        // Verifica se acabaram as perguntas
        if (listaPerguntas.Count == 0)
        {
            mecanicas.GameOver();
            return;
        }

        // Sorteia uma pergunta aleatória
        int indice = Random.Range(0, listaPerguntas.Count);
        perguntaAtual = listaPerguntas[indice];
        listaPerguntas.RemoveAt(indice);

        // Mostra a pergunta na tela
        MostrarPergunta();

        // Reinicia o timer
        mecanicas.ReiniciarTimer();
    }

    // Exibe a pergunta, imagem e alternativas na tela
    void MostrarPergunta()
    {
        // Atualiza o texto da pergunta
        txtPergunta.text = perguntaAtual.enunciado;

        // Atualiza a imagem da pergunta
        if (perguntaAtual.imagemPergunta != null)
        {
            imgPergunta.sprite = perguntaAtual.imagemPergunta;
            imgPergunta.gameObject.SetActive(true);
        }
        else
        {
            imgPergunta.gameObject.SetActive(false);
        }

        // Cria ordem embaralhada das alternativas
        int[] ordem = CriarOrdemEmbaralhada();

        // Configura cada botão
        for (int i = 0; i < btnAlternativas.Length; i++)
        {
            int indiceAlt = ordem[i];
            TextMeshProUGUI textoBotao = btnAlternativas[i].GetComponentInChildren<TextMeshProUGUI>();
            textoBotao.text = perguntaAtual.alternativas[indiceAlt];

            bool acertou = (indiceAlt == perguntaAtual.respostaCorreta);

            btnAlternativas[i].onClick.RemoveAllListeners();
            btnAlternativas[i].onClick.AddListener(() => mecanicas.Responder(acertou));
        }
    }

    // Embaralha os índices 0,1,2,3 (algoritmo Fisher-Yates)
    int[] CriarOrdemEmbaralhada()
    {
        int[] indices = { 0, 1, 2, 3 };
        for (int i = 0; i < indices.Length; i++)
        {
            int sorteado = Random.Range(i, indices.Length);
            int temp = indices[i];
            indices[i] = indices[sorteado];
            indices[sorteado] = temp;
        }
        return indices;
    }
}