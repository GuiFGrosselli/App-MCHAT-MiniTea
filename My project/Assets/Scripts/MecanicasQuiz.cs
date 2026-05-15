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
    public void GameOver()
    {
        timerAtivo = false;
        painelFim.SetActive(true);
        txtPontosFinal.text = "Você fez " + placar + " pontos!";
    }

    // Botão Reiniciar (chamado pelo botão na UI)
    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}