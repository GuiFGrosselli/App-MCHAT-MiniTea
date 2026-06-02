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

    [Header("Painel de Explicação Dinâmico")]
    public GameObject painelExplicacao;    // Objeto do Painel de instruções
    public TextMeshProUGUI txtExplicacao;   // Componente de texto que exibirá as instruções

    [Header("Dados do Quiz")]
    public Questionarios questionarioAtual; // Questionário carregado

    private List<Perguntas> listaPerguntas;
    private Perguntas perguntaAtual;
    private MecanicasQuiz mecanicas;

    void Start()
    {
        mecanicas = GetComponent<MecanicasQuiz>();

        // Se viemos do menu e um questionário foi escolhido, usamos ele
        if (MenuSelecao.QuestionarioEscolhido != null)
        {
            questionarioAtual = MenuSelecao.QuestionarioEscolhido;
        }

        // Configura e mostra a explicação personalizada antes do jogo começar
        MostrarPainelExplicacao();
    }

    void MostrarPainelExplicacao()
    {
        if (painelExplicacao != null)
        {
            painelExplicacao.SetActive(true);
            
            // Alimenta o componente de texto com o que foi digitado no ScriptableObject do questionário atual
            if (txtExplicacao != null && !string.IsNullOrEmpty(questionarioAtual.introducaoDoQuiz))
            {
                txtExplicacao.text = questionarioAtual.introducaoDoQuiz;
            }
            else if (txtExplicacao != null)
            {
                // Texto de segurança caso você esqueça de preencher o ScriptableObject
                txtExplicacao.text = "Instruções:\n\nSelecione a resposta correta para avançar.";
            }
        }
        else
        {
            // Se você não colocou um painel na cena, o jogo inicia direto para não travar
            IniciarQuiz();
        }
    }

    // Este método DEVE ser associado ao evento On Click() do botão "Começar" do painel
    public void FecharExplicacaoEDesenrolarQuiz()
    {
        if (painelExplicacao != null)
        {
            painelExplicacao.SetActive(false); // Esconde o painel de instruções
        }
        
        IniciarQuiz(); // Inicia o temporizador e carrega a primeira pergunta
    }

    public void IniciarQuiz()
    {
        // Cria a cópia das perguntas para jogar
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

        // Sorteia uma pergunta aleatória da lista
        int indice = Random.Range(0, listaPerguntas.Count);
        perguntaAtual = listaPerguntas[indice];
        listaPerguntas.RemoveAt(indice);

        MostrarPergunta();
        mecanicas.ReiniciarTimer();
    }

    void MostrarPergunta()
    {
        // Atualiza o enunciado
        txtPergunta.text = perguntaAtual.enunciado;

        // Lógica da Imagem da pergunta
        if (perguntaAtual.imagemPergunta != null)
        {
            imgPergunta.sprite = perguntaAtual.imagemPergunta;
            imgPergunta.gameObject.SetActive(true);
        }
        else
        {
            imgPergunta.gameObject.SetActive(false);
        }

        // Configura exatamente 2 botões na tela, sem embaralhar
        for (int i = 0; i < btnAlternativas.Length; i++)
        {
            // Ativa apenas os dois primeiros botões se houver alternativas suficientes
            if (i < 2 && i < perguntaAtual.alternativas.Length)
            {
                btnAlternativas[i].gameObject.SetActive(true);
                
                TextMeshProUGUI textoBotao = btnAlternativas[i].GetComponentInChildren<TextMeshProUGUI>();
                textoBotao.text = perguntaAtual.alternativas[i];

                // Compara se o índice atual (0 ou 1) é a resposta correta configurada
                bool acertou = (i == perguntaAtual.respostaCorreta);

                btnAlternativas[i].onClick.RemoveAllListeners();
                btnAlternativas[i].onClick.AddListener(() => mecanicas.Responder(acertou));
            }
            else
            {
                // Desativa botões extras (3º, 4º...) que estejam na tela
                btnAlternativas[i].gameObject.SetActive(false);
            }
        }
    }
}