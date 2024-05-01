using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Rendering;
using UnityEngine;

public class Ascenceur : MonoBehaviour
{
    // Elliot Script


    public Transform StartPoint;
    public Transform EndPoint;
    public Transform Platform;
    private int direction = 1;
    public float speed = 1f;
   
    public bool Canelevate;
    
    private void Awake()
    {

    }
    void Update()
    {
        if (Canelevate)
        {
            Vector2 target = currentMovementTarget();

            Platform.position = Vector2.Lerp(Platform.position, target, speed * Time.deltaTime);

            float distance = (target - (Vector2)Platform.position).magnitude;

            if (distance <= 0.1f)
            {
                direction *= -1;
            }

        }
        

    }

    Vector2 currentMovementTarget()
    {
        if (direction == 1)
        {
            return StartPoint.position;
        }
        else
        {
            return EndPoint.position;
        }
    }


    private void OnDrawGizmos()
    {
        if (Platform != null && StartPoint != null && EndPoint != null)
        {
            Gizmos.DrawLine(Platform.transform.position, StartPoint.position);
            Gizmos.DrawLine(Platform.transform.position, EndPoint.position);

        }
    }
}
