using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
    public void PlayFootstepSound()
    {
        SoundManager.PlaySound(SoundType.Footsteps);
    }
}
