using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Aria : MonoBehaviour
{
    //For idel Stage
    [Header("Idel")]
    [SerializeField] float IdelMoveSpeed;
    [SerializeField] Vector2 IdelMoveDirection;
    //For up and down Stage
    [Header("AttackUpAndDown")]
    [SerializeField] float AttackMoveSpeed;
    [SerializeField] Vector2 AttackMoveDirection;
    //For AttackPlayer Stage
    [Header("AttackPlayer")]
    [SerializeField] float AttackPlayerMoveSpeed;
    [SerializeField] float maxAttackPlayerHeight;

    // player check
    [SerializeField] Transform Player;
    
    private Vector2 PlayerPosition;
    private bool HasPlayerPosition;
    

    //Other
    [SerializeField] Transform GroundCheck;
    [SerializeField] Transform RoofCheck;
    [SerializeField] Transform WallCheck;
  


    [SerializeField] float CheckRadius;
    [SerializeField] float CheckRadiusPlayer;
    [SerializeField] LayerMask GroundWallRoof_Layer;
    [SerializeField] LayerMask Player_Layer;

    [SerializeField] private bool LastWasAttackPlayer;
    [SerializeField] private bool LastWasUpAndDown;
    private bool IsTouchingRoof;
    private bool IsTouchingWall;
    private bool IsTouchingGround;
    
    private bool GoingUp = true;
    private bool FacingLeft = true;
    
    private Rigidbody2D enemyRB;
    private Animator Enemyanimator;
    void Start()
    {
        IdelMoveDirection.Normalize();
        AttackMoveDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
        Enemyanimator = GetComponent<Animator>();   
    }

   
    void FixedUpdate()
    {
        IsTouchingRoof = Physics2D.OverlapCircle(RoofCheck.position, CheckRadius, GroundWallRoof_Layer);
        IsTouchingWall = Physics2D.OverlapCircle(WallCheck.position, CheckRadius, GroundWallRoof_Layer);
        IsTouchingGround = Physics2D.OverlapCircle(GroundCheck.position, CheckRadius, GroundWallRoof_Layer);
        

    }
  
    private void RandomStatePicker()
    {
        int randomState = Random.Range(0, 11);

        if (LastWasUpAndDown == false)

        {
            if (randomState >= 5 && LastWasAttackPlayer == false && transform.position.y >= 2.7)
            {

                FlipTowardsPlayer();
                Enemyanimator.Play("AboutToAttackPlayer");
                LastWasAttackPlayer = true;
                LastWasUpAndDown = false;
                
                // Attaque Up And Down
            }
            else
            {

                Enemyanimator.Play("AboutToAttackUpAndDown");
                LastWasAttackPlayer = false; // Attaque Player
                LastWasUpAndDown = true;
            }
        }
        if(LastWasUpAndDown)
        {
            if (randomState >= 3 && LastWasAttackPlayer == false && transform.position.y >= 2.7)
            {

                FlipTowardsPlayer();
                Enemyanimator.Play("AboutToAttackPlayer");
                LastWasAttackPlayer = true;
                LastWasUpAndDown = false;
                // Attaque Up And Down
            }
            else
            {

                Enemyanimator.Play("AboutToAttackUpAndDown");
                LastWasAttackPlayer = false; // Attaque Player
                LastWasUpAndDown = true;
            }
        }
       
        
    }
    
    public void IdelState()
    {
        Enemyanimator.ResetTrigger("Slam");
        
        if (IsTouchingRoof && GoingUp) 
        {
            ChangeDirection();
        }
        else if (IsTouchingGround && !GoingUp)
        {
            ChangeDirection();
        }
        if (IsTouchingWall)
        {
            if (FacingLeft)
            {
                Flip();
            }
            else if (!FacingLeft)
            {
                Flip();
            }
        }
          enemyRB.velocity = IdelMoveSpeed * IdelMoveDirection * Time.fixedDeltaTime;
    }

   public void AttackUpAndDownState()
    {
        if (IsTouchingRoof && GoingUp)
        {
            ChangeDirection();
        }
        else if (IsTouchingGround && !GoingUp)
        {
            ChangeDirection();
        }
        if (IsTouchingWall)
        {
            if (FacingLeft)
            {
                Flip();
            }
            else if (!FacingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = AttackMoveSpeed * AttackMoveDirection * Time.fixedDeltaTime;
    }

    public void AttackPlayer()
    {
        if (!HasPlayerPosition)
        {
            PlayerPosition = Player.position - transform.position;
            PlayerPosition.Normalize();
            HasPlayerPosition = true;

        }
        if (HasPlayerPosition)
        
        {
            FlipTowardsPlayer();
            enemyRB.velocity = PlayerPosition * AttackPlayerMoveSpeed * Time.fixedDeltaTime;
        }
       if (IsTouchingGround || IsTouchingWall)
        {
            enemyRB.velocity = Vector2.zero;
            HasPlayerPosition= false;
            Enemyanimator.SetTrigger("Slam");
            Enemyanimator.ResetTrigger("AttackPlayer");
        }
        
        
    }

    void FlipTowardsPlayer ()
    {
        float PlayerDirection = Player.position.x - transform.position.x;

        if (PlayerDirection > 0 && FacingLeft)
        {
            Flip();
        }
        else if (PlayerDirection < 0 && !FacingLeft)
        {
            Flip();
        }
    }
    void ChangeDirection()
    {
        GoingUp = !GoingUp;
        IdelMoveDirection.y *= -1;
        AttackMoveDirection.y *= -1;
    }

    void Flip()
    {  
        FacingLeft = !FacingLeft;
        IdelMoveDirection.x *= -1;
        AttackMoveDirection.x *= -1;
        transform.Rotate(0, 180, 0);

    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(GroundCheck.position, CheckRadius);
     
        Gizmos.DrawWireSphere(WallCheck.position, CheckRadius);
        
        Gizmos.DrawWireSphere(RoofCheck.position, CheckRadius);
      

    }
}
