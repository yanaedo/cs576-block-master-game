using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDialogs : MonoBehaviour
{
    private GameObject instructionsDialog;

    // Start is called before the first frame update
    void Start()
    {
        instructionsDialog = GameObject.Find("InstructionsContainer");
        instructionsDialog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showDialog(GameObject dialog)
    {
        dialog.SetActive(!dialog.activeSelf);
    }
}
