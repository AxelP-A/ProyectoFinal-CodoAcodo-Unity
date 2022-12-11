using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public float dmgValue = 4;
	public GameObject throwableObject;
	public Transform attackCheck;
	private Rigidbody2D m_Rigidbody2D;
	public Animator animator;
	public bool canAttack = true;
	public bool canRangeAttack = true;
	public bool isTimeToCheck = false;
	//SpriteRenderer playerRangeAttackRenderer;
	CharacterController2D character;

	bool isRangeAttacking;
	bool isAttacking;

	public GameObject cam;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	//	playerRangeAttackRenderer = throwableObject.GetComponent<SpriteRenderer>();
		character = GetComponent<CharacterController2D>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		character.AttackAbility(isAttacking);
		character.RangeAttackAbility(isRangeAttacking);
		if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
		{
			isAttacking = true;
			character.AttackAbility(isAttacking);
			canAttack = false;
			animator.SetBool("IsAttacking", true);
			StartCoroutine(AttackCooldown());
			isAttacking = false;
		}

		if (Input.GetKeyDown(KeyCode.Alpha1) && canRangeAttack && character.mana > 0)
		{
			isRangeAttacking = true;
			character.RangeAttackAbility(isRangeAttacking);
			canRangeAttack = false;
				
			character.mana--;
			character.HeartsController();
			StartCoroutine(RangeAttack());
			isRangeAttacking = false;
		}
	}


	IEnumerator AttackCooldown()
	{

		yield return new WaitForSeconds(0.25f);
		canAttack = true;
	}

	IEnumerator RangeAttack()
	{
		GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f,-0.2f), Quaternion.identity) as GameObject; 
		SpriteRenderer sp = throwableWeapon.GetComponent<SpriteRenderer>();
		if(!character.m_FacingRight)
		{
			sp.flipX = true;
		}
			else 
		{
			sp.flipX = false;
		}
		Vector2 direction = new Vector2(transform.localScale.x, 0);
		throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction; 
		throwableWeapon.name = "ThrowableWeapon";

		yield return new WaitForSeconds(2.5f);
		canRangeAttack = true;
	}

	public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				cam.GetComponent<CameraFollow>().ShakeCamera();
			}
		}
	}
}