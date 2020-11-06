using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public GameManager gameManager;
    public AudioSource audioSource;

    public AudioClip audioClip;
    public AudioClip audioClipDead;

    void Start ()
    {
        gameManager.OnStar += OnStar;
        gameManager.OnDisableStar += OnDisableStar;
        gameManager.OnDead += OnDead;

        audioSource.clip = audioClip;
        audioSource.Play ();
    }

    public void OnStar ()
    {
        audioSource.pitch = 1.5f;
    }

    public void OnDisableStar ()
    {
        audioSource.pitch = 1f;
    }

    public void OnDead ()
    {
        audioSource.clip = audioClipDead;
        audioSource.Play ();
    }
}
