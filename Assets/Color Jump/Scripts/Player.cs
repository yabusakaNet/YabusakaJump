using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    bool isDragging = false;

    Vector2 TouchPosition;
    Vector2 PlayerPosition;
    Vector2 DragPosition;

    float RightEnd;
    float LeftEnd;
    float Height;

    public GameObject FX_Jump;
    public GameObject FX_StepDestory;
    public GameObject FX_Dead;

    StepManager stepManager;
    GameManager gameManager;

    bool isStart = false;
    bool isDead = false;

    AudioSource source;
    public AudioClip JumpClip;
    public AudioClip DeadClip;
    public AudioClip CoinClip;

    float JumpVelocity;

    public float gravity;
    public float maxGravity;
    public float gravityIncrease;

    public Transform character;

    public Animator playerAnimator;

    int lastJumpStepIndex = 0;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D> ();
        source = GetComponent<AudioSource> ();

        gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
        stepManager = GameObject.Find ("StepManager").GetComponent<StepManager> ();

        RightEnd = GameObject.Find ("GameManager").GetComponent<DisplayManager> ().RIGHT;
        LeftEnd = GameObject.Find ("GameManager").GetComponent<DisplayManager> ().LEFT;
        Height = GameObject.Find ("GameManager").GetComponent<DisplayManager> ().HEIGHT;

        playerAnimator.Play ("Character@Stand", 0, 0f);
    }

    void Update ()
    {
        WaitToTouch ();
        if (!isStart) return;
        if (isDead) return;

        GetInput ();
        MovePlayer ();
        AddGravityToPlayer ();

        DeadJudgement ();
    }

    void WaitToTouch ()
    {
        if (!isStart) {
            if (Input.GetMouseButtonDown (0)) {
                if (!isStart) isStart = true;
                gameManager.StartGame ();
            }
        }
    }

    void DeadJudgement ()
    {
        if (isDead == false && Camera.main.transform.position.y - transform.position.y > Height / 2) {
            isDead = true;
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            GameOver ();
        }
    }

    void GameOver ()
    {
        GameObject effectObj = Instantiate (FX_Dead, transform.position, Quaternion.identity);
        gameManager.GameOver ();
        source.PlayOneShot (DeadClip, 1);
    }

    void GetInput ()
    {
        if (Input.GetMouseButtonDown (0)) {
            if (!isStart) isStart = true;

            isDragging = true;
            TouchPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10));
            PlayerPosition = transform.position;

        } else if ((Input.GetMouseButtonUp (0))) {
            isDragging = false;
        }
    }

    void MovePlayer ()
    {
        if (isDragging == true) {
            DragPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10));
            var beforPosX = transform.position.x;
            transform.position = new Vector3 (PlayerPosition.x + (DragPosition.x - TouchPosition.x) * 1.5f, transform.position.y);
            var afterPosX = transform.position.x;

            if (beforPosX < afterPosX) {
                character.rotation = Quaternion.Euler (0f, 180f, 0f);
            } else if (beforPosX > afterPosX) {
                character.rotation = Quaternion.Euler (0f, 0f, 0f);
            }

            if (transform.position.x < LeftEnd)
                transform.position = new Vector3 (LeftEnd, transform.position.y);
            if (transform.position.x > RightEnd)
                transform.position = new Vector3 (RightEnd, transform.position.y);
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.tag == "Step") {
            if (other.gameObject.name == "Step") {
                return;
            }
            if (rb.velocity.y <= 0 || gameManager.isStar) {
                Jump (other);
            }
        } else if (other.gameObject.tag == "DummyStep") {
            if (other.gameObject.name == "DummyStep") {
                return;
            }
            if (gameManager.isStar) {
                Jump (other);
            } else if (rb.velocity.y <= 0) {
                isDead = true;
                rb.isKinematic = true;
                rb.velocity = Vector2.zero;
                GameOver ();
            }
        } else if (other.gameObject.tag == "Coin") {
            gameManager.AddScore (1);
            source.PlayOneShot (CoinClip, 1);
            DestroyItem (other);
        } else if (other.gameObject.tag == "Star") {
            gameManager.StartStar ();
            DestroyItem (other);
        }
    }

    void Jump (Collider2D step)
    {
        var stepIndex = int.Parse (step.gameObject.name);

        JumpVelocity = gravity * (gameManager.isStar ? 30f : 26f);
        rb.velocity = new Vector2 (0, JumpVelocity);
        playerAnimator.Play ("Character@Jump", 0, 0f);

        Effect (step);
        Destroy (step.gameObject);
        stepManager.MakeNewStep ();
        if (lastJumpStepIndex != stepIndex - 1) {
            stepManager.MakeNewStep ();
        }

        IncreaseGravity ();

        gameManager.AddScore (1);

        source.PlayOneShot (JumpClip, 1);

        lastJumpStepIndex = int.Parse (step.gameObject.name);
    }

    void Effect (Collider2D step)
    {
        GameObject jumpEffect = Instantiate (FX_Jump, transform.position, Quaternion.identity);
        Destroy (jumpEffect, 1.0f);

        GameObject stepDestroyEffect = Instantiate (FX_StepDestory, step.gameObject.transform.position, Quaternion.identity);
        Destroy (stepDestroyEffect, 0.5f);
    }

    void DestroyItem (Collider2D item)
    {
        Destroy (item.gameObject);
    }

    void AddGravityToPlayer ()
    {
        rb.velocity = new Vector2 (0, rb.velocity.y - (gravity * gravity));
    }

    void IncreaseGravity ()
    {
        gravity += gravityIncrease;
        if (gravity > maxGravity) gravity = maxGravity;
    }

}
