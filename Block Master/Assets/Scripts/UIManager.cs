using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public AudioMixer audioMix;
    private GameObject instructionsDialog;
    private GameObject settingsDialog;
    private GameObject introIntructionsBox;
    private GameObject claireControlsBox;
    private GameObject keysControlsBox;

    // Start is called before the first frame update
    void Start()
    {
        //find instructions and settings dialog boxes and hide them in the scene immediately
        introIntructionsBox = GameObject.Find("InstructionsBox");
        claireControlsBox = GameObject.Find("ClaireControlsBox");
        claireControlsBox.SetActive(false);
        keysControlsBox = GameObject.Find("KeysControlsBox");
        keysControlsBox.SetActive(false);
        instructionsDialog = GameObject.Find("InstructionsContainer");
        instructionsDialog.SetActive(false);
        settingsDialog = GameObject.Find("SettingsContainer");
        settingsDialog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showDialog(GameObject dialog)
    {
        dialog.SetActive(!dialog.activeSelf);
    }

    public void pageTurnerNext(GameObject curPage)
    {
            if (curPage.name == "InstructionsBox")
            {
                curPage.SetActive(false);
                //GameObject nextPage = GameObject.Find("ClaireControlsBox");
                claireControlsBox.SetActive(true);
            } 
            else if (curPage.name == "ClaireControlsBox")
            {
                curPage.SetActive(false);
                //GameObject nextPage = GameObject.Find("KeysControlsBox");
                keysControlsBox.SetActive(true);
            }
    }

    public void pageTurnerPrev(GameObject curPage)
    {
        if (curPage.name == "ClaireControlsBox")
        {
            curPage.SetActive(false);
            //GameObject prevPage = GameObject.Find("InstructionsBox");
            introIntructionsBox.SetActive(true);
        }
        else if (curPage.name == "KeysControlsBox")
        {
            curPage.SetActive(false);
            //GameObject prevPage = GameObject.Find("ClaireControlsBox");
            claireControlsBox.SetActive(true);
        }
    }

public void setBackgroundVolume(float volume)
    {
        audioMix.SetFloat("backgroundMusicVolume", volume);
    }

    public void setGameSoundsVolume(float volume)
    {
        audioMix.SetFloat("gameSoundsVolume", volume);
    }

    public void setQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void exitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
