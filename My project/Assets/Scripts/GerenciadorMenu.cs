using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorMenu : MonoBehaviour
{
    [Header("Painel de Termos de Uso")]
    public GameObject painelTermoDeUso; // Arraste o painel aqui no Inspector

    // 'static' sobrevive às trocas de cena (ex.: botão Retornar), mas é zerado
    // quando o aplicativo é fechado. Assim o termo aparece uma vez a cada abertura do app.
    private static bool termoJaAceito = false;

    void Start()
    {
        if (painelTermoDeUso != null)
        {
            // Mostra o termo só se ainda não foi aceito nesta abertura do app
            painelTermoDeUso.SetActive(!termoJaAceito);
        }
    }

    // Função para o botão "Aceitar/Fechar" do Termo de Uso
    public void FecharTermo()
    {
        termoJaAceito = true; // Lembra que o termo foi aceito enquanto o app estiver aberto

        if (painelTermoDeUso != null)
        {
            painelTermoDeUso.SetActive(false); // Esconde o painel
        }
    }

    // --- NOVA FUNÇÃO PARA SAIR DO APLICATIVO ---
    public void FecharAplicativo()
    {
        Debug.Log("Fechando o aplicativo...");

        // Fecha o aplicativo no Android ou iOS real
        Application.Quit();

        #if UNITY_EDITOR
        // Se estiver rodando dentro do editor da Unity, para a execução do Playmode
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    
    
}