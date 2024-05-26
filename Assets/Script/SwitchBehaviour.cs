using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBehaviour : MonoBehaviour
{
    [SerializeField] List<DoorBehaviour> doorBehaviours; // Liste de portes à gérer
    [SerializeField] bool isDoorOpenSwitch;
    [SerializeField] bool isDoorCloseSwitch;

    [SerializeField] float shakeDuration = 0.2f; // Durée du tremblement
    [SerializeField] float shakeMagnitude = 0.3f; // Intensité du tremblement
    [SerializeField] float shakeDelay = 0.5f; // Délai avant le tremblement

    float switchSizeY;
    float switchspeed = 1f;
    float switchDelay = 0.2f;
    Vector3 switchUpPos;
    Vector3 switchDownPos;
    bool isPressingSwitch = false;
    CameraShake cameraShake; // Référence au script CameraShake
    Transform playerTransform; // Référence à la position du joueur

    void Awake()
    {
        switchSizeY = transform.localScale.y / 2;
        switchUpPos = transform.position;
        switchDownPos = new Vector3(transform.position.x, transform.position.y - switchSizeY, transform.position.z);
        cameraShake = Camera.main.GetComponent<CameraShake>(); // Obtenez la référence au script CameraShake
    }

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
            playerTransform = collision.transform; // Obtenez la position du joueur

            foreach (DoorBehaviour doorBehaviour in doorBehaviours)
            {
                if (isDoorOpenSwitch && !doorBehaviour.isDoorOpen)
                {
                    doorBehaviour.isDoorOpen = true;
                    StartCoroutine(TriggerShakeWithDelay()); // Appeler le tremblement de la caméra avec délai
                }
                else if (isDoorCloseSwitch && doorBehaviour.isDoorOpen)
                {
                    doorBehaviour.isDoorOpen = false;
                    StartCoroutine(TriggerShakeWithDelay()); // Appeler le tremblement de la caméra avec délai
                }
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

    IEnumerator TriggerShakeWithDelay()
    {
        yield return new WaitForSeconds(shakeDelay); // Attendre avant de déclencher le tremblement
        StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude, playerTransform.position));
    }
}
