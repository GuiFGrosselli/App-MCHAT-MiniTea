using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MecanicasQuiz : MonoBehaviour
{
    [Header("UI – Informações")]
    public TextMeshProUGUI txtPontos;        // Texto da pontuação durante o jogo
    public TextMeshProUGUI txtTimer;         // Texto do timer

    [Header("UI – Game Over")]
    public GameObject painelFim;             // Painel de Game Over
    public TextMeshProUGUI txtPontosFinal;    // Texto da pontuação final
    public Button btnIniciarSeguimento;      // Botão que ativa a Fase 2 (Apenas M-CHAT com 3+ pontos)

    [Header("Configurações do Timer")]
    public float tempoMaximo = 15f;          // Segundos para responder

    // Variáveis privadas de controle
    private int placar = 0;
    private float tempoRestante;
    private bool timerAtivo = false;
    private bool modoSeguimentoAtivo = false; // Controla se estamos na reavaliação do M-CHAT

    private GerenciadorPerguntas gerenciador;

    void Start()
    {
        gerenciador = GetComponent<GerenciadorPerguntas>();
        placar = 0;
        modoSeguimentoAtivo = false; // Começa no jogo normal
        
        // SEGURANÇA 1: Garante que o botão comece totalmente escondido ao iniciar o app
        if (btnIniciarSeguimento != null) 
            btnIniciarSeguimento.gameObject.SetActive(false);

        AtualizarPlacarUI();
    }

    void Update()
    {
        if (timerAtivo)
        {
            tempoRestante -= Time.deltaTime;
            AtualizarTimerUI();

            if (tempoRestante <= 0f)
            {
                tempoRestante = 0f;
                timerAtivo = false;
                TempoEsgotado();
            }
        }
    }

    public void ReiniciarTimer()
    {
        tempoRestante = tempoMaximo;
        timerAtivo = true;
        AtualizarTimerUI();
    }

    void AtualizarTimerUI()
    {
        int segundos = Mathf.CeilToInt(tempoRestante);
        txtTimer.text = "Tempo: " + segundos;

        if (tempoRestante <= 5f)
            txtTimer.color = Color.red;
        else
            txtTimer.color = Color.white;
    }

    void TempoEsgotado()
    {
        gerenciador.CarregarPergunta();
    }

    public void Responder(bool devePontuar)
    {
        timerAtivo = false;

        if (devePontuar)
        {
            placar += 1;
        }
        
        AtualizarPlacarUI();
        gerenciador.CarregarPergunta();
    }

    // Chamado pelo gerenciador quando o usuário clica no botão de iniciar a Fase 2 do M-CHAT
    public void IniciarNovaContagemSeguimento()
    {
        modoSeguimentoAtivo = true;
        placar = 0; // Zera a pontuação para começar a nova contagem do passa/falha
        
        // SEGURANÇA 2: Esconde o botão imediatamente ao clicar para ir para o seguimento
        if (btnIniciarSeguimento != null) 
            btnIniciarSeguimento.gameObject.SetActive(false);

        AtualizarPlacarUI();
    }

    void UpdatePlacar() // Função interna para reset manual se necessário
    {
        placar = 0;
        AtualizarPlacarUI();
    }

    void AtualizarPlacarUI()
    {
        if (gerenciador != null && gerenciador.questionarioAtual != null && gerenciador.questionarioAtual.name == "MCHAT")
        {
            if (modoSeguimentoAtivo)
            {
                txtPontos.text = "Falhas no Seguimento: " + placar;
            }
            else
            {
                txtPontos.text = "Sinais de Alerta: " + placar;
            }
        }
        else
        {
            txtPontos.text = "Acertos: " + placar;
        }
    }

    public void GameOver()
    {
        timerAtivo = false;
        painelFim.SetActive(true);

        // TESTE DE CONSOLE:
    if (btnIniciarSeguimento != null) 
    {
        Debug.Log("Tentando esconder o botão agora...");
        btnIniciarSeguimento.gameObject.SetActive(false);
        Debug.Log("Estado atual do botão: " + btnIniciarSeguimento.gameObject.activeSelf);
    }
    else
    {
        Debug.LogError("O slot do botão está VAZIO no Inspector!");
    }

        // SEGURANÇA 3: Sempre força o botão a sumir ao abrir a tela de fim.
        // Ele só vai ligar se passar pelas condições estritas do M-CHAT abaixo.
        if (btnIniciarSeguimento != null) 
            btnIniciarSeguimento.gameObject.SetActive(false);

        Questionarios questionarioRodando = gerenciador.questionarioAtual;

        if (questionarioRodando != null)
        {
            // ====================================================================
            // --- BLOCO EXCLUSIVO DO M-CHAT (Sistemas de Alerta e Seguimento) ---
            // ====================================================================
            if (questionarioRodando.name == "MCHAT")
            {
                if (modoSeguimentoAtivo)
                {
                    txtPontosFinal.text = "Passa-falha Concluída.\nTotal de confirmações de risco: " + placar + ".";
                    // O botão já foi desligado na Segurança 3 e continuará desligado aqui.
                }
                else
                {
                    if (placar >= 3)
                    {
                        txtPontosFinal.text = "Pontuação: " + placar + " pontos de alerta.\nRisco Moderado/Alto detectado. É necessário realizar a passa-falha.";
                        
                        // LIGA O BOTÃO: Único lugar em todo o código onde o botão tem permissão de aparecer
                        if (btnIniciarSeguimento != null) 
                            btnIniciarSeguimento.gameObject.SetActive(true); 
                    }
                    else
                    {
                        txtPontosFinal.text = "Pontuação: " + placar + " pontos de alerta.\nRisco Baixo. Nenhuma ação é necessária.";
                    }
                }
            }
            // ====================================================================
            // --- BLOCO EXCLUSIVO DO QUIZ 2 (Regra de Escore 9+) -----------------
            // ====================================================================
            else if (questionarioRodando.name == "minitea") // <--- Troque pelo nome exato do seu arquivo na pasta Assets
            {
                if (placar >= 9)
                {
                    txtPontosFinal.text = "Escore de " + placar + " indica que a criança deve ser melhor avaliada quanto à possibilidade de TEA.";
                }
                else
                {
                    txtPontosFinal.text = "Você concluiu o Questionário 2 e marcou " + placar + " pontos, não apresentando grande possibilidade de TEA";
                }
            }
            // ====================================================================
            // --- PADRÃO GERAL (Para outros questionários futuros se houver) ----
            // ====================================================================
            else
            {
                txtPontosFinal.text = "Você concluiu o quiz tradicional!\nTotal: " + placar + " acertos de " + questionarioRodando.listaPerguntas.Length + ".";
            }
        }
        else
        {
            txtPontosFinal.text = "Fim do Questionário! Pontos obtidos: " + placar;
        }
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene("MenuInicial");
    }
}