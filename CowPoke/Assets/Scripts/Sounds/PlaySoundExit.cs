using UnityEngine;

public class PlaySoundExit : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;
    [SerializeField, Range(0, 1)] private float volume = 1f;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.PlaySound(sound, volume);
    }
}
