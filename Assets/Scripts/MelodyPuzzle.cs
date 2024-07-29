using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MelodyPuzzle : MonoBehaviour
{
    public List<AudioClip> melodySequence; // Melodi sýrasý
    private List<AudioClip> currentSequence = new List<AudioClip>(); // Geçerli melodi sýrasý
    private List<AudioClip> playerSequence = new List<AudioClip>(); // Oyuncunun girdiði sýra
    public List<Crystal> crystals; // Sahnedeki kristaller
    public AudioSource audioSource;
    public GameObject startButton; // Baþlama butonu
    public TMP_Text resultText; // Sonuç metni
    public int currentMelodyIndex = 0; // Geçerli melodi indeksi
    public bool canClick = false; // Oyuncunun týklayýp týklayamayacaðý

    void Start()
    {
        startButton.GetComponent<Collider>().enabled = true; // Baþlama butonunu etkinleþtir
        resultText.gameObject.SetActive(false); // Sonuç metnini gizler
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == startButton)
                {
                    StartPuzzle(); // Baþlama butonuna týklanýnca bulmacayý baþlatýr
                }
                else
                {
                    Crystal crystal = hit.transform.GetComponent<Crystal>();
                    if (crystal != null)
                    {
                        crystal.OnClick(); // Kristale týklanýnca ilgili iþlemi yapar
                    }
                }
            }
        }
    }

    void StartPuzzle()
    {
        startButton.GetComponent<Collider>().enabled = false; // Baþlama butonunu devre dýþý býrak
        currentMelodyIndex = 0; // Melodi indeksini sýfýrlar
        PlayMelodySequence();
    }

    void PlayMelodySequence()
    {
        currentSequence.Clear(); // Geçerli sýrayý temizler
        currentSequence.Add(melodySequence[0]);
        for (int i = 1; i <= currentMelodyIndex; i++)
        {
            currentSequence.Add(melodySequence[i]); // Melodi sýrasýný ekler
        }
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        canClick = false; // Oyuncu týklayamaz
        foreach (var clip in currentSequence)
        {
            foreach (var crystal in crystals)
            {
                if (crystal.crystalSound == clip)
                {
                    crystal.PlaySound();
                    yield return new WaitForSeconds(clip.length); // Ses uzunluðu kadar bekler
                }
            }
        }
        canClick = true; // Oyuncu týklayabilir
        playerSequence.Clear(); // Oyuncunun sýrasýný temizler
    }

    public void AddToPlayerSequence(AudioClip clip)
    {
        playerSequence.Add(clip); // Oyuncunun sýrasýna sesi ekler
        if (playerSequence.Count == currentSequence.Count)
        {
            CheckSequence();
        }
    }

    void CheckSequence()
    {
        for (int i = 0; i < currentSequence.Count; i++)
        {
            if (playerSequence[i] != currentSequence[i])
            {
                StartCoroutine(PlaySequence()); // Yanlýþsa tekrar çalar
                return;
            }
        }
        StopAllSounds();
        currentMelodyIndex++; // Doðruysa bir sonraki melodiyi çalar
        if (currentMelodyIndex == melodySequence.Count) // Tüm melodileri doðru bildikten sonra biter
        {
            EndPuzzle();
        }
        else
        {
            StartCoroutine(WaitAndPlayNextMelody()); // Bekleme süresi eklenir
        }
    }

    void EndPuzzle()
    {
        resultText.gameObject.SetActive(true); // Sonuç metnini gösterir
        resultText.text = "Tebrikler, bulmacayý tamamladýnýz!";
    }

    void StopAllSounds()
    {
        audioSource.Stop();
    }

    IEnumerator WaitAndPlayNextMelody()
    {
        yield return new WaitForSeconds(3f); // 2 saniye bekler
        PlayMelodySequence();
    }
}
