using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelecao : MonoBehaviour
{
    public static Questionarios QuestionarioEscolhido { get; private set; }

    [Header("Arraste os dois Questionários aqui")]
    public Questionarios questionario1;
    public Questionarios questionario2;

    [Header("Nome da Cena do Quiz")]
    public string nomeCenaQuiz = "CenaQuiz";

    // Função para o Botão 1
    public void JogarQuestionario1()
    {
        CarregarQuiz(questionario1);
    }

    // Função para o Botão 2
    public void JogarQuestionario2()
    {
        CarregarQuiz(questionario2);
    }

    private void CarregarQuiz(Questionarios q)
    {
        if (q != null)
        {
            QuestionarioEscolhido = q;
            SceneManager.LoadScene(nomeCenaQuiz);
        }
        else
        {
            Debug.LogError("O questionário selecionado está faltando no Inspector!");
        }
    }
}