using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class MenuSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip spotlightSound;
    [SerializeField]
    private AudioClip holySound;

    private AudioClip hoverSound;
    private AudioClip clickSound;

    private void Awake()
    {
        hoverSound = Resources.Load("Sounds/Hover") as AudioClip;
        clickSound = Resources.Load("Sounds/Click") as AudioClip;
    }
    public void PlaySpotlightSound(float _volume)
    {
        AudioManager._instance.Play2dOneShotSound(spotlightSound, 0.75f, 1.25f, _volume);
    }

    public void PlayHolySound()
    {
        AudioManager._instance.Play2dOneShotSound(holySound);
    }

    public void PlayHover()
    {
        AudioManager._instance.Play2dOneShotSound(hoverSound);
    }

    public void PlayClick()
    {
        AudioManager._instance.Play2dOneShotSound(clickSound);
    }
}
