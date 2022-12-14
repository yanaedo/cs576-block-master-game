using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject instructionsDialog;
    private GameObject settingsDialog;

    // Start is called before the first frame update
    void Start()
    {
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

    public void setVolume(float volume)
    {

    }

    public void exitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
