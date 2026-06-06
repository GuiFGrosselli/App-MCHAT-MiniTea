using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MecanicasQuiz : MonoBehaviour
{
    [Header("UI – Informações")]
    public TextMeshProUGUI txtPontos;        // Texto da pontuação
    public TextMeshProUGUI txtTimer;         // Texto do timer

    [Header("UI – Game Over")]
    public GameObject painelFim;             // Painel de Game Over
    public TextMeshProUGUI txtPontosFinal;    // Pontuação final

    [Header("Configurações do Timer")]
    public float tempoMaximo = 15f;          // Segundos para responder

    // Variáveis privadas
    private int placar = 0;
    private float tempoRestante;
    private bool timerAtivo = false;

    // Referência ao gerenciador de perguntas
    private GerenciadorPerguntas gerenciador;

    void Start()
    {
        gerenciador = GetComponent<GerenciadorPerguntas>();
        placar = 0;
        AtualizarPlacar();
    }

    void Update()
    {
        // Só conta o tempo se o timer estiver ativo
        if (timerAtivo)
        {
            tempoRestante -= Time.deltaTime;
            AtualizarTimerUI();

            // Se o tempo acabou
            if (tempoRestante <= 0f)
            {
                tempoRestante = 0f;
                timerAtivo = false;
                TempoEsgotado();
            }
        }
    }

    // Reinicia o timer (chamado pelo GerenciadorPerguntas)
    public void ReiniciarTimer()
    {
        tempoRestante = tempoMaximo;
        timerAtivo = true;
        AtualizarTimerUI();
    }

    // Atualiza o texto do timer na tela
    void AtualizarTimerUI()
    {
        int segundos = Mathf.CeilToInt(tempoRestante);
        txtTimer.text = "Tempo: " + segundos;

        // Muda a cor quando faltam 5 segundos ou menos
        if (tempoRestante <= 5f)
            txtTimer.color = Color.red;
        else
            txtTimer.color = Color.white;
    }

    // Quando o tempo acaba: não pontua, vai para próxima
    void TempoEsgotado()
    {
        // Não soma pontos!
        gerenciador.CarregarPergunta();
    }

    // Chamado quando o jogador clica em uma alternativa
    public void Responder(bool acertou)
    {
        timerAtivo = false; // Para o timer

        if (acertou)
        {
            placar += 1;
            AtualizarPlacar();
        }

        gerenciador.CarregarPergunta();
    }

    // Atualiza o texto da pontuação
    void AtualizarPlacar()
    {
        txtPontos.text = "Pontos: " + placar;
    }

    // Mostra a tela de Game Over
 // Mostra a tela de Game Over com mensagens baseadas no questionário e na pontuação
    public void GameOver()
    {
        timerAtivo = false;
        painelFim.SetActive(true);

        // Pega o questionário atual do gerenciador
        Questionarios questionarioRodando = gerenciador.questionarioAtual;

        if (questionarioRodando != null)
        {
            // --- REGRAS PARA O QUESTIONÁRIO 1 ---
            // IMPORTANTE: Mude "NomeDoSeuArquivo1" para o nome do arquivo na pasta Assets
            if (questionarioRodando.name == "mchat")
            {
                if (placar >= 0 && placar <= 2)
                {
                    txtPontosFinal.text = "BAIXO RISCO: Pontuação total é de 0-2; se a criança tiver menos de 24 meses, repetir o M-CHAT-R aos 24 meses. Não é necessário qualquer outra medida, a não ser que a vigilância indique risco de TEA.";
                }
                else if (placar >= 3 && placar <= 7)
                {
                    txtPontosFinal.text = "RISCO MODERADO: Pontuação total é 3-7; administrar a Entrevista de Seguimento (segunda etapa do M-CHAT-R/F)";
                }
                else if (placar >= 8 && placar <= 20)
                {
                    txtPontosFinal.text = "ALTO RISCO: Pontuação total é de 8-20; pode-se prescindir da Entrevista de Seguimento e encaminhar a criança para avaliação diagnóstica e também para avaliação da necessidade de intervenção.";
                }
                else
                {
                    // Caso faça mais de 20 pontos por algum motivo
                    txtPontosFinal.text = "Excelente resultado! Você fez " + placar + " pontos.";
                }
            }
            
            // --- REGRAS PARA O QUESTIONÁRIO 2 ---
            // IMPORTANTE: Mude "NomeDoSeuArquivo2" para o nome do arquivo na pasta Assets
            else if (questionarioRodando.name == "minitea")
            {
                if (placar >= 9)
                {
                    txtPontosFinal.text = "Escore de 09 ou mais indica que a criança deve ser melhor avaliada quanto à possibilidade de TEA.";
                }
                else
                {
                    // Mensagem caso a pontuação seja 9 ou menos no Questionário 2
                    txtPontosFinal.text = "Você concluiu o Questionário 2 e marcou " + placar + " pontos.";
                }
            }
            
            // --- CASO NENHUM DOS DOIS SEJA IDENTIFICADO (SEGURANÇA) ---
            else
            {
                txtPontosFinal.text = "Você concluiu o quiz! Total: " + placar + " pontos.";
            }
        }
        else
        {
            txtPontosFinal.text = "Fim de jogo! Pontuação: " + placar;
        }
    }

    // Botão Reiniciar (chamado pelo botão na UI)
    public void Reiniciar()
    {
        SceneManager.LoadScene("MenuInicial");
    }
}