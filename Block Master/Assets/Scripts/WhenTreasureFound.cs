using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhenTreasureFound : MonoBehaviour
{
    public GameObject treasure;
    private AudioSource sound;
    private float timeWon;
    private bool soundTrigger;

    // Start is called before the first frame update
    void Start()
    {
        timeWon = 0.0f;
        sound = treasure.GetComponent<AudioSource>();
        soundTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!soundTrigger && Time.time - timeWon > 2.0f)
            SceneManager.LoadScene("WonGameMenu");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Claire")
        {
            if (soundTrigger)
            {
                timeWon = Time.time;
                sound.Play();
                soundTrigger = false;
            }
        }
    }
}
