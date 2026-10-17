using UnityEngine;

public class TocadorSomBotao : MonoBehaviour
{
    [Header("Configuração de Áudio")]
    public AudioSource audioSource;
    public AudioClip somClique;

    // Função para ser chamada no On Click() de cada botão
    public void TocarSom()
    {
        if (audioSource != null && somClique != null)
        {
            // PlayOneShot garante que o som toca do início sem ser cortado
            audioSource.PlayOneShot(somClique);
        }
        else if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}