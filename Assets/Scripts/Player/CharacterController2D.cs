using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField]  float m_JumpForce = 400f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
	[SerializeField]  bool m_AirControl = false;
	[SerializeField]  LayerMask m_WhatIsGround;
	[SerializeField]  Transform m_GroundCheck;
	[SerializeField]  Transform m_WallCheck;
	SpriteRenderer spriteRenderer;

	[SerializeField] UIKeysController UIKeysRef;

	const float k_GroundedRadius = .2f;
	public bool m_Grounded;
	private Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true;
	private Vector3 velocity = Vector3.zero;
	private float limitFallSpeed = 25f;

	public bool canDoubleJump = false; 
	public bool doubleJumpUnlocked = false;
	[SerializeField] private float m_DashForce = 25f;
	private bool canDash = true;
	private bool isDashing = false; 
	private bool m_IsWall = false; 
	private bool isWallSliding = false; 
	private bool oldWallSlidding = false; 
	private float prevVelocityX = 0f;
	private bool canCheck = false; 

	public int life = 5; 
	public bool invincible = false; 
	private bool canMove = true;
	bool canJumpInWall = true;

	private Animator animator;
	public ParticleSystem particleJumpUp; 
	public ParticleSystem particleJumpDown; 

	private float jumpWallStartX = 0;
	private float jumpWallDistX = 0; 
	private bool limitVelOnWallJump = false; 
	bool jumpState;
	bool isHealing;

    [SerializeField]  int heartsQuantity;
    [SerializeField]  Image[] hearts;
    [SerializeField]  Sprite fullHeart;
    [SerializeField]  Sprite emptyHeart;

	public int manaQuantity;
    [SerializeField]  Image[] manaPotions;
    [SerializeField]  Sprite fullManaPotion;
    [SerializeField]  Sprite emptyManaPotion;
	public int mana = 5;
