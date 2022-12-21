using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationDialog : MonoBehaviour
{
    private GameObject confirmationBox;

    // Start is called before the first frame update
    void Start()
    {
        confirmationBox = GameObject.Find("ConfirmationContainer");
        confirmationBox.SetActive(false);     
    }

    public void showDialog(GameObject dialog)
    {
        dialog.SetActive(!dialog.activeSelf);
    }
}
