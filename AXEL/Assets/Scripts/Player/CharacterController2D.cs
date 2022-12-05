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

	const float k_GroundedRadius = .2f;
	private bool m_Grounded;
	private Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true;
	private Vector3 velocity = Vector3.zero;
	private float limitFallSpeed = 25f;

	public bool canDoubleJump = false; 
	bool doubleJumpUnlocked = false;
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

    [SerializeField]  int heartsQuantity;
    [SerializeField]  Image[] hearts;
    [SerializeField]  Sprite fullHeart;
    [SerializeField]  Sprite emptyHeart;

	private enum CURRENT_TERRAIN {GRASS, GRAVEL};
    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

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
		HeartsController();

		if (OnFallEvent == null)
			OnFallEvent = new UnityEvent();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha2) && life < heartsQuantity && m_Grounded)
		{
			StartCoroutine(Heal());
		}
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

				JumpAnimation();
				m_Grounded = false;
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				if(doubleJumpUnlocked)
				{
				canDoubleJump = true;
				}
				particleJumpDown.Play();
				particleJumpUp.Play();
			}
			else if (!m_Grounded && jump && canDoubleJump && !isWallSliding)
			{
				canDoubleJump = false;
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

	void DetermineTerrain()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position, Vector3.down, 10.0f);

        foreach (RaycastHit rayhit in hit)
        {
            if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Gravel"))
            {
                currentTerrain = CURRENT_TERRAIN.GRAVEL;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Grass"))
            {
                currentTerrain = CURRENT_TERRAIN.GRASS;
            }
        }
    }



	IEnumerator Heal()
	{
		m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
		canMove = false;
		GetComponent<Attack>().enabled = false;
		yield return new WaitForSeconds(2f);
		life++;
		HeartsController();
		GetComponent<Attack>().enabled = true;
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
		GetComponent<Attack>().enabled = false;
		GameManager.deathCount++;
		yield return new WaitForSeconds(0.4f);
		m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
		yield return new WaitForSeconds(1.1f);
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}

	IEnumerator WaitToWallJumpAgain()
	{
	canJumpInWall = false;
	yield return new WaitForSeconds(0.3f);
	canJumpInWall = true;					
	}
}