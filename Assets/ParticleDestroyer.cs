using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    private ParticleSystem ps;
    private float delay;

    private void OnEnable()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if(ps != null)
        {
            delay = ps.startLifetime + ps.duration;
            Destroy(gameObject, delay);
        }
    }

}
