using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxslyth : MonoBehaviour
{
    private AudioSource MainAudio;
    [SerializeField] private AudioClip[] audioClip;
    // Start is called before the first frame update
    void Start()
    {
        MainAudio = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MainAudio.clip = audioClip[1];
            MainAudio.Play();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MainAudio.clip = audioClip[0];
            MainAudio.Play();
        }
    }
}
