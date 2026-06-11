using UnityEngine;

[System.Serializable]
public class Perguntas
{
    public string enunciado;
    public Sprite imagemPergunta;
    public string[] alternativas; 

    [Header("Configurações de Resposta")]
    [Tooltip("Usado no Quiz Normal: Índice da alternativa correta que dá ponto.")]
    public int respostaCorreta; 

    [Tooltip("Usado apenas no M-CHAT: Índice da alternativa que gera sinal de alerta.")]
    public int respostaAlerta; 
}