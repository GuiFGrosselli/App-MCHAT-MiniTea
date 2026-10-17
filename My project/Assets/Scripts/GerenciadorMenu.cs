using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorMenu : MonoBehaviour
{
    [Header("Painel de Termos de Uso")]
    public GameObject painelTermoDeUso; // Arraste o painel aqui no Inspector

    void Start()
    {
        // Opcional: Garante que o termo de uso apareça assim que o app abrir
        if (painelTermoDeUso != null)
        {
            painelTermoDeUso.SetActive(true);
        }
    }

    // Função para o botão "Aceitar/Fechar" do Termo de Uso
    public void FecharTermo()
    {
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