using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSelect : MonoBehaviour
{
    
    [SerializeField] private AudioSource audioSourceJohn;
    [SerializeField] private AudioSource audioSourceKount;
    [SerializeField] private AudioSource audioSourceJohnSr;

    [SerializeField]
    public static string character = "John Basch";

    [SerializeField]
    private TextMeshProUGUI characterNameText;

    public void SelectJohn()
    {
        character = "John Basch";
        UpdateCharacterName();
        audioSourceJohn.Play();
    }

    public void SelectJohnSr()
    {
        character = "SelectJohnSr";
        UpdateCharacterName();
        audioSourceJohnSr.Play();
    }

    public void SelectKountBashyoula()
    {
        character = "Kount Bashyoula";
        UpdateCharacterName();
        audioSourceKount.Play();
    }

    public void SelectUnnamedCharacterOne()
    {
        character = "Unnamed Character One";
        UpdateCharacterName();
    }

    public void SelectUnnamedCharacterTwo()
    {
        character = "Unnamed Character Two";
        UpdateCharacterName();
    }

    public void SelectUnnamedCharacterThree()
    {
        character = "Unnamed Character Three";
        UpdateCharacterName();
    }

    public void SelectUnnamedCharacterFour()
    {
        character = "Unnamed Character Four";
        UpdateCharacterName();
    }

    private void UpdateCharacterName()
    {
        if (characterNameText != null)
        {
            characterNameText.text = character;
            Debug.Log("Character name updated to: " + character);
        }
        else
        {
            Debug.LogError("characterNameText is not assigned in the Inspector!");
        }
    }
}
