using UnityEngine;

public class SpecialPower : MonoBehaviour
{
    protected bool once;
    protected float timer;

    protected CircleCollider2D cirCol;
    protected SpriteRenderer spriteRenderer;

    public void ActivatePower(ref bool bPower, ref float startDuration)
    {
        once = true;
        AudioManager.instance.PickSFxPlay();
        startDuration += Time.time;
        bPower = true;
        GameManager.instance.power.SetActive(true);
        spriteRenderer.enabled = false;
        cirCol.enabled = false;
    }

    public void LossPower(ref bool bPower)            //Called this after the duration of power of shield is finished
    {
        bPower = false;
        once = false;
        GameManager.instance.power.SetActive(false);
        PoolManager.instance.PoolDestroyObj(this.gameObject);
    }

    public void BoundaryCheck()
    {
        if (transform.position.y < -4.5f)    // Check if the piece's y position is below the lower boundary
        {
            PoolManager.instance.PoolDestroyObj(this.gameObject);
        }
    }
}
