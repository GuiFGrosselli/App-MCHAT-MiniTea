using UnityEngine;

[CreateAssetMenu(fileName = "NovoQuestionario", menuName = "Questionarios", order = 1)]
public class Questionarios : ScriptableObject
{
    public string nomeQuestionario;

    [TextArea(5, 12)] // Cria uma caixa de texto maior e confortável no Inspector para digitar as instruções
    public string introducaoDoQuiz;

    public Perguntas[] listaPerguntas;
}