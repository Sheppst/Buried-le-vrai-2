using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPos : MonoBehaviour
{
    [SerializeField] float EcartementCam;
    [SerializeField] float CamSpeed;
    [SerializeField] float CamReturnSpeedDivisor = 4;

    private Vector3 Target;
    private bool turn;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.up * transform.parent.position.y;
        if (!Input.anyKey)
        {
            Target = transform.parent.position;
            if (transform.position != Target) 
            { 
                transform.position = Vector3.MoveTowards(transform.position, Target, CamSpeed * Time.deltaTime/CamReturnSpeedDivisor); 
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            Target = transform.parent.position + Vector3.right * EcartementCam;
            if (transform.position.x < Target.x)
            {
                transform.position += Vector3.right * Time.deltaTime * CamSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Target = transform.parent.position + Vector3.left * EcartementCam;
            if (transform.position.x > Target.x)
            {
                transform.position += Vector3.left * Time.deltaTime * CamSpeed;
            }
        }
        //transform.position = Target;
    }
}
