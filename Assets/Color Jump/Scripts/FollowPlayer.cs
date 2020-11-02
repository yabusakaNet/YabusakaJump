
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public GameObject PlayerObj;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    public Transform BackTransform;
    public Transform ForegroundTransform;

    public int yOffset;
    public float yOffsetBack;
    public float yOffsetForeground;

    void Update ()
    {
        Vector3 targetPosition = PlayerObj.transform.TransformPoint (new Vector3 (0, yOffset, -10));

        if (targetPosition.y < transform.position.y) return;

        var diffY = targetPosition.y - transform.position.y;

        targetPosition = new Vector3 (0, targetPosition.y, targetPosition.z);
        transform.position = Vector3.SmoothDamp (transform.position, targetPosition, ref velocity, smoothTime);

        if (diffY < 1f) {
            return;
        }

        var backTargetPosition = new Vector3 (0, BackTransform.position.y + diffY + yOffsetBack, BackTransform.position.z);
        BackTransform.position = Vector3.SmoothDamp (BackTransform.position, backTargetPosition, ref velocity, smoothTime);

        var foregroundTargetPosition = new Vector3 (0, ForegroundTransform.position.y + diffY + yOffsetForeground, ForegroundTransform.position.z);
        ForegroundTransform.position = Vector3.SmoothDamp (ForegroundTransform.position, foregroundTargetPosition, ref velocity, smoothTime);
    }

}