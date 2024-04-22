using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dash : MonoBehaviour
{
    private Rigidbody2D rb;
    private TrailRenderer _trailRenderer;
    private Animator _animator;
    [Header ("GroundCheck")]

    [SerializeField] Transform GroundCheck;
    [SerializeField] float CheckRadius;
    [SerializeField] LayerMask GroundWallRoof_Layer;
    [SerializeField] private bool _IsGrounded;






    [Header("Dashing")]

    [SerializeField] private float _dashingVelocity = 14f;
    [SerializeField] private float _dashingTime = 0.5f;

    private Vector2 _dashingDir;
    private bool _isDashing;
    private bool _canDash;
    void Start()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, CheckRadius, GroundWallRoof_Layer);
    }

    // Update is called once per frame
    void Update()
    {
        var dashInput = Input.GetButtonDown("Dash");
        if (dashInput && _canDash)
        {
            _isDashing = true;
            _canDash = true;
            _trailRenderer.emitting = true;
            _dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (_dashingDir ==  Vector2.zero)
            {
                _dashingDir = new Vector2(transform.localScale.x, 0);
            }
            StartCoroutine(StopDashing());  
        }
        _animator.SetBool("IsDashing", _isDashing);

        if (_isDashing)
        {
            rb.velocity = _dashingDir.normalized * _dashingVelocity;
            return;
        }
        if (_IsGrounded)
        {
            _canDash = true;
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_dashingTime);
        _trailRenderer.emitting = false;
        _isDashing = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(GroundCheck.position, CheckRadius);
    }
}
