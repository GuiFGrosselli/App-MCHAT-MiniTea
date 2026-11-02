using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GerenciadorPerguntas : MonoBehaviour
{
    [Header("Referências da UI")]
    public TextMeshProUGUI txtPergunta;    
    public Image imgPergunta;              
    public Button[] btnAlternativas;       

    [Header("Painel de Explicação Dinâmico")]
    public GameObject painelExplicacao;    
    public TextMeshProUGUI txtExplicacao;   

    [Header("Barra de Progresso")]
    public Slider barraProgresso;           // Slider com Min 0, Max 1
    public TextMeshProUGUI txtProgresso;    // "Pergunta 4 de 20"
    public float velocidadeBarra = 4f;      // Maior = barra enche mais rápido
    public GameObject grupoProgresso;       // Objeto que contém a barra e o texto (liga/desliga)

    [Header("Dados do Quiz")]
    public Questionarios questionarioAtual;

    private int totalDaFase = 0;
    private int numeroDaPergunta = 0;
    private float valorAlvoBarra = 0f;

    private List<Perguntas> listaPerguntasFase1;
    private List<Perguntas> perguntasParaSeguimento = new List<Perguntas>(); 
    private Perguntas perguntaAtual;
    private MecanicasQuiz mecanicas;

    private bool estaNaFaseSeguimento = false;

    void Start()
    {
        mecanicas = GetComponent<MecanicasQuiz>();

        if (txtProgresso == null)
            Debug.LogWarning("[Progresso] 'Txt Progresso' está vazio no Inspector do GerenciadorPerguntas.");
        if (barraProgresso == null)
            Debug.LogWarning("[Progresso] 'Barra Progresso' está vazio no Inspector do GerenciadorPerguntas.");

        if (MenuSelecao.QuestionarioEscolhido != null)
        {
            questionarioAtual = MenuSelecao.QuestionarioEscolhido;
        }

        MostrarPainelExplicacao();
    }

    void Update()
    {
        // Anima a barra até o valor alvo, de forma suave
        if (barraProgresso != null && !Mathf.Approximately(barraProgresso.value, valorAlvoBarra))
        {
            barraProgresso.value = Mathf.MoveTowards(
                barraProgresso.value, valorAlvoBarra, velocidadeBarra * Time.deltaTime);
        }
    }

    void AtualizarProgresso()
    {
        if (totalDaFase <= 0) return;

        valorAlvoBarra = Mathf.Clamp01((float)numeroDaPergunta / totalDaFase);

        if (txtProgresso != null)
        {
            txtProgresso.gameObject.SetActive(true);
            txtProgresso.enabled = true;
            txtProgresso.text = "Pergunta " + numeroDaPergunta + " de " + totalDaFase;
        }

        if (barraProgresso != null) barraProgresso.gameObject.SetActive(true);
    }

    // Mostra a barra só durante as perguntas (esconde na explicação e nas telas finais)
    void MostrarProgresso(bool visivel)
    {
        if (grupoProgresso != null)
        {
            grupoProgresso.SetActive(visivel);
            return;
        }

        // Sem grupo configurado: liga/desliga a barra e o texto separadamente
        if (barraProgresso != null) barraProgresso.gameObject.SetActive(visivel);
        if (txtProgresso != null) txtProgresso.gameObject.SetActive(visivel);
    }

    void ZerarProgresso(int total)
    {
        totalDaFase = total;
        numeroDaPergunta = 0;
        valorAlvoBarra = 0f;
        if (barraProgresso != null) barraProgresso.value = 0f;
    }

    void MostrarPainelExplicacao()
    {
        if (painelExplicacao != null)
        {
            painelExplicacao.SetActive(true);
            MostrarProgresso(false);
            if (txtExplicacao != null && !string.IsNullOrEmpty(questionarioAtual.introducaoDoQuiz))
            {
                txtExplicacao.text = questionarioAtual.introducaoDoQuiz;
            }
        }
        else
        {
            IniciarQuiz();
        }
    }

    public void FecharExplicacaoEDesenrolarQuiz()
    {
        if (painelExplicacao != null) painelExplicacao.SetActive(false);
        IniciarQuiz();
    }

    public void IniciarQuiz()
    {
        estaNaFaseSeguimento = false;
        perguntasParaSeguimento.Clear();
        listaPerguntasFase1 = new List<Perguntas>(questionarioAtual.listaPerguntas);
        ZerarProgresso(listaPerguntasFase1.Count);
        MostrarProgresso(true);
        CarregarPergunta();
    }

    public void CarregarPergunta()
    {
        if (!estaNaFaseSeguimento)
        {
            if (listaPerguntasFase1.Count == 0)
            {
                MostrarProgresso(false);
                mecanicas.GameOver();
                return;
            }

            int indice = Random.Range(0, listaPerguntasFase1.Count);
            perguntaAtual = listaPerguntasFase1[indice];
            listaPerguntasFase1.RemoveAt(indice);
        }
        else
        {
            if (perguntasParaSeguimento.Count == 0)
            {
                MostrarProgresso(false);
                mecanicas.GameOver();
                return;
            }

            perguntaAtual = perguntasParaSeguimento[0];
            perguntasParaSeguimento.RemoveAt(0);
        }

        numeroDaPergunta++;
        AtualizarProgresso();

        MostrarPergunta();
        mecanicas.ReiniciarTimer();
    }

    public void ComecarFaseSeguimentoBotao()
    {
        mecanicas.painelFim.SetActive(false); 
        estaNaFaseSeguimento = true;
        ZerarProgresso(perguntasParaSeguimento.Count);
        MostrarProgresso(true);

        // Avisa o MecanicasQuiz para iniciar a nova contagem limpa
        mecanicas.IniciarNovaContagemSeguimento(); 
        
        CarregarPergunta();
    }

    void MostrarPergunta()
    {
        if (estaNaFaseSeguimento)
        {
            txtPergunta.text = "[PASSA-FALHA] " + perguntaAtual.enunciado;
        }
        else
        {
            txtPergunta.text = perguntaAtual.enunciado;
        }

        if (perguntaAtual.imagemPergunta != null)
        {
            imgPergunta.sprite = perguntaAtual.imagemPergunta;
            imgPergunta.gameObject.SetActive(true);
        }
        else
        {
            imgPergunta.gameObject.SetActive(false);
        }

        for (int i = 0; i < btnAlternativas.Length; i++)
        {
            if (i < 2 && i < perguntaAtual.alternativas.Length)
            {
                btnAlternativas[i].gameObject.SetActive(true);
                TextMeshProUGUI textoBotao = btnAlternativas[i].GetComponentInChildren<TextMeshProUGUI>();
                textoBotao.text = perguntaAtual.alternativas[i];

                int indiceAlternativa = i; 

                btnAlternativas[i].onClick.RemoveAllListeners();
                btnAlternativas[i].onClick.AddListener(() => ComputarResposta(indiceAlternativa));
            }
            else
            {
                btnAlternativas[i].gameObject.SetActive(false);
            }
        }
    }

    void ComputarResposta(int indiceEscolhido)
    {
        bool devePontuar = false;

        if (questionarioAtual.name == "MCHAT")
        {
            if (indiceEscolhido == perguntaAtual.respostaAlerta)
            {
                devePontuar = true;
                
                if (!estaNaFaseSeguimento)
                {
                    perguntasParaSeguimento.Add(perguntaAtual);
                }
            }
        }
        else
        {
            if (indiceEscolhido == perguntaAtual.respostaCorreta)
            {
                devePontuar = true;
            }
        }

        mecanicas.Responder(devePontuar);
    }
}