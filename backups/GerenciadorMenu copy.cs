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
}