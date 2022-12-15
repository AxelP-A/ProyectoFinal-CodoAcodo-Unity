using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeath : BaseEnemy
{

    public GameObject enemy;
    public float meleeDist = 3.5f;
	public float rangeDist = 5f;
	private bool canAttack = true;
	private Transform attackCheck;
	public int dmgValue = 1;
	//public GameObject throwableObject;
	Rigidbody2D m_Rigidbody2D;
	Animator anim;
	SpriteRenderer spriteRenderer;
	float randomDecision = 0;
	bool doOnceDecision = true;
	bool endDecision = false;
	float distToPlayer;
	float distToPlayerY;
	int life = 10;
	public float speed = 5f; 
    private bool m_FacingRight = false; 
	bool fightStarted = false;
	public GameObject arenaController;
	BossFightEvent bossFightEvent;

    void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		attackCheck = transform.Find("AttackCheck").transform;
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		bossFightEvent = arenaController.GetComponent<BossFightEvent>();

	}

    void FixedUpdate()
	{
		DetectingEnemy();
		if (life <= 0)
		{
			if(destroyEnemy == null)
			{
				destroyEnemy = StartCoroutine(DestroyEnemy());
				//StartCoroutine(DestroyEnemy());
			}
		}
		else if (enemy != null) 
		{
			if(fightStarted)
			{
				if (!isHitted)
				{
					distToPlayer = enemy.transform.position.x - transform.position.x;
					distToPlayerY = enemy.transform.position.y - transform.position.y;

					if (Mathf.Abs(distToPlayer) < 0.25f)
					{
						GetComponent<Rigidbody2D>().velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
						anim.SetBool("IsWaiting", true);
					}
					else if (Mathf.Abs(distToPlayer) > 0.25f && Mathf.Abs(distToPlayer) < meleeDist && Mathf.Abs(distToPlayerY) < 2f)
					{
						GetComponent<Rigidbody2D>().velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
						if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f)) 
							FlipBoss();
						if (canAttack)
						{
							MeleeAttack();
						}
					}
					else if (Mathf.Abs(distToPlayer) > meleeDist && Mathf.Abs(distToPlayer) < rangeDist)
					{
						anim.SetBool("IsWaiting", false);
						m_Rigidbody2D.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
					}
					else
					{
						if (!endDecision)
						{
							if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f)) 
								FlipBoss();

							if (randomDecision < 0.4f)
								Run();
							else if (randomDecision >= 0.4f && randomDecision < 0.6f)
								Jump();
						/*else if (randomDecision >= 0.6f && randomDecision < 0.8f)
							StartCoroutine(Dash());
						else if (randomDecision >= 0.8f && randomDecision < 0.95f)
							RangeAttack();*/
							else
								Idle();
						}
						else
						{
							endDecision = false;
						}
					}
				}
				else if (isHitted)
				{
					if ((distToPlayer > 0f && transform.localScale.x > 0f) || (distToPlayer < 0f && transform.localScale.x < 0f))
					{
						FlipBoss();
						//StartCoroutine(Dash());
					}
					/*else
						StartCoroutine(Dash());*/
				}
			}
		}
		else 
		{
			enemy = GameObject.Find("DrawCharacter");
		}

		if (transform.localScale.x * m_Rigidbody2D.velocity.x > 0 && !m_FacingRight && life > 0)
		{
			FlipBoss();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (transform.localScale.x * m_Rigidbody2D.velocity.x < 0 && m_FacingRight && life > 0)
		{
			FlipBoss();
		}
	}

	

	public void ApplyDamage(int damage)
	{
		if (!isInvincible)
		{
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			anim.SetBool("Hit", true);
			StartCoroutine(ChangeRenderer(0.25f));
			life -= damage;
			transform.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
			transform.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 300f, 100f)); 
			StartCoroutine(HitTime());
		}
	}

	public void MeleeAttack()
	{
		transform.GetComponent<Animator>().SetBool("Attack", true);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy" && collidersEnemies[i].gameObject != gameObject )
			{
				if (transform.localScale.x < 1)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
			}
			else if (collidersEnemies[i].gameObject.tag == "Player")
			{
				collidersEnemies[i].gameObject.GetComponent<CharacterController2D>().ApplyDamage(1, transform.position);
			}
		}
		StartCoroutine(WaitToAttack(0.5f));
	}

	/*public void RangeAttack()
	{
		if (doOnceDecision && canRangeAttack)
		{
			if(!m_FacingRight)
			{
				rangedAttackRenderer.flipX = true;
			}
			else
			{
				rangedAttackRenderer.flipX = false;
			}
			GameObject throwableProj = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f), Quaternion.identity) as GameObject;
			throwableProj.GetComponent<ThrowableProjectile>().owner = gameObject;
			Vector2 direction = new Vector2(transform.localScale.x, 0f);
			throwableProj.GetComponent<ThrowableProjectile>().direction = direction;
			StartCoroutine(WaitToRangeAttack(5f));
			StartCoroutine(NextDecision(0.5f));
		}
	}*/

	public void Run()
	{
		anim.SetBool("IsWaiting", false);
		m_Rigidbody2D.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
		if (doOnceDecision)
			StartCoroutine(NextDecision(0.5f));
	}
	public void Jump()
	{
		Vector3 targetVelocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
		Vector3 velocity = Vector3.zero;
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, 0.05f);
		if (doOnceDecision)
		{
			anim.SetBool("IsWaiting", false);
			m_Rigidbody2D.AddForce(new Vector2(0f, 450f));
			StartCoroutine(NextDecision(1f));
		}
	}

	public void Idle()
	{
		m_Rigidbody2D.velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
		if (doOnceDecision)
		{
			anim.SetBool("IsWaiting", true);
			StartCoroutine(NextDecision(1f));
		}
	}


	void DetectingEnemy()
    {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left);

		if (hit.collider.gameObject.tag == "player")
		{
			fightStarted = true;
		}
		if (hit.transform.gameObject.CompareTag("Player"))
        {
            fightStarted = true;
        }
    }

	IEnumerator ChangeRenderer(float time)
	{
		spriteRenderer.color = new Color(239f, 0f, 0f, 255f);
		yield return new WaitForSeconds(time);
		spriteRenderer.color = new Color(255f, 255f, 255f, 255f);
		yield return new WaitForSeconds(time);
		spriteRenderer.color = new Color(239f, 0f, 0f, 255f);
		yield return new WaitForSeconds(time);
		spriteRenderer.color = new Color(255f, 255f, 255f, 255f);
		yield return new WaitForSeconds(time);		
		spriteRenderer.color = new Color(239f, 0f, 0f, 255f);
		yield return new WaitForSeconds(time);
		spriteRenderer.color = new Color(255f, 255f, 255f, 255f);
		yield return new WaitForSeconds(time);
		spriteRenderer.color = new Color(239f, 0f, 0f, 255f);
		yield return new WaitForSeconds(time);
		spriteRenderer.color = new Color(255f, 255f, 255f, 255f);
	}


	IEnumerator WaitToAttack(float time)
	{
		canAttack = false;
		yield return new WaitForSeconds(time);
		canAttack = true;
	}

	/*IEnumerator WaitToRangeAttack(float time)
	{
		canRangeAttack = false;
		yield return new WaitForSeconds(time);
		canRangeAttack = true;
	}*/

	/*IEnumerator Dash()
	{
		anim.SetBool("IsDashing", true);
		isDashing = true;
		yield return new WaitForSeconds(0.1f);
		isDashing = false;
		EndDecision();
	}*/

	Coroutine destroyEnemy = null;

	IEnumerator DestroyEnemy()
	{
		bossFightEvent.BossDefeated();
		GameManager.killCount++;
		FMODAudioPlayer.Instance.StopBossStartFightSound();
		FMODAudioPlayer.Instance.PlayBossDeathSound();
		CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
		capsule.size = new Vector2(1f, 0.25f);
		capsule.offset = new Vector2(0f, -0.8f);
		capsule.direction = CapsuleDirection2D.Horizontal;
		transform.GetComponent<Animator>().SetBool("IsDead", true);
		yield return new WaitForSeconds(0.25f);
		m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}

	IEnumerator NextDecision(float time)
	{
		doOnceDecision = false;
		yield return new WaitForSeconds(time);
		EndDecision();
		doOnceDecision = true;
		anim.SetBool("IsWaiting", false);
	}

	public void EndDecision()
	{
		randomDecision = Random.Range(0.0f, 1.0f); 
		endDecision = true;
	}
	
}