using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Mana mana;
    private float Life;
    public CharacterController2D controller;
    public Animator animator;
    public GameObject TBombs;
    public GameObject SBombs;
    [SerializeField] private GameObject Boss;
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private float PropulseX;
    [SerializeField] private float PropulseY;
    [SerializeField] private float ChargePower;
    [SerializeField] private float dashspeed;
    private float dashPower = 0f;

    [SerializeField] private int dashCost = 15;
    [SerializeField] private int DoubleJumpCost = 15;
    private bool YesDash = true;

    public float runSpeed = 40f;
    public bool canControl = true; // Variable to manage player control

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    [HideInInspector] public bool dash;
    private bool animationsEnabled = true;

    private void Awake()
    {
        dash = false;
        mana = GetComponent<Mana>();
    }

    void Update()
    {
        if (!canControl) return; // Prevent player control during respawn delay

        if (!controller.isKnockedBack && animationsEnabled)
        {
            animator.SetFloat("JumpFall", rigid.velocity.y);
            animator.SetFloat("IsRunning", Mathf.Abs(horizontalMove));
        }

        if (controller.isKnockedBack) return; // Désactiver les contrôles pendant le knockback

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        dashPower = Input.GetAxisRaw("Horizontal") * dashspeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            if (animationsEnabled)
            {
                animator.SetBool("IsJumping", true);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && YesDash && mana.manaPool >= dashCost)
        {
            mana.SpendMana(dashCost);
            dash = true;
            YesDash = false;
            StartCoroutine(WaitDash());
        }
        if (Input.GetKeyDown(KeyCode.Q) && TBombs != null)
        {
            Instantiate(TBombs, transform.position, Quaternion.identity);
        }
        if (Input.GetMouseButtonDown(1) && SBombs != null)
        {
            Instantiate(SBombs, transform.position, Quaternion.identity);
        }
    }

    void FixedUpdate()
    {
        if (canControl) // Prevent player movement during respawn delay
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, dash, dashPower);
        }
        jump = false;
        dash = false;
    }

    public void SetAnimationsEnabled(bool enabled)
    {
        animationsEnabled = enabled;
        if (!enabled)
        {
            animator.SetFloat("IsRunning", 0f);
            animator.SetBool("IsJumping", false);
        }
    }

    public void BiteByBoss()
    {
        print("Mordu");
        if (Boss.GetComponent<Phase01>().transform.localScale.x < 0)
        {
            rigid.velocity = Vector3.zero;
            Vector2 Repouss = new Vector2(PropulseX * -1, PropulseY);
            rigid.AddForce(Repouss);
        }
        else if (Boss.GetComponent<Phase01>().transform.localScale.x > 0)
        {
            rigid.velocity = Vector3.zero;
            Vector2 Repouss = new Vector2(PropulseX, PropulseY);
            rigid.AddForce(Repouss);
        }
        Life -= 20;
    }

    public void ChargeByBoss()
    {
        print("Chargé");
        if (Boss.GetComponent<Phase01>().transform.localScale.x < 0)
        {
            rigid.velocity = Vector3.zero;
            Vector2 Repouss = new Vector2(PropulseX * -1 * ChargePower, PropulseY * ChargePower);
            rigid.AddForce(Repouss);
        }
        else if (Boss.GetComponent<Phase01>().transform.localScale.x > 0)
        {
            rigid.velocity = Vector3.zero;
            Vector2 Repouss = new Vector2(PropulseX * ChargePower, PropulseY * ChargePower);
            rigid.AddForce(Repouss);
        }
        Life -= 15;
    }

    private IEnumerator WaitDash()
    {
        yield return new WaitForSeconds(0.5f);
        YesDash = true;
    }
}
