using System.Collections;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    private ParticleSystem ps;
    private float delay;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        if(ps != null)
        {
            delay = ps.startLifetime + ps.duration;     // Calculate delay based on particle system's start lifetime and duration
            StartCoroutine(wait(delay));
        }
    }

    IEnumerator wait(float delay)
    {
        yield return new WaitForSeconds(delay);
        PoolManager.instance.PoolDestroyObj(this.gameObject);        // Coroutine to wait for a specified delay before destroying the object
    }

}
