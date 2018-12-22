using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CLASS BulletEffects : MonoBehaviour
 * -----------------------------------
 * Provides visual/audio effects for any bullet
 * -----------------------------------
 */ 

public class BulletEffects : MonoBehaviour, IActivateable
{
    [SerializeField]
    private GameObject collisionParticle;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private float particleTime; // Time given for the particle to display before disabling the bullet
    [SerializeField]
    private List<AudioClip> hitClips;   // A clip from the list is randomly played when the bullet hits an object

    private bool isActive;
    public bool IsActive { get { return isActive; } }
    public float ParticleTime { get { return particleTime; } }

    public void Collision ()
    {
        collisionParticle.SetActive(false);
        collisionParticle.SetActive(true);

        if (hitClips.Count > 0)
        {
            SoundPlayer.PlayRandomEffect(hitClips, audioSource);
        }
    }

    public void Activate (bool active)
    {
        isActive = active;

        if (active)
        {
            collisionParticle.SetActive(false);
        }
    }
}
