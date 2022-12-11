using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKeysController : MonoBehaviour
{

    public GameObject leftMouse;
    public GameObject rightMouse;
    public GameObject alpha1;
    public GameObject alpha2;
    public GameObject backSpace;
    public GameObject doubleBackSpace1;
    public GameObject doubleBackSpace2;

    Image leftMouseImage;
    Image rightMouseImage;
    Image alpha1Image;
    Image alpha2Image;
    Image backSpaceImage;   
    Image doubleBackSpace1Image;
    Image doubleBackSpace2Image;    

    // Start is called before the first frame update
    void Start()
    {
        leftMouseImage = leftMouse.GetComponent<Image>();
        rightMouseImage = rightMouse.GetComponent<Image>();
        alpha1Image = alpha1.GetComponent<Image>();
        alpha2Image = alpha2.GetComponent<Image>();
        backSpaceImage = backSpace.GetComponent<Image>();
        doubleBackSpace1Image = doubleBackSpace1.GetComponent<Image>();
        doubleBackSpace2Image = doubleBackSpace2.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.gameIsPaused)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameManager.mouseLeftTimesPressed++;
                StartCoroutine(ChangeColor(leftMouseImage, 0.1f));
            }
            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                GameManager.mouseRightTimesPressed++;
                StartCoroutine(ChangeColor(rightMouseImage, 0.1f));
            }
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameManager.alpha1TimesPressed++;
                StartCoroutine(ChangeColor(alpha1Image, 0.1f));
            }                
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameManager.alpha2TimesPressed++;
                StartCoroutine(ChangeColor(alpha2Image, 0.1f));
            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.spacebarTimesPressed++;
                StartCoroutine(ChangeColor(backSpaceImage, 0.1f));
            } 
        }
    }

	IEnumerator ChangeColor(Image colorImage, float time)
    {
        colorImage.color = new Color32(0, 255, 0, 255);
		yield return new WaitForSeconds(time);
		colorImage.color = new Color32(255, 255, 255, 255);
    }

    public void CambiarColorDobleSaltoCorutina(){
        StartCoroutine(ChangeDualColor(doubleBackSpace1Image, doubleBackSpace2Image, 0.1f));
    }
    IEnumerator ChangeDualColor(Image colorImage,Image colorImage2, float time)
    {
        colorImage.color = new Color32(0, 255, 0, 255);
        colorImage2.color = new Color32(0, 255, 0, 255);
		yield return new WaitForSeconds(time);
		colorImage.color = new Color32(255, 255, 255, 255);
        colorImage2.color = new Color32(255, 255, 255, 255);
    }


}
