using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuAndButtons : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverScreen;
    public float fadeInTime; // Tiempo que tarda en aparecer el GameOver
    public float fadeTimeMenu; // Tiempo del fade del menu.
    //public Button retryButton;
    //public Button backToMenuButton;

    void Start(){

    }

    public void Retry(){
        // Recargamos la escena.
        SceneManager.LoadScene("Level 1");
    }
    public void QuitToMenu(){
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu_Scene");
    }

    public void Continue(CanvasGroup panel){
        Time.timeScale = 1;
        fadeOutCoroutine = StartCoroutine(FadeOut(panel, fadeTimeMenu));
    }

    public void ShowGameOverScreen(){
        gameOverScreen.SetActive(true);
        CanvasGroup panel = gameOverScreen.GetComponent<CanvasGroup>();
        if(fadeInCoroutine == null){
            fadeInCoroutine = StartCoroutine(FadeIn(panel, fadeInTime)); // Llamo a la rutina que hace visible al menu.
        }
    }

    public void TogglePauseScreen(){
        if(fadeInCoroutine != null || fadeOutCoroutine != null)
        {
            return; // SI se esta ejecutando algo ya, no hacer nada.
        }
        if(Time.timeScale == 0){ // Si esta pausado, despausamos el tiempo.
            Time.timeScale = 1;
        }
        pauseMenu.SetActive(true); // Lo activo al comienzo
        CanvasGroup panel = pauseMenu.GetComponent<CanvasGroup>();
        if(fadeInCoroutine == null && panel.alpha == 0){
            //Debug.Log("Entro al fade in");
            fadeInCoroutine = StartCoroutine(FadeIn(panel, fadeTimeMenu, true)); // Lo hace visible
        } else if(fadeOutCoroutine == null && panel.alpha == 1){
            //Debug.Log("Entro al fade out");
            fadeOutCoroutine = StartCoroutine(FadeOut(panel, fadeTimeMenu)); // Lo hace invisible
        }
    }

    // Rutina que vuelve un panel visible.
    public Coroutine fadeInCoroutine = null;
    IEnumerator FadeIn(CanvasGroup panel, float fadeTime, bool changeTimeScale = false){
        while(panel.alpha < 1){
            panel.alpha += Time.deltaTime / fadeTime;
            yield return null;
        }
        panel.interactable = true;
        yield return null;
        fadeInCoroutine = null;
        if(changeTimeScale){
            Time.timeScale = 0;
        }
    }

    // Rutina que lo hace invisible.
    public Coroutine fadeOutCoroutine = null;
    IEnumerator FadeOut(CanvasGroup panel, float fadeTime){
        while(panel.alpha > 0){
            panel.alpha -= Time.deltaTime / fadeTime;
            yield return null;
        }
        panel.interactable = false;
        yield return null;
        pauseMenu.SetActive(false); // Lo desactivo
        fadeOutCoroutine = null;
    }
}
