using System.Collections;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    private Mana mana;
    [SerializeField] private int doubleJumpCost;
    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_CeilingCheck;
    [SerializeField] private Collider2D m_CrouchDisableCollider;
    [SerializeField] private Animator m_PlayerAnimJump;
    [SerializeField] private AudioSource m_PlayerAudioSourceStep;
    [SerializeField] private AudioSource m_PlayerAudioSourceDashSaut;
    [SerializeField] private AudioClip[] m_StepClip;
    [SerializeField] private DustEffect dustEffect; // Référence au script de l'effet de poussière

    const float k_GroundedRadius = .2f;
    private bool m_Grounded;
    const float k_CeilingRadius = .2f;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private bool m_DoubleJump = true;
    private bool m_JumpAnim;
    private bool m_OneDash;
    private int m_CurrentAudio;
    private Vector3 velocity = Vector3.zero;

    public bool canMove = true; // Drapeau pour contrôler le mouvement, initialisé à true
    public bool isKnockedBack = false; // Drapeau pour indiquer si le joueur est en knockback

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        mana = GetComponent<Mana>();
        canMove = true; // Assurez-vous que canMove est initialisé à true
    }

    private void FixedUpdate()
    {
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_OneDash = true;
                m_Grounded = true;
                m_DoubleJump = true; // Réinitialiser le double saut lorsqu'on touche le sol
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
        if (!canMove) return; // Empêcher le mouvement si canMove est false
        if (isKnockedBack) return; // Si le joueur est en knockback, désactiver le mouvement et les sauts

        if (!crouch)
        {
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        if (dash && m_OneDash)
        {
            m_PlayerAudioSourceDashSaut.Play();
            if (dashPower == 0 && transform.localScale.x < 0)
            {
                dashPower = 3500f * -1;
            }
            else if (dashPower == 0)
            {
                dashPower = 3500f;
            }
            Vector2 D = new Vector2(dashPower, 0f);
            m_Rigidbody2D.AddForce(D);
            m_OneDash = false;
        }

        if (m_Grounded || m_AirControl)
        {
            if (crouch)
            {
                move *= m_CrouchSpeed;

                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;
            }

            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

            if (move > 0 && !m_FacingRight)
            {
                Flip();
                dustEffect.CreateDust(); // Créer l'effet de poussière lors du changement de direction
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip();
                dustEffect.CreateDust(); // Créer l'effet de poussière lors du changement de direction
            }

            // Gérer les sons de pas ici
            // if (move != 0 && m_Grounded)
            // {
            //     if (!m_PlayerAudioSourceStep.isPlaying)
            //     {
            //         m_PlayerAudioSourceStep.clip = m_StepClip[m_CurrentAudio];
            //         m_CurrentAudio = (m_CurrentAudio + 1) % m_StepClip.Length;
            //         m_PlayerAudioSourceStep.Play();
            //     }
            // }
        }

        if (jump)
        {
            if (m_Grounded)
            {
                StartCoroutine(AnimJump());
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                dustEffect.CreateDust(); // Créer l'effet de poussière lors du saut
            }
            else if (m_DoubleJump && mana.manaPool >= doubleJumpCost)
            {
                mana.SpendMana(doubleJumpCost);
                StopAllCoroutines();
                StartCoroutine(AnimJump());
                m_PlayerAnimJump.Play("Player_Jump", 0, 0f);
                m_DoubleJump = false;
                m_Rigidbody2D.velocity = Vector3.zero;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                dustEffect.CreateDust(); // Créer l'effet de poussière lors du double saut
            }
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private IEnumerator AnimJump()
    {
        yield return new WaitForSeconds(0.2f);
        m_JumpAnim = true;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!canMove)
        {
            // Désactiver les animations si le mouvement est désactivé
            m_PlayerAnimJump.SetFloat("IsRunning", 0f);
            m_PlayerAnimJump.SetBool("IsJumping", false);
            m_Rigidbody2D.velocity = Vector2.zero; // Réinitialiser la vélocité
        }
    }

    public bool IsGrounded()
    {
        return m_Grounded;
    }
}
