using System.Collections;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
    [SerializeField] private Animator m_PlayerAnimJump;

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	//[SerializeField] private float m_DashPower = 10000f;
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private bool m_DoubleJump = true;
	private bool m_JumpAnim;
	private bool m_OneDash;
	private Vector3 velocity = Vector3.zero;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround); // Crée une sorte de cercle avec comme centre un transform d'un objet en paramètre, la taille du cercle puis le layer où il est actif.
																													   // Tous collider rentrant dans les caractérisques du cercle sont listé....
        for (int i = 0; i < colliders.Length; i++) // ... Puis lu dans cette boucle 
		{
			if (colliders[i].gameObject != gameObject) // La détection va forcément détecter l'objet lui-même s'il possède un collider donc on cherche à éviter ça.
													   // Et si un autre objet entre en collision déclenche la condition.
			{
				m_OneDash = true;
				m_Grounded = true;
				if (m_JumpAnim)
				{ 
					m_PlayerAnimJump.SetBool("IsJumping", false);
					m_JumpAnim = false;
				}
			}	
		}
    }

    public void Move(float move, bool crouch, bool jump, bool dash, float dashPower)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround)) 
			{
				crouch = true;
			}
		}

		if (dash && m_OneDash)
		{
            if (dashPower == 0 && transform.localScale.x < 0)
            {
                dashPower = 10000f * -1;
            }
            else if (dashPower == 0)
            {
                dashPower = 10000f;
            }
            Vector2 D = new Vector2(dashPower, 0f);
			m_Rigidbody2D.AddForce(D);
			m_OneDash = false;
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing); // Ici la méthode applique un mouvement que doit suivre le joueur pendant la frame, qui dépendra de la vélocité voulu, celle actuelle ainsi que le temps voulu pour atteindre la cible 

            // If the input is moving the player right and the player is facing left...
            // Move étant la valeur de déplacement du joueur si elle est positive alors le joueur se déplacera sur la droite et sinon sur la gauche 
            if (move > 0) //&& !m_FacingRight// Ici le joueur on remarque que le move passe en positif et que le joueur fixe à gauche alors on le retourne 
            {
				// ... flip the player.
				Flip();
                Vector3 theScale = transform.localScale;
                theScale.x = Mathf.Abs(theScale.x);
                transform.localScale = theScale;
                //m_DashPower = Mathf.Abs(m_DashPower);
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0) //&& m_FacingRight // Ici l'inverse se produit
            {
				// ... flip the player.
				Flip();
                Vector3 theScale = transform.localScale;
                theScale.x = Mathf.Abs(theScale.x) * - 1;
                transform.localScale = theScale;
                //m_DashPower = Mathf.Abs(m_DashPower) * -1;
            }
		}
		// If the player should jump...
		if (m_Grounded && jump) // Si les conditons sont vraie autorise le joueur à sauté une fois
		{
			StartCoroutine(AnimJump()); // Lance une coroutine pour l'animation de saut
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce)); // Confère une vélocité au joueur pour aller vers le haut
			m_DoubleJump = true;
		}
		else if (m_DoubleJump && jump) // Permet au joueur si les conditons sont rassemblé à sauté une deuxième fois 
		{
			StopAllCoroutines();
            StartCoroutine(AnimJump());
            m_PlayerAnimJump.Play("Player_Jump", 0, 0f); // Réinitialise l'animation de saut 
			m_DoubleJump = false;
			m_Rigidbody2D.velocity = Vector3.zero;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
	}


	private void Flip()
	{
		// Change le booléen en fonction de là ou le joueur fait face
		// càd que qu'au départ le sprite regarde à droite et la variable est en "true",
		// ainsi si la variable prend ici l'inverse de la valeur qu'elle possédait elle va inulectablement nous dire si le joueur fera face ou non à la droite
		

		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		//Vector3 theScale = transform.localScale;
		//theScale.x *= -1;
		//transform.localScale = theScale;
	}
	private IEnumerator AnimJump ()
	{
		yield return new WaitForSeconds(0.2f);
		m_JumpAnim = true;
	}

}
