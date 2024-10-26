using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameController : MonoBehaviour
{
    [SerializeField] private string startLocation = "floor1"; // Renamed to follow conventions
    private AudioSource audioSource; // Declare audioSource

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on this GameObject!");
        }
    }

    public void StartButton()
    {
        audioSource.Play();
        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(audioSource.clip.length); // Wait for the audio clip duration + 5 seconds
        SceneManager.LoadScene(startLocation);
        
    }
}
