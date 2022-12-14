using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODAudioPlayer : MonoBehaviour
{

    /*private FMOD.Studio.EventInstance jumpInstance;
    public FMODUnity.EventReference jumpEvent;*/

    private FMOD.Studio.EventInstance walkInstance;
    public FMODUnity.EventReference walkEvent;

    private FMOD.Studio.EventInstance ambienceInstance;
    public FMODUnity.EventReference ambienceEvent;

    private FMOD.Studio.EventInstance bossDeathInstance;
    public FMODUnity.EventReference bossDeathEvent;   

    private FMOD.Studio.EventInstance itemInstance;
    public FMODUnity.EventReference itemEvent;

    /*private FMOD.Studio.EventInstance introductoryNarrativeInstance;
    public FMODUnity.EventReference introductoryNarrativeEvent;*/

    private FMOD.Studio.EventInstance bossFightStartInstance;
    public FMODUnity.EventReference bossFightStartEvent;

    private FMOD.Studio.EventInstance distortionInstance;
    public FMODUnity.EventReference distortionEvent;

    [SerializeField]
    public string floorType = null;

    [SerializeField]
    public string gameSection = null;    

    [SerializeField]
    public string itemState = null;   

   // public GameObject player;
   public bool isPaused = false;
   

   public static FMODAudioPlayer Instance = null;
    
    void Awake()
	{

		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		//DontDestroyOnLoad (gameObject);

        //jumpInstance = FMODUnity.RuntimeManager.CreateInstance(jumpEvent);
        walkInstance = FMODUnity.RuntimeManager.CreateInstance(walkEvent);
        ambienceInstance = FMODUnity.RuntimeManager.CreateInstance(ambienceEvent);
        itemInstance = FMODUnity.RuntimeManager.CreateInstance(itemEvent);
        bossDeathInstance = FMODUnity.RuntimeManager.CreateInstance(bossDeathEvent);
        //introductoryNarrativeInstance = FMODUnity.RuntimeManager.CreateInstance(introductoryNarrativeEvent);
        bossFightStartInstance = FMODUnity.RuntimeManager.CreateInstance(bossFightStartEvent);
        distortionInstance = FMODUnity.RuntimeManager.CreateInstance(distortionEvent);
	}

    void Update()
    {
        walkInstance.setParameterByNameWithLabel("floorType", floorType);
        ambienceInstance.setParameterByNameWithLabel("GameState", gameSection);
        itemInstance.setParameterByNameWithLabel("itemState", itemState);
    }

    public void setFloorType(string type)
    {
        floorType = type;
    }

    public void setGameSection(string type)
    {
        gameSection = type;
    }

    public void setItemState(string type)
    {
        itemState = type;
    }


    /* public void PlayJumpSound()
    {
        jumpInstance.start();
        //FMODUnity.RuntimeManager.PlayOneShotAttached(jumpEvent, gameObject);
    }*/

    public void PlayWalkSound()
    {
        walkInstance.start();
 
        //walkInstance.release();        
        //FMODUnity.RuntimeManager.PlayOneShotAttached(walkEvent, gameObject);
    }

    public void StopWalkingSound()
    {
        walkInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void PlayAmbienceSound()
    {
        ambienceInstance.start();
        //FMODUnity.RuntimeManager.PlayOneShotAttached(ambienceEvent, gameObject);
    }
        public void StopAmbienceSound()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        ambienceInstance.release();
    }

     public void PauseSound()
     {
        isPaused = !isPaused;
        ambienceInstance.setPaused(isPaused);
        //introductoryNarrativeInstance.setPaused(isPaused);
     }

    public void PlayItemSound()
    {
        itemInstance.start();
    }

    public void StopItemSound()
    {
        itemInstance.release();
    }

    public void PlayBossDeathSound()
    {
        bossDeathInstance.start();
        //FMODUnity.RuntimeManager.PlayOneShotAttached(musicEvent, gameObject);
    }

    /*public void StopBossDeathSound()
    {
        bossDeathInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        ambienceInstance.release();
        //FMODUnity.RuntimeManager.PlayOneShotAttached(musicEvent, gameObject);
    }*/
    



    public void PlayBossStartFightSound()
    {
        bossFightStartInstance.start();
        //FMODUnity.RuntimeManager.PlayOneShotAttached(musicEvent, gameObject);
    }

    public void StopBossStartFightSound()
    {
        bossFightStartInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        bossFightStartInstance.release();
        //FMODUnity.RuntimeManager.PlayOneShotAttached(musicEvent, gameObject);
    } 

    public void StartDistortionSound()
    {
            distortionInstance.start();
    }



   /* public void PlayIntroductoryNarrativeSound()
    {
        introductoryNarrativeInstance.start();
        //FMODUnity.RuntimeManager.PlayOneShotAttached(introductoryNarrativeEvent, gameObject);
    }  */ 

  /*  public void PlaySong(FMODUnity.EventReference event)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(event, gameObject);
    } 
     */  



/*
    public void PlayJumpSound(event, gameObject)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(event, gameObject);
    }*

    {
        StartCoroutine(JumpSound(1.1f));
    }

     IEnumerator JumpSound(float time)
    {
        Debug.Log(enSuelo + "valor de variable antes de modificar su valor");
        enSuelo = 0;
        Debug.Log(enSuelo + "valor de variable al modificar su valor a 0");
        FMODUnity.RuntimeManager.PlayOneShotAttached(jumpEvent, gameObject);
        yield return new WaitForSeconds(time);
        enSuelo = 1; 
        Debug.Log(enSuelo + "valor de variable al modificar su valor a 1");
        FMODUnity.RuntimeManager.PlayOneShotAttached(jumpEvent, gameObject);
    }

*/

    public IEnumerator WaitToStopFootsteps(float time)
    {
        walkInstance.start();
        yield return new WaitForSeconds(time);
        walkInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    IEnumerator StartFlowOfAmbienceSound()
	{
        //PlayIntroductoryNarrativeSound();
		yield return new WaitForSeconds(0.5f);
        PlayAmbienceSound();
	}


}
