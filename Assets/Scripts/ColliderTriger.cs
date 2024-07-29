using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColliderTriger : MonoBehaviour
{

    [SerializeField] private GameObject spaceshipCube;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Geçici olarak "NextScene" sahnesine geçiyoruz.
            SceneManager.LoadScene("Last");
        }
    }
}
