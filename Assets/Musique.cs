using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musique : MonoBehaviour
{
    public AudioListener Player;

    public BoxCollider2D ColliderFallend;
    [SerializeField] private AudioSource MusiqueSource;
    [SerializeField] private AudioClip[] MusiqueClip;
    // Start is called before the first frame update
    void Start()
    {
        MusiqueSource.clip = MusiqueClip[0];
        MusiqueSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            MusiqueSource.clip = MusiqueClip[1];
            MusiqueSource.Play();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MusiqueSource.clip = MusiqueClip[0];
            MusiqueSource.Play();
        }
    }
}
