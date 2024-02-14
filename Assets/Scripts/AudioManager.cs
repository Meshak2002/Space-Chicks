using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioBloodSplash, audioChickEgg, audioSmallExplosion,
                                       audioBigExplosion, audioBulletImpact, audioPickup, audioBite;
    public static AudioManager instance;

    private AudioSource audioSourceEat, audioSourcePickup, audioSourcePBullet, audioSourceEgg;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;


        audioSourceEat = transform.GetChild(0).GetComponent<AudioSource>();
        audioSourcePickup = transform.GetChild(1).GetComponent<AudioSource>();
        audioSourcePBullet = transform.GetChild(2).GetComponent<AudioSource>();
        audioSourceEgg = transform.GetChild(3).GetComponent<AudioSource>();
    }

    public void EatSFxPlay()
    {
        audioSourceEat.Play();
    }
    
    public void PickSFxPlay()
    {
        audioSourcePickup.Play();
    }
    public void PBulletSFxPlay()
    {
        audioSourcePBullet.Play();
    }

    public void EggSFxPlay()
    {
        audioSourceEgg.Play();
    }
}