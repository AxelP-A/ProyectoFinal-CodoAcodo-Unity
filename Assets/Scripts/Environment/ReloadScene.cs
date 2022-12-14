using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReloadScene : MonoBehaviour
{

    public GameObject endOfRun;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Player"))
       {
            StartCoroutine(EndOfThisRun());
       }
    }

     IEnumerator EndOfThisRun()
    {
        FMODAudioPlayer.Instance.StopAmbienceSound();
        yield return new WaitForSeconds(0.2f);
        endOfRun.SetActive(true);
        FMODAudioPlayer.Instance.StartDistortionSound();
        GameManager.Instance.DestroyAndSaveData();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(2);
    }

}
