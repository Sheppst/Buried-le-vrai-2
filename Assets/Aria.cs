using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] Transform Player;
    private Vector2 PlayerPosition;
    //Other
    [SerializeField] Transform GroundCheck;
    [SerializeField] Transform RoofCheck;
    [SerializeField] Transform WallCheck;
    [SerializeField] Transform LauchAttackUpAndDown;
    [SerializeField] float CheckRadius;
    [SerializeField] float CheckRadiusPlayer;
    [SerializeField] LayerMask GroundWallRoof_Layer;
    [SerializeField] LayerMask Player_Layer;

    private bool IsTouchingRoof;
    private bool IsTouchingWall;
    private bool IsTouchingGround;
    private bool PlayerIsInRange;
    private bool GoingUp = true;
    private bool FacingRight = true;
    private bool CanIdleState = true;
    private Rigidbody2D enemyRB;
    void Start()
    {
        IdelMoveDirection.Normalize();
        AttackMoveDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {
        IsTouchingRoof = Physics2D.OverlapCircle(RoofCheck.position, CheckRadius, GroundWallRoof_Layer);
        IsTouchingWall = Physics2D.OverlapCircle(WallCheck.position, CheckRadius, GroundWallRoof_Layer);
        IsTouchingGround = Physics2D.OverlapCircle(GroundCheck.position, CheckRadius, GroundWallRoof_Layer);
        PlayerIsInRange = Physics2D.OverlapCircle(LauchAttackUpAndDown.position, CheckRadiusPlayer,Player_Layer);
        if (CanIdleState)
        {
            IdelState();
        }
        
       //AttackUpAndDownState();
       if (PlayerIsInRange)
        {
            CanIdleState = false;
            AttackPlayer();
        }
        //FlipTowardsPlayer();
    }

    public void IdelState()
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
            if (FacingRight)
            {
                Flip();
            }
            else if (!FacingRight)
            {
                Flip();
            }
        }
          enemyRB.velocity = IdelMoveSpeed * IdelMoveDirection;
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
            if (FacingRight)
            {
                Flip();
            }
            else if (!FacingRight)
            {
                Flip();
            }
        }
        enemyRB.velocity = AttackMoveSpeed * AttackMoveDirection;
    }

    public void AttackPlayer()
    {
        // take player position 
        PlayerPosition = Player.position - transform.position;
        // normalize player position
        PlayerPosition.Normalize();
        // Attack On that position
        enemyRB.velocity = PlayerPosition * AttackPlayerMoveSpeed;
    }

    void FlipTowardsPlayer ()
    {
        float PlayerDirection = Player.position.x - transform.position.x;

        if (PlayerDirection > 0 && FacingRight)
        {
            Flip();
        }
        else if (PlayerDirection < 0 && !FacingRight)
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
        FacingRight = !FacingRight;
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
        Gizmos.DrawWireSphere(LauchAttackUpAndDown.position, CheckRadiusPlayer);
    }
}
