using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    CharacterController2D character;

    [Header("AttackAbility")]
    public Image attackImage;
    public float attackCooldown = 0.3f;
    bool attackIsCooldown = false;
    //public KeyCode keyAttack;

    [Header("RangeAttackAbility")]
    public Image rangeAttackImage;
    public float rangeAttackCooldown = 2.5f;
    bool rangeAttackisCooldown = false;
    //public KeyCode keyRangeAttack;

    [Header("HealAbility")]
    public Image healImage;
    public float healCooldown = 2.0f;
    bool healisCooldown = false;
    //public KeyCode keyHeal;   

    [Header("DashAbility")]
    public Image dashImage;
    public float dashCooldown = 0.5f;
    bool dashIsCooldown = false;
    //public KeyCode keyDash;

    [Header("JumpAbility")]
    public Image jumpImage;
    public float jumpCooldown = 0.5f;
    bool jumpisCooldown = false;
    //public KeyCode keyJump;   

    [Header("DoubleJumpAbility")]
    public Image doubleJumpImage;
    bool doubleJumpIsCooldown = false;
    //public KeyCode keyDoubleJump;     

    // Start is called before the first frame update
    void Start()
    {
        attackImage.fillAmount = 0;
        rangeAttackImage.fillAmount = 0;
        healImage.fillAmount = 0;
        dashImage.fillAmount = 0;
        jumpImage.fillAmount = 0;
        doubleJumpImage.fillAmount = 1;
        character = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackAbility();
        RangeAttackAbility();
        HealAbility();
        DashAbility();
        JumpAbility();
        DoubleJumpAbility();
    }

    public void AttackAbility()
    {
        if(!attackIsCooldown)
        {
            attackIsCooldown = true;
            attackImage.fillAmount = 1;
        }
        if(attackIsCooldown)
        {
            attackImage.fillAmount -= 1 / attackCooldown * Time.deltaTime;
            if(attackImage.fillAmount <= 0)
            {
                attackImage.fillAmount = 0;
                attackIsCooldown = false;
            }
        }
    }

    public void RangeAttackAbility()
    {
        if(!rangeAttackisCooldown)
        {
            rangeAttackisCooldown = true;
            rangeAttackImage.fillAmount = 1;
        }
        if(rangeAttackisCooldown)
        {
            rangeAttackImage.fillAmount -= 1 / rangeAttackCooldown * Time.deltaTime;
            if(rangeAttackImage.fillAmount <= 0)
            {
                rangeAttackImage.fillAmount = 0;
                rangeAttackisCooldown = false;
            }
        }
    }

    public void HealAbility()
    {
        if(!healisCooldown)
        {
            healisCooldown = true;
            healImage.fillAmount = 1;
        }
        if(healisCooldown)
        {
            healImage.fillAmount -= 1 / healCooldown * Time.deltaTime;
            if(healImage.fillAmount <= 0)
            {
                healImage.fillAmount = 0;
                healisCooldown = false;
            }
        }
    }

    public void DashAbility()
    {
        if(!dashIsCooldown)
        {
            dashIsCooldown = true;
            dashImage.fillAmount = 1;
        }
        if(dashIsCooldown)
        {
            dashImage.fillAmount -= 1 / dashCooldown * Time.deltaTime;
            if(dashImage.fillAmount <= 0)
            {
                dashImage.fillAmount = 0;
                dashIsCooldown = false;
            }
        }
    }

    public void JumpAbility()
    {
        if(!jumpisCooldown)
        {
            jumpisCooldown = true;
            jumpImage.fillAmount = 1;
        }
        if(jumpisCooldown)
        {
            jumpImage.fillAmount -= 1 / jumpCooldown * Time.deltaTime;
            if(jumpImage.fillAmount <= 0)
            {
                jumpImage.fillAmount = 0;
                jumpisCooldown = false;
            }
        }
    }

    public void DoubleJumpAbility()
    {
        if(!doubleJumpIsCooldown)
        {
            doubleJumpIsCooldown = true;
            doubleJumpImage.fillAmount = 1;
        }
        if(doubleJumpIsCooldown)
        {
            if(character.canDoubleJump && character.doubleJumpUnlocked)
            {
             doubleJumpImage.fillAmount= 0;
            doubleJumpIsCooldown = false;
            }
        }
    }



}
