using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; set; }

    public AudioSource shootingChannel;
   

    public AudioClip P1911Shot;
    public AudioClip M16Shot;

    public AudioSource reloadingSoundM1911;
    public AudioSource emptyMagSoundM1911;
    public AudioSource reloadingSoundM107;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch(weapon)
        {
            case WeaponModel.pistolM1911:
                shootingChannel.PlayOneShot(P1911Shot);
                break;
            case WeaponModel.M107:
                shootingChannel.PlayOneShot(M16Shot);
                break;

        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.pistolM1911:
                reloadingSoundM1911.Play();
                break;
            case WeaponModel.M107:
                reloadingSoundM107.Play();
                break;
        }
    }
}
