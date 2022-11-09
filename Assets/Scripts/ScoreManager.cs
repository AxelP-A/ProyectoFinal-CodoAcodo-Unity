using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreDisplay;
    int scoreValue;
    void Start()
    {
        scoreValue = 0;
        scoreDisplay.text = "0";
    }

    // Se le puede dar un valor negativo si se quiere quitar puntos tmb x ser golpeado o algo.
    public void UpdateScore(int ammount){ // Actualizamos el valor y lo mostramos.
        scoreValue += ammount;
        scoreDisplay.text = scoreValue.ToString();
        // Si se logra una meta, pasar al jefe x ahi ?.
        if(scoreValue >= 4000)
        {
            Debug.Log("PASAS DE NIVEL O ALGO");
        }
    }
}
