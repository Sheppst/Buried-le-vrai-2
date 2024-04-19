using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPos : MonoBehaviour
{
    [SerializeField] float EcartementCam;
    [SerializeField] float CamSpeed;
    private bool turn;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Target = new Vector3();
        if (transform.parent.localScale.x > 0)
        {
            Target = transform.parent.position + Vector3.right * EcartementCam;
        }
        else if (transform.parent.localScale.x < 0)
        {
            Target = transform.parent.position + Vector3.left * EcartementCam;
        }
        if (Target.x == transform.position.x)
        {
            transform.Translate(Target * Time.deltaTime * CamSpeed);
        }
    }
}
