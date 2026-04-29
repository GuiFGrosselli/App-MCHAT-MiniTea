using UnityEngine;


[CreateAssetMenu(fileName = "NovoQuestionario", menuName = "Questionarios", order = 1)]

public class Questionarios : ScriptableObject
{
    public string nomeQuestionario;
    public Perguntas [] listaPerguntas;
}
