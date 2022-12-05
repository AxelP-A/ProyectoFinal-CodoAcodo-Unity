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

    /*private FMOD.Studio.EventInstance musicInstance;
    public FMODUnity.EventReference musicEvent; */  

    private FMOD.Studio.EventInstance itemInstance;
    public FMODUnity.EventReference itemEvent;

    private FMOD.Studio.EventInstance introductoryNarrativeInstance;
    public FMODUnity.EventReference introductoryNarrativeEvent;

    [SerializeField]
    public string floorType = null;

    [SerializeField]
    public string gameSection = null;    

    [SerializeField]
    public string itemState = null;   

   // public GameObject player;
   

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
		DontDestroyOnLoad (gameObject);

        //jumpInstance = FMODUnity.RuntimeManager.CreateInstance(jumpEvent);
        walkInstance = FMODUnity.RuntimeManager.CreateInstance(walkEvent);
        ambienceInstance = FMODUnity.RuntimeManager.CreateInstance(ambienceEvent);
        itemInstance = FMODUnity.RuntimeManager.CreateInstance(itemEvent);
        //musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        introductoryNarrativeInstance = FMODUnity.RuntimeManager.CreateInstance(introductoryNarrativeEvent);
        StartCoroutine(StartFlowOfAmbienceSound());
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
        //FMODUnity.RuntimeManager.PlayOneShotAttached(walkEvent, gameObject);
    }

    public void StopWalkingSound()
    {
        walkInstance.release();
    }

    public void PlayAmbienceSound()
    {
        ambienceInstance.start();
        //FMODUnity.RuntimeManager.PlayOneShotAttached(ambienceEvent, gameObject);
    }

    public void PlayItemSound()
    {
        itemInstance.start();
    }

    public void StopItemSound()
    {
        itemInstance.release();
    }

    /*public void PlayMusicSound()
    {
        musicInstance.start();
        //FMODUnity.RuntimeManager.PlayOneShotAttached(musicEvent, gameObject);
    }*/


    public void PlayIntroductoryNarrativeSound()
    {
        introductoryNarrativeInstance.start();
        //FMODUnity.RuntimeManager.PlayOneShotAttached(introductoryNarrativeEvent, gameObject);
    }   


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

    IEnumerator StartFlowOfAmbienceSound()
	{
        //PlayIntroductoryNarrativeSound();
		yield return new WaitForSeconds(1f);
        PlayAmbienceSound();
	}


}
