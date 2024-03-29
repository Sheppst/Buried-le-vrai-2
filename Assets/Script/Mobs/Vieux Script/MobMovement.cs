using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public Transform Player;
    public Transform Right;
    public Transform Left;
    public Transform Current;
    public Transform Detect;
    public float speed;
    private bool HitReactPlayer;
    public bool Aggro;
    // Start is called before the first frame update
    void Start()
    {
        Right.parent = null;
        Left.parent = null;
        Detect = GetComponentInChildren<Transform>();

    }

    // Update is called once per frame
    void Update()
    {

        HitReactPlayer = GetComponentInChildren<Detect>().Player;

        if (transform.position == Current.position) 
        {
            if (Current == Right)
            {
                Current = Left;                
            }
            else if (Current == Left) 
            {
                Current = Right;
            }
        }
        if (!HitReactPlayer) //&& !Aggro) 
        { 
            transform.position = Vector3.MoveTowards(transform.position, Current.position,Time.deltaTime * speed);
        }
        else if (HitReactPlayer) 
        {
        //    Aggro = true;
        //    StartCoroutine(FollowPlayer()); 
        //}
        //if (Aggro) 
        //{
            Vector3 Change = new Vector3(Player.position.x, transform.position.y, Player.position.z);
            transform.position = Vector3.MoveTowards(transform.position, Change, Time.deltaTime * speed);
        }

        if (transform.position.x > Current.position.x && !HitReactPlayer)
        {
            transform.localScale = new Vector3(2, transform.localScale.y, transform.localScale.z);
        }
        else if (transform.position.x < Current.position.x && !HitReactPlayer)
        {
            transform.localScale = new Vector3(-2, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            if (transform.position.x > Player.position.x)
            {
                transform.localScale = new Vector3(2, transform.localScale.y, transform.localScale.z);
            }
            else if (transform.position.x < Player.position.x)
            {
                transform.localScale = new Vector3(-2, transform.localScale.y, transform.localScale.z);
            }
        }

    }
    private IEnumerator FollowPlayer()
    {
        yield return new WaitForSeconds(3);
        if (HitReactPlayer) 
        {
            StartCoroutine(FollowPlayer());
        }
        else
        { Aggro = false; }
        
    }
}
