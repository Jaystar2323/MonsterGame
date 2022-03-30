using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    private void Start()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            this.GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            this.GetComponent<AudioSource>().enabled = false;
        }
    }
    public void changeMusicStatus()
    {
        this.GetComponent<AudioSource>().enabled = !this.GetComponent<AudioSource>().enabled;
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            PlayerPrefs.SetInt("Music", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
        }
    }
}
