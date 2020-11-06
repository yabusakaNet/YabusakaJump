using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioClip audioClipDead;
    private AudioSource audioSource;

    GameManager gameManager;
    Player player;

    void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play ();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (gameManager.isStar)
        {
            audioSource.pitch = 1.5f;
        }
        else
        {
            audioSource.pitch = 1f;
        }
    }
}
