using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossFightEvent : MonoBehaviour
{

    public GameObject bossWall1;
    public GameObject bossWall2;   
    public GameObject boss;
    GameObject[] wallLimitBossEncounter;
    GameObject[] childsWallLimitBossEncounter;
    //int count;
    bool hasEnter = false;

    void Awake()
    {
       //count = 0; 
       wallLimitBossEncounter = GameObject.FindGameObjectsWithTag("BossArenaLimit");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player") && !hasEnter)
        {
            hasEnter = true;
            StartCoroutine(BossFightStarting(2f));
           bossWall1.SetActive(true);
           bossWall2.SetActive(true);
           boss.SetActive(true);

            /*for(int i=0; i < wallLimitBossEncounter.Length; i++)
            {
                BoxCollider2D boxCol = wallLimitBossEncounter[i].GetComponent<BoxCollider2D>();
                Light2D light = wallLimitBossEncounter[i].GetComponent<Light2D>();
                light.enabled = true;
                boxCol.enabled = true;

                foreach (Transform child in wallLimitBossEncounter[i].transform)
                {
                    count++;
                }  

                childsWallLimitBossEncounter = new GameObject[count];
                count = 0;

                foreach (Transform childs in wallLimitBossEncounter[i].transform)
                {
                    childsWallLimitBossEncounter[count] = childs.gameObject;
                    count++;
                }

                foreach (GameObject go in childsWallLimitBossEncounter)
                {
                    SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
                    spriteRenderer.enabled = true;
                }
            }*/
        }
    }


    public void BossDefeated()
    {
    bossWall1.SetActive(false);
    bossWall2.SetActive(false);
    }

    IEnumerator BossFightStarting(float time)
	{
        FMODAudioPlayer.Instance.PlayBossStartFightSound();
		yield return new WaitForSeconds(time);
        FMODAudioPlayer.Instance.setGameSection("BossFight");

	}
    


   /* private void ChangeState(bool state)
    {
            for(int i=0; i < wallLimitBossEncounter.Length; i++)
            {
                BoxCollider2D boxCol = wallLimitBossEncounter[i].GetComponent<BoxCollider2D>();
                Light2D light = wallLimitBossEncounter[i].GetComponent<Light2D>();
                light.enabled = state;
                boxCol.enabled = state;

                foreach (Transform child in wallLimitBossEncounter[i].transform)
                {
                    count++;
                }  

                childsWallLimitBossEncounter = new GameObject[count];
                count = 0;

                foreach (Transform childs in wallLimitBossEncounter[i].transform)
                {
                    childsWallLimitBossEncounter[count] = childs.gameObject;
                    count++;
                }

                foreach (GameObject go in childsWallLimitBossEncounter)
                {
                    SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
                    spriteRenderer.enabled = state;
                }
            }
        }*/
}