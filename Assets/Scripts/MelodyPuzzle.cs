using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MelodyPuzzle : MonoBehaviour
{
    public List<AudioClip> melodySequence; // Melodi s�ras�
    private List<AudioClip> currentSequence = new List<AudioClip>(); // Ge�erli melodi s�ras�
    private List<AudioClip> playerSequence = new List<AudioClip>(); // Oyuncunun girdi�i s�ra
    public List<Crystal> crystals; // Sahnedeki kristaller
    public GameObject startButton; // Ba�lama butonu
    public TMP_Text resultText; // Sonu� metni
    public int currentMelodyIndex = 0; // Ge�erli melodi indeksi
    public bool canClick = false; // Oyuncunun t�klay�p t�klayamayaca��

    void Start()
    {
        startButton.GetComponent<Collider>().enabled = true; // Ba�lama butonunu etkinle�tir
        resultText.gameObject.SetActive(false); // Sonu� metnini gizler
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
                    StartPuzzle(); // Ba�lama butonuna t�klan�nca bulmacay� ba�lat�r
                }
                else
                {
                    Crystal crystal = hit.transform.GetComponent<Crystal>();
                    if (crystal != null)
                    {
                        crystal.OnClick(); // Kristale t�klan�nca ilgili i�lemi yapar
                    }
                }
            }
        }
    }

    void StartPuzzle()
    {
        startButton.GetComponent<Collider>().enabled = false; // Ba�lama butonunu devre d��� b�rak
        currentMelodyIndex = 0; // Melodi indeksini s�f�rlar
        PlayMelodySequence();
    }

    void PlayMelodySequence()
    {
        currentSequence.Clear(); // Ge�erli s�ray� temizler
        for (int i = 0; i <= currentMelodyIndex; i++)
        {
            currentSequence.Add(melodySequence[i]); // Melodi s�ras�n� ekler
        }
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        canClick = false; // Oyuncu t�klayamaz
        foreach (var clip in currentSequence)
        {
            foreach (var crystal in crystals)
            {
                if (crystal.crystalSound == clip)
                {
                    crystal.PlaySound();
                    yield return new WaitForSeconds(clip.length + 0.5f); // Ses uzunlu�u kadar bekler
                }
            }
        }
        canClick = true; // Oyuncu t�klayabilir
        playerSequence.Clear(); // Oyuncunun s�ras�n� temizler
    }

    public void AddToPlayerSequence(AudioClip clip)
    {
        playerSequence.Add(clip); // Oyuncunun s�ras�na sesi ekler
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
                StartCoroutine(PlaySequence()); // Yanl��sa tekrar �alar
                return;
            }
        }
        currentMelodyIndex++; // Do�ruysa bir sonraki melodiyi �alar
        if (currentMelodyIndex == melodySequence.Count) // T�m melodileri do�ru bildikten sonra biter
        {
            EndPuzzle();
        }
        else
        {
            PlayMelodySequence();
        }
    }

    void EndPuzzle()
    {
        resultText.gameObject.SetActive(true); // Sonu� metnini g�sterir
        resultText.text = "Tebrikler, bulmacay� tamamlad�n�z!";
    }
}
