using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreDisplay;
    public int pointsTowin;
    int scoreValue;
    bool bossEnabled;
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
        if(scoreValue >= pointsTowin && !GameManager.instance.gameState && !bossEnabled) // Nos fijamos el estado para que trigeree 1 vez sola
        {
            // x ahi trigereamos algo q flashie una imagen de warning con un sonido.
            GameManager.instance.menuScript.ToggleWarningMessage();
            // Desactivamos enemigos
            GameManager.instance.DisableEnemySpawning();
            VFXController.instance.PlayVFX(VFXController.VFXName.SIREN); // CAMBIAR A SIRENA.
            if(waitTime == null){
                waitTime = StartCoroutine(WaitTimeForBoss(5f));
            }
            bossEnabled = true;
            //GameManager.instance.TriggerVictory();
        }
    }

    Coroutine waitTime = null;
    IEnumerator WaitTimeForBoss(float waitTimeF){
        yield return new WaitForSeconds(waitTimeF);
        // Despues trigereamos el boss.
        GameManager.instance.TriggerBoss();
        waitTime = null;
    }
}
