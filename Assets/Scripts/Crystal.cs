using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public AudioClip crystalSound; // Kristalin sesi
    private AudioSource audioSource;
    private MelodyPuzzle melodyPuzzle; // Melodi bulmaca scripti referansý

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        melodyPuzzle = FindObjectOfType<MelodyPuzzle>(); // MelodyPuzzle scriptini bulur
    }

    public void OnClick()
    {
        if (melodyPuzzle.canClick)
        {
            PlaySound();
            melodyPuzzle.AddToPlayerSequence(crystalSound); // Oyuncunun sýrasýna sesi ekler
        }
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(crystalSound);
    }
}
