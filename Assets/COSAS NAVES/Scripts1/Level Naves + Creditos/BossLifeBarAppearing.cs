using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLifeBarAppearing : MonoBehaviour
{
    private Action _appear;
    private Image barraHp;
    [SerializeField] float spawnSpeed; // Mas lento va mas rapido.
    // Start is called before the first frame update
    void Awake()
    {
        barraHp = transform.GetChild(1).GetComponent<Image>();
        _appear = delegate {};
        
    }

    void OnEnable(){
        _appear += LoadBar;
    }
    void LoadBar()
    {
        if(barraHp.fillAmount < 1)
        {
            barraHp.fillAmount += Time.deltaTime/spawnSpeed;
        } else {
            _appear = delegate { };
            Destroy(this);
        }
    }

    void Update(){
        // Ejecutamos el delegate, q dsps se borra solo.
        _appear();
    }
}
