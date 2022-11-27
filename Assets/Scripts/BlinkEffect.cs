using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    public Color startColor;
    public Color endColor;
    [Range(0, 10)]
    public float speed = 3.0f;

    SpriteRenderer spriteRenderer;
    SpriteRenderer shieldRenderer;
    public bool blinkShield;
    public bool isABoss;

    void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if(!isABoss){
            shieldRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
        if(blinkShield)
        {
            shieldRenderer.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
        }
    }
    void OnDisable(){
        // Cuando se apaga le ponemos su color normal.
        spriteRenderer.material.color = startColor;
        if(blinkShield){
            shieldRenderer.material.color = startColor;
        }
    }
}
