using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    Vector2 startPosition;
    Vector2 targetPosition;

    float randomFloat;

    public float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    private bool isMove = false;

    void Start ()
    {
        targetPosition = transform.position;

        if (Random.Range (0, 2) == 0) {
            randomFloat = -10;
        } else {
            randomFloat = 10;
        }

        startPosition = new Vector2 (targetPosition.x + randomFloat, targetPosition.y);

        transform.position = startPosition;
    }

    void Update ()
    {
        if (isMove && Vector2.Distance (targetPosition, transform.position) < 0.3f) {
            var newPosX = targetPosition.x > 0 ? -4 : 4;
            targetPosition = new Vector2 (newPosX, transform.position.y);
        } else if (Vector2.Distance (targetPosition, transform.position) > 0.01f) {
            MoveToTargetPosition ();
        }
    }

    void MoveToTargetPosition ()
    {
        transform.position = Vector3.SmoothDamp (transform.position, targetPosition, ref velocity, smoothTime);

    }

    public void StartMove ()
    {
        isMove = true;
    }

}
