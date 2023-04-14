using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityComponentManager : MonoBehaviour
{
    public CapsuleCollider2D physicalCollision;
    public Collider2D headCollision;
    public Collider2D feetCollision;
    public SpriteRenderer headSR;
    [SerializeField]
    private ParticleSystem feetParticles;

    public void SetFeetParticles(bool _activated)
    {
        if (_activated)
        {
            feetParticles.Play();
        }
        else
        {
            feetParticles.Stop();
        }


    }
}
