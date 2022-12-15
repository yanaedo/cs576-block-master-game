using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public AudioMixer audioMix;
    public GameObject fullscreenToggle;
    private GameObject instructionsDialog;
    private GameObject settingsDialog;

    // Start is called before the first frame update
    void Start()
    {
        instructionsDialog = GameObject.Find("InstructionsContainer");
        instructionsDialog.SetActive(false);
        settingsDialog = GameObject.Find("SettingsContainer");
        settingsDialog.SetActive(false);
        UnityEngine.UI.Toggle toggle = fullscreenToggle.GetComponent<UnityEngine.UI.Toggle>();

        ColorBlock colors = toggle.colors;
            colors.normalColor = new Color(255, 174, 47, 1);
            toggle.colors = colors;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showDialog(GameObject dialog)
    {
        dialog.SetActive(!dialog.activeSelf);
    }

    public void setBackgroundVolume(float volume)
    {
        audioMix.SetFloat("backgroundMusicVolume", volume);
    }

    public void setGameSoundsVolume(float volume)
    {
        audioMix.SetFloat("gameSoundsVolume", volume);
    }

    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void changeButtonColor()
    {
        /*ColorBlock colors = fullscreenToggle.colors;

        if (fullscreenToggle.isOn)
        {
            colors.normalColor = new Color(255, 174, 47);
            fullscreenToggle.colors = colors;
        }
        else
        {
            colors.normalColor = new Color(255, 255, 255, 0.6f);
            fullscreenToggle.colors = colors;
        }*/
    }

    public void exitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
