using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    [SerializeField] DoorBehaviour DoorBehaviour;

    [SerializeField] bool isDoorOpenSwitch;
    [SerializeField] bool isDoorCloseSwitch;

    float switchSizeY;
    float switchspeed = 1f;
    float switchDelay = 0.2f;
    Vector3 switchUpPos;
    Vector3 switchDownPos;
    bool isPressingSwitch = false;
    void Awake()
    {
        switchSizeY = transform.localScale.y / 2;
        switchUpPos = transform.position;
        switchDownPos = new Vector3(transform.position.x, transform.position.y - switchSizeY, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressingSwitch)
        {
            MoveSwitchDown();
        }
        else if (!isPressingSwitch)
        {
            MoveSwitchUp();
        }
    }
    void MoveSwitchDown()
    {
        if (transform.position != switchDownPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, switchDownPos, switchspeed * Time.deltaTime);
        }
    }
    void MoveSwitchUp()
    {
        if (transform.position != switchUpPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, switchUpPos, switchspeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPressingSwitch = !isPressingSwitch;

            if (isDoorOpenSwitch && !DoorBehaviour.isDoorOpen)
            {
                DoorBehaviour.isDoorOpen = !DoorBehaviour.isDoorOpen;
            }
            else if (isDoorCloseSwitch && DoorBehaviour.isDoorOpen)
            {
                DoorBehaviour.isDoorOpen = !DoorBehaviour.isDoorOpen;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SwitchUpDelay(switchDelay));
        }
    }
    IEnumerator SwitchUpDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isPressingSwitch = false;
    }
}
