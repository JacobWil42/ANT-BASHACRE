using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private string playerSelectScreen = "PlayerSelect";
    public void StartButton()
    {
        SceneManager.LoadScene(playerSelectScreen);
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on this GameObject!");
            return;
        }
        audioSource.Play(); // Play the sound
    }
}
