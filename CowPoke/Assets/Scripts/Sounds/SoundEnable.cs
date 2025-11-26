using Unity.VisualScripting;
using UnityEngine;

public class SoundEnable : MonoBehaviour
{
    public AudioSource sound;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            sound.enabled = true;
        }
        else
        {
            sound.enabled = false;
        }
    }
}