UIKeysController uiKeysController;
	private enum CURRENT_TERRAIN { GRASS, GRAVEL};
    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

	//Abilities abilities;


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
    //public float jumpCooldown = 0.5f;
    bool jumpisCooldown = false;
    //public KeyCode keyJump;   

    [Header("DoubleJumpAbility")]
    public Image doubleJumpImage;
    //bool doubleJumpIsCooldown = false;
    //public KeyCode keyDoubleJump;    





	[Header("Events")]
	[Space]
	public UnityEvent OnFallEvent;
	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		//abilities = GetComponent<Abilities>();
		HeartsController();
		ManaController();
		attackImage.fillAmount = 0;
        rangeAttackImage.fillAmount = 0;
        healImage.fillAmount = 0;
        dashImage.fillAmount = 0;
        jumpImage.fillAmount = 0;
        doubleJumpImage.fillAmount = 1;

		if (OnFallEvent == null)
			OnFallEvent = new UnityEvent();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha2) && life < heartsQuantity && mana > 0 && m_Grounded && !isHealing)
		{
			isHealing = true;
			GameManager.timesHealed++;
			StartCoroutine(ChangeRenderer(0.25f));
			StartCoroutine(Heal());
		}
		DetermineTerrain();
		HealAbility(isHealing);
		/*AttackAbility();
        RangeAttackAbility();*/
        DashAbility(isDashing);
        JumpAbility(jumpState);
        DJAbilityDisplayUpdate(jumpState);
	}

	private void FixedUpdate()
	{	
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
				if (!wasGrounded )
				{
					OnLandEvent.Invoke();
					if (!m_IsWall && !isDashing) 
						particleJumpDown.Play();
						if(doubleJumpUnlocked)
						{
							canDoubleJump = true;
							doubleJumpImage.fillAmount = 1;
						}

					
					if (m_Rigidbody2D.velocity.y < 0f)
						limitVelOnWallJump = false;
				}
		}
		m_IsWall = false;

		if (!m_Grounded)
		{
			OnFallEvent.Invoke();
			Collider2D[] collidersWall = Physics2D.OverlapCircleAll(m_WallCheck.position, k_GroundedRadius, m_WhatIsGround);
			for (int i = 0; i < collidersWall.Length; i++)
			{
				if (collidersWall[i].gameObject != null)
				{
					isDashing = false;
					m_IsWall = true;
				}
			}
			prevVelocityX = m_Rigidbody2D.velocity.x;
		}

		if (limitVelOnWallJump)
		{
			if (m_Rigidbody2D.velocity.y < -0.5f)
				limitVelOnWallJump = false;
			jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
			if (jumpWallDistX < -0.5f && jumpWallDistX > -1f) 
			{
				canMove = true;
			}
			else if (jumpWallDistX < -1f && jumpWallDistX >= -2f) 
			{
				canMove = true;
				m_Rigidbody2D.velocity = new Vector2(10f * transform.localScale.x, m_Rigidbody2D.velocity.y);
			}
			else if (jumpWallDistX < -2f) 
			{
				limitVelOnWallJump = false;
				m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
			}
			else if (jumpWallDistX > 0) 
			{
				limitVelOnWallJump = false;
				m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
			}
		}

	}

	public void SetUnlockDoubleJump(bool unlockDoubleJump)
	{
		doubleJumpUnlocked = unlockDoubleJump;
	}

	public void SetIfCanMove(bool canImove)
	{
		canMove = canImove;
	}

	public void SetIfCanAttack(bool canIattack)
	{
	GetComponent<Attack>().enabled = canIattack;
	}


	public void Move(float move, bool jump, bool dash)
	{

		if (canMove) {
			if (dash && canDash && !isWallSliding)
			{
				GameManager.quantityOfDashes++;
				StartCoroutine(DashCooldown());
			}

			if (isDashing)
			{
				m_Rigidbody2D.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
			}
			else if (m_Grounded || m_AirControl)
			{
				if (m_Rigidbody2D.velocity.y < -limitFallSpeed)
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -limitFallSpeed);
				Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

				if (move > 0 && !m_FacingRight && !isWallSliding)
				{
					Flip();
				}
				else if (move < 0 && m_FacingRight && !isWallSliding)
				{
					Flip();
				}
			}

			if (m_Grounded && jump)
			{
				jumpState = jump;
				GameManager.quantityOfJumps++;
				JumpAnimation();
				m_Grounded = false;
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				if(doubleJumpUnlocked)
				{
					canDoubleJump = true;
					doubleJumpImage.fillAmount = 1;
				}
				particleJumpDown.Play();
				particleJumpUp.Play();
			}
			else if (!m_Grounded && jump && canDoubleJump && !isWallSliding)
			{
				canDoubleJump = false;
				GameManager.quantityOfDoubleJumps++;
				UIKeysRef.CambiarColorDobleSaltoCorutina();
				DJAbilityDisplayUpdate(jump);
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce / 1.2f));
				animator.SetBool("IsDoubleJumping", true);
			}
			else if (m_IsWall && !m_Grounded)
			{
				if (!oldWallSlidding && m_Rigidbody2D.velocity.y < 0 || isDashing)
				{
					isWallSliding = true;
					m_WallCheck.localPosition = new Vector3(-m_WallCheck.localPosition.x, m_WallCheck.localPosition.y, 0);
					Flip();
					StartCoroutine(WaitToCheck(0.1f));
					if(doubleJumpUnlocked)
					{
						canDoubleJump = true;
						doubleJumpImage.fillAmount = 1;

					}
					animator.SetBool("IsWallSliding", true);
				}
				isDashing = false;

				if (isWallSliding)
				{
					if (move * transform.localScale.x > 0.1f)
					{
						StartCoroutine(WaitToEndSliding());
					}
					else 
					{
						oldWallSlidding = true;
						m_Rigidbody2D.velocity = new Vector2(-transform.localScale.x * 2, -5);
					}
				}

				if (jump && isWallSliding && canJumpInWall)
				{
					jumpState = jump;
					GameManager.quantityOfJumps++;
					JumpAnimation();
					m_Rigidbody2D.velocity = new Vector2(0f, 0f);
					m_Rigidbody2D.AddForce(new Vector2(transform.localScale.x * m_JumpForce * 1.5f, m_JumpForce));
					jumpWallStartX = transform.position.x;
					limitVelOnWallJump = true;
					StartCoroutine(WaitToWallJumpAgain());
					IsntWallSliding();
					canMove = false;
				}
				else if (dash && canDash)
				{
					IsntWallSliding();
					StartCoroutine(DashCooldown());
				}
			}
			else if (isWallSliding && !m_IsWall && canCheck) 
			{
			IsntWallSliding();
			}
		}
	}

	private void JumpAnimation()
	{
		animator.SetBool("IsJumping", true);
		animator.SetBool("JumpUp", true);	
	}

	private void IsntWallSliding()
	{
		isWallSliding = false;
		animator.SetBool("IsWallSliding", false);
		oldWallSlidding = false;
		m_WallCheck.localPosition = new Vector3(Mathf.Abs(m_WallCheck.localPosition.x), m_WallCheck.localPosition.y, 0);
		if(doubleJumpUnlocked)
		{
			canDoubleJump = true;
			doubleJumpImage.fillAmount = 1;			
		}
	}

	private void Flip()
	{
		m_FacingRight = !m_FacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void ApplyDamage(int damage, Vector3 position) 
	{
		if (!invincible)
		{
			animator.SetBool("Hit", true);
			GameManager.hitsGivenToYou++;
			life -= damage;
			HeartsController();
			Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f ;
			m_Rigidbody2D.velocity = Vector2.zero;
			m_Rigidbody2D.AddForce(damageDir * 10);

			if (life <= 0)
			{
				StartCoroutine(WaitToDead());
			}
			else
			{
				StartCoroutine(Stun(0.25f));
				StartCoroutine(BlinkingRenderer(0.25f));
				StartCoroutine(MakeInvincible(1f));
			}
		}
	}

	public void HeartsController()
    {
        if (life > heartsQuantity)
        {
            life = heartsQuantity;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < life)
            {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }

            if(i < heartsQuantity)
            {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
    }

	public void ManaController()
    {
        if (mana > manaQuantity)
        {
            mana = manaQuantity;
        }

        for (int i = 0; i < manaPotions.Length; i++)
        {
            if(i < mana)
            {
                manaPotions[i].sprite = fullManaPotion;
            } else {
                manaPotions[i].sprite = emptyManaPotion;
            }

            if(i < manaQuantity)
            {
                manaPotions[i].enabled = true;
            } else {
                manaPotions[i].enabled = false;
            }
        }
    }


	void DetermineTerrain()
    {
        //RaycastHit[] hit;
		RaycastHit2D hit;
		hit = Physics2D.Raycast(transform.position, Vector2.down, 5f);
        //hit = Physics.RaycastAll(transform.position, Vector3.down, 10.0f);
		Debug.Log(hit.transform.gameObject.layer + " ESTO ES LO QUE TOMA EL RAYCAST");
        //foreach (RaycastHit rayhit in hit)
        //{
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Gravel"))
            {
                currentTerrain = CURRENT_TERRAIN.GRAVEL;
				Debug.Log("Estoy en el terreno 1" + currentTerrain);
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Grass"))
            {
                currentTerrain = CURRENT_TERRAIN.GRASS;
				Debug.Log("Estoy en el terreno 2" + currentTerrain);
            }
       // }
    }

	public void SelectAndPlayFootstep()
    {     
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.GRAVEL:
				FMODAudioPlayer.Instance.setFloorType("gravel");
                FMODAudioPlayer.Instance.PlayWalkSound();
				Debug.Log("Estoy en el terreno 3" + currentTerrain);
				//FMODAudioPlayer.Instance.WaitToStopFootsteps(0.3f);

                break;

            case CURRENT_TERRAIN.GRASS:
				FMODAudioPlayer.Instance.setFloorType("sand");
                FMODAudioPlayer.Instance.PlayWalkSound();
				Debug.Log("Estoy en el terreno 4" + currentTerrain);
				//FMODAudioPlayer.Instance.WaitToStopFootsteps(0.3f);
                break;
            default:
                FMODAudioPlayer.Instance.PlayWalkSound();
				//FMODAudioPlayer.Instance.WaitToStopFootsteps(0.3f);
                break;
        }
    }



	public void AttackAbility(bool attack)
    {
        if(attack &&!attackIsCooldown)
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

    public void RangeAttackAbility(bool rangeAttack)
    {
        if(rangeAttack && !rangeAttackisCooldown)
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

    public void HealAbility(bool healing)
    {
        if(healing && !healisCooldown)
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

    public void DashAbility(bool dashing)
    {
        if(dashing && !dashIsCooldown)
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

    public void JumpAbility(bool jumping)
    {
        if(jumping && !jumpisCooldown)
        {
            jumpisCooldown = true;
            jumpImage.fillAmount = 1;
        }
        if((m_Grounded || m_IsWall) && jumpisCooldown)
        {
            jumpImage.fillAmount = 0;
			jumpisCooldown = false;
        }
    }

    public void DJAbilityDisplayUpdate(bool jumpedOnce)
    {
		//print("VALUE:" + doubleJumpImage.fillAmount);
		if(doubleJumpUnlocked){
			// 0 visible 1 gris
			//doubleJumpImage.fillAmount = 0;
			if(jumpedOnce && canDoubleJump)
			{
				doubleJumpImage.fillAmount = 0;
			} else {
				doubleJumpImage.fillAmount = 1;
			}
		}else { // Si no esta desbloqueado.
			doubleJumpImage.fillAmount = 1;
		}
    }

	


	IEnumerator ChangeRenderer(float time)
	{
		spriteRenderer.color = new Color(0f, 255f, 0f, 255f);
		yield return new WaitForSeconds(time);
		spriteRenderer.color = new Color(255f, 255f, 255f, 255f);
		yield return new WaitForSeconds(time);
		spriteRenderer.color = new Color(0f, 255f, 0f, 255f);
		yield return new WaitForSeconds(time);
		spriteRenderer.color = new Color(255f, 255f, 255f, 255f);
		yield return new WaitForSeconds(time);		
		spriteRenderer.color = new Color(0f, 255f, 0f, 255f);
		yield return new WaitForSeconds(time);
		spriteRenderer.color = new Color(255f, 255f, 255f, 255f);
		yield return new WaitForSeconds(time);
		spriteRenderer.color = new Color(0f, 255f, 0f, 255f);
		yield return new WaitForSeconds(time);
		spriteRenderer.color = new Color(255f, 255f, 255f, 255f);
	}

	IEnumerator BlinkingRenderer(float time)
	{
		spriteRenderer.enabled = false;
		yield return new WaitForSeconds(time);
		spriteRenderer.enabled = true;
		yield return new WaitForSeconds(time);
		spriteRenderer.enabled = false;
		yield return new WaitForSeconds(time);
		spriteRenderer.enabled = true;
	}


	IEnumerator Heal()
	{
		Animator anim = transform.GetComponent<Animator>();
		m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
		canMove = false;
		anim.SetBool("IsHealing", true);
		GetComponent<Attack>().enabled = false;
		yield return new WaitForSeconds(2f);
		mana--;
		ManaController();
		life++;
		HeartsController();
		GetComponent<Attack>().enabled = true;
		isHealing = false;
		anim.SetBool("IsHealing", false);
		canMove = true;
	}
	
	IEnumerator DashCooldown()
	{
		animator.SetBool("IsDashing", true);
		isDashing = true;
		canDash = false;
		yield return new WaitForSeconds(0.1f);
		isDashing = false;
		yield return new WaitForSeconds(0.5f);
		canDash = true;
	}

	IEnumerator Stun(float time) 
	{
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}

	IEnumerator MakeInvincible(float time) 
	{
		invincible = true;
		yield return new WaitForSeconds(time);
		invincible = false;
	}

	IEnumerator WaitToMove(float time)
	{
		canMove = false;
		yield return new WaitForSeconds(time);
		canMove = true;
	}

	IEnumerator WaitToCheck(float time)
	{
		canCheck = false;
		yield return new WaitForSeconds(time);
		canCheck = true;
	}

	IEnumerator WaitToEndSliding()
	{
		yield return new WaitForSeconds(0.1f);
		IsntWallSliding();
	}

	IEnumerator WaitToDead()
	{
		animator.SetBool("IsDead", true);
		canMove = false;
		invincible = true;
		FMODAudioPlayer.Instance.setGameSection("Forest(day)");
		GetComponent<Attack>().enabled = false;
		GameManager.deathCount++;
		yield return new WaitForSeconds(0.4f);
		m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
		FMODAudioPlayer.Instance.StopAmbienceSound();
		yield return new WaitForSeconds(1.1f);
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		//GameManager.Instance.startGameBlackScreen.SetActive(false);

	}

	IEnumerator WaitToWallJumpAgain()
	{
	canJumpInWall = false;
	yield return new WaitForSeconds(0.3f);
	canJumpInWall = true;					
	}
}