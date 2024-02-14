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
        // Ensure only one instance of AudioManager exists
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Get audio sources from child objects
        audioSourceEat = transform.GetChild(0).GetComponent<AudioSource>();
        audioSourcePickup = transform.GetChild(1).GetComponent<AudioSource>();
        audioSourcePBullet = transform.GetChild(2).GetComponent<AudioSource>();
        audioSourceEgg = transform.GetChild(3).GetComponent<AudioSource>();
    }

    // Method to play eating sound effect
    public void EatSFxPlay()
    {
        audioSourceEat.Play();
    }

    // Method to play pickup sound effect
    public void PickSFxPlay()
    {
        audioSourcePickup.Play();
    }

    // Method to play bullet sound effect
    public void PBulletSFxPlay()
    {
        audioSourcePBullet.Play();
    }

    // Method to play egg laying sound effect
    public void EggSFxPlay()
    {
        audioSourceEgg.Play();
    }
}