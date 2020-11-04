using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    public float offset;
    public float smoothTime = 0.5f;
    public float limitPosY;
    public bool isLoop = false;

    bool isMove = false;
    Vector3 velocity = Vector3.zero;

    private void Start ()
    {
        isMove = false;
    }

    void Update ()
    {
        if (!isMove) {
            return;
        }

        var targetPos = new Vector3 (transform.position.x, transform.position.y + offset, transform.position.z);
        transform.position = Vector3.SmoothDamp (transform.position, targetPos, ref velocity, smoothTime);

        if (transform.localPosition.y <= limitPosY) {
            if (isLoop) {
                transform.localPosition = Vector3.zero;
            } else {
                isMove = false;
            }
        }
    }

    public void StartMove ()
    {
        isMove = true;
    }

    public void StopMove ()
    {
        isMove = false;
    }
}
