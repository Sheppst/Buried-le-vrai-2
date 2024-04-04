using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;
	public GameObject TBombs;
	public GameObject SBombs;
	[SerializeField] private GameObject Boss;
	[SerializeField] private Rigidbody2D rigid;
	[SerializeField] private float PropulseX;
	[SerializeField] private float PropulseY;
	[SerializeField] private float ChargePower;


    public float runSpeed = 40f;

	private float Life = 100;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
	[HideInInspector] public bool dash;

    //Pourquoi c'est compliqué de faire son character controller : 
    //Flip le personnage de gauche a droite
    //Quand saute doit check si il y a le sol en dessous de lu
    //Check quand retombe par terre

    private void Awake()
    {
        dash = false;
    }

    // Update is called once per frame
    void Update () {

		if (Life <= 0f) 
		{
			print("Dead");
		}

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		animator.SetFloat("IsRunning",Mathf.Abs(horizontalMove));

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("IsJumping", true);
		}
		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
            animator.SetBool("IsCrouching", true);
        } else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
            animator.SetBool("IsCrouching", false);
        }
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			dash = true;
		}
		if (Input.GetKeyDown(KeyCode.Q)) 
		{
			Instantiate(TBombs,transform.position, Quaternion.identity);
		}
		if(Input.GetMouseButtonDown(1))
		{
            Instantiate(SBombs, transform.position, Quaternion.identity);
        }

	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, dash);
		jump = false;
		dash = false;
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mob")
		{
			Life -= 5;
		}
    }

    public void BiteByBoss ()
	{
		print("Mordu");
		if (Boss.GetComponent<Phase01>().transform.localScale.x < 0 )
		{
			// pousse le joueur sur la gauche légerement
			rigid.velocity = Vector3.zero;
            Vector2 Repouss = new Vector2(PropulseX * -1, PropulseY);
			rigid.AddForce(Repouss);
        }
		else if (Boss.GetComponent<Phase01>().transform.localScale.x > 0)
		{
            // pousse le joueur sur la droite légerement
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
            // pousse le joueur sur la gauche légerement
            rigid.velocity = Vector3.zero;
			Vector2 Repouss = new Vector2(PropulseX * -1 * ChargePower, PropulseY * ChargePower);
			rigid.AddForce(Repouss); 
        }
        else if (Boss.GetComponent<Phase01>().transform.localScale.x > 0)
        {
            // pousse le joueur sur la droite légerement
            rigid.velocity = Vector3.zero;
            Vector2 Repouss = new Vector2(PropulseX * ChargePower, PropulseY * ChargePower);
			rigid.AddForce(Repouss);
        }
        Life -= 15;
    }
}
