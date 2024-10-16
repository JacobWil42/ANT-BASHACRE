using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private string newGameLevel = "Floor1";
    public void StartButton()
    {
        SceneManager.LoadScene(newGameLevel);
    }
}
