using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Return : MonoBehaviour
{
    public GameObject panelEstadisticas;
    CanvasGroup panelEst;
    public Creditos cScript;

    void Awake(){
        panelEst = panelEstadisticas.transform.GetComponent<CanvasGroup>();
    }
    public void Volver()
    {
        SceneManager.LoadScene("Menu_Scene");
    }

    public void Continue()
    {
        Debug.Log("Se presiono continue");
        if(fadeOutCoroutine == null){
            fadeOutCoroutine = StartCoroutine(FadeOut(panelEst, 1f));
        }
    }

    public Coroutine fadeOutCoroutine = null;
    IEnumerator FadeOut(CanvasGroup panel, float fadeTime){
        while(panel.alpha > 0){
            panel.alpha -= Time.deltaTime / fadeTime;
            yield return null;
        }
        panel.interactable = false;
        yield return null;
        panelEstadisticas.SetActive(false); // Lo desactivo
        cScript.StartRollingCredits();
        fadeOutCoroutine = null;
    }
}
