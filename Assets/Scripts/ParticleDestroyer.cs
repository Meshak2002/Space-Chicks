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
            delay = ps.startLifetime + ps.duration;
            StartCoroutine(wait(delay));
        }
    }

    IEnumerator wait(float delay)
    {
        yield return new WaitForSeconds(delay);
        PoolManager.instance.poolDestroyObj(this.gameObject);
    }

}
