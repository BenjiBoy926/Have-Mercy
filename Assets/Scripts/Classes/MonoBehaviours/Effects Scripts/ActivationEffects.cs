using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS ActivationEffects : MinionEffects
 * ---------------------------------------
 * A child class of the class that manages minion effects.
 * Used by minions that have additional activated actions
 * like spawning and shooting
 * ---------------------------------------
 */ 

public class ActivationEffects : MinionEffects
{
    [SerializeField]
    private List<AudioClip> actClips;   // A clip is randomly selected to play when the minion does an action

    // Use minion's animator to produce the effect
    public void PrepareAct ()
    {
        anim.SetTrigger("prepareAct");
    }

    // Use minion's animator to produce the effect
    public void Act ()
    {
        anim.SetTrigger("act");
        SoundPlayer.PlayRandomEffect(actClips, audioSource);
    }

    // Prep act sound placed in a seperate function 
    // and called as an event through the corresponding animation trigger event
    public void ActSound ()
    {
        SoundPlayer.PlayRandomEffect(returnClips, audioSource);
    }
}
