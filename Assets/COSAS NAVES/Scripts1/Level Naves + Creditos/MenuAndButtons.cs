using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuAndButtons : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverScreen;
    public GameObject winScreen;
    public GameObject warningScreen;
    public GameObject tutorialScreen;
    public float fadeInTime; // Tiempo que tarda en aparecer el GameOver
    public float fadeTimeMenu; // Tiempo del fade del menu.
    int jugoNivel;

    void Awake(){
        Cursor.visible = true;
        jugoNivel = PlayerPrefs.GetInt("JugoNivel", 0);
        if(jugoNivel == 0){
        Time.timeScale = 0;
        }
        if(jugoNivel == 1){
            // Escondemos el tuto
            tutorialScreen.SetActive(false);
        }
    }

    public void Retry(){
        int holder;
        holder = PlayerPrefs.GetInt("deathCount",0);
        holder++;
        PlayerPrefs.SetInt("deathCount",holder);
        Debug.Log(holder + " SE ESTÁ MURIENDO EN EL NIVEL DE NAVES");
        // Recargamos la escena.
        SceneManager.LoadScene("Level 1");
    }
    public void QuitToMenu(){
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu_Scene");
    }

    public void QuitToCredits(){
        Time.timeScale = 1;
        SceneManager.LoadScene("Credits");
    }

    public void Continue(CanvasGroup panel){
        Time.timeScale = 1;
        PlayerPrefs.SetInt("JugoNivel", 1);
        fadeOutCoroutine = StartCoroutine(FadeOut(panel, fadeTimeMenu));
    }

    public void ShowGameOverScreen(){
        MakeMouseVisible();
        gameOverScreen.SetActive(true);
        CanvasGroup panel = gameOverScreen.GetComponent<CanvasGroup>();
        if(fadeInCoroutine == null){
            fadeInCoroutine = StartCoroutine(FadeIn(panel, fadeInTime)); // Llamo a la rutina que hace visible al menu.
        }
    }

    public void ShowVictoryScreen(){
        MakeMouseVisible();
        winScreen.SetActive(true);
        CanvasGroup panel = winScreen.GetComponent<CanvasGroup>();
        if(fadeInCoroutine == null){
            fadeInCoroutine = StartCoroutine(FadeIn(panel, fadeInTime, true)); // Llamo a la rutina que hace visible al menu.
        }
    }

    void MakeMouseVisible(){
        if(!Cursor.visible){
            Cursor.visible = true;
        }
    }

    public void TogglePauseScreen(){
        MakeMouseVisible();
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

    public void ToggleWarningMessage(){
        if(fadeInCoroutine != null || fadeOutCoroutine != null)
        {
            return; // SI se esta ejecutando algo ya, no hacer nada.
        }
        warningScreen.SetActive(true);
        CanvasGroup panel = warningScreen.GetComponent<CanvasGroup>();
        if(fadeInCoroutine == null && panel.alpha == 0){
            //Debug.Log("Entro al fade in");
            blinking = StartCoroutine(Blinking(panel, 1f)); // Lo hace visible
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
        if(tutorialScreen.activeSelf == true){
            tutorialScreen.SetActive(false);
        }
        fadeOutCoroutine = null;
    }

    public Coroutine blinking = null;
    IEnumerator Blinking(CanvasGroup panel, float fadeTime){
        for(int i=0; i<3; i++){
            while(panel.alpha < 1){
                panel.alpha += Time.deltaTime / fadeTime;
                yield return null;
            }
            yield return new WaitForEndOfFrame();
            // Lo hacemos invisible
            while(panel.alpha > 0){
                panel.alpha -= Time.deltaTime / fadeTime;
                yield return null;
            }
            yield return null;
        }
        yield return null;
        warningScreen.SetActive(false);
    }
}
