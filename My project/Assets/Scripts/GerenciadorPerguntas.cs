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

    [Header("Dados do Quiz")]
    public Questionarios questionarioAtual; 

    private List<Perguntas> listaPerguntasFase1;
    private List<Perguntas> perguntasParaSeguimento = new List<Perguntas>(); 
    private Perguntas perguntaAtual;
    private MecanicasQuiz mecanicas;

    private bool estaNaFaseSeguimento = false;

    void Start()
    {
        mecanicas = GetComponent<MecanicasQuiz>();

        if (MenuSelecao.QuestionarioEscolhido != null)
        {
            questionarioAtual = MenuSelecao.QuestionarioEscolhido;
        }

        MostrarPainelExplicacao();
    }

    void MostrarPainelExplicacao()
    {
        if (painelExplicacao != null)
        {
            painelExplicacao.SetActive(true);
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
        CarregarPergunta();
    }

    public void CarregarPergunta()
    {
        if (!estaNaFaseSeguimento)
        {
            if (listaPerguntasFase1.Count == 0)
            {
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
                mecanicas.GameOver();
                return;
            }

            perguntaAtual = perguntasParaSeguimento[0];
            perguntasParaSeguimento.RemoveAt(0);
        }

        MostrarPergunta();
        mecanicas.ReiniciarTimer();
    }

    public void ComecarFaseSeguimentoBotao()
    {
        mecanicas.painelFim.SetActive(false); 
        estaNaFaseSeguimento = true;
        
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