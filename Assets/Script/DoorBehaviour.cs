using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
   public bool isDoorOpen = false;
    Vector3 doorClosedPos;
    Vector3 doorOpenPos;
    [SerializeField] float doorSpeed;
    [SerializeField] float doorHigh;
    // Start is called before the first frame update
    void Awake()
    {
        doorClosedPos = transform.position;
        doorOpenPos = new Vector3(transform.position.x, transform.position.y + doorHigh, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        if (isDoorOpen)
        {
            OpenDoor();
        }
        else if (!isDoorOpen)
        {
            CloseDoor();
        }
    }
    void OpenDoor()
    {
        if (transform.position != doorOpenPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorOpenPos, doorSpeed * Time.deltaTime);
        }
    }
    void CloseDoor()
    {
        if (transform.position != doorClosedPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorClosedPos, doorSpeed * Time.deltaTime);
        }
    }
}
