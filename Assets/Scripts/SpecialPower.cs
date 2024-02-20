using UnityEngine;

public class SpecialPower : MonoBehaviour
{
    protected bool once;
    protected float timer;

    protected CircleCollider2D cirCol;
    protected SpriteRenderer spriteRenderer;

    public void ActivatePower(ref bool bPower, ref GameObject objPower, ref float startDuration)
    {
        once = true;
        AudioManager.instance.PickSFxPlay();
        startDuration += Time.time;
        bPower = true;
        objPower.SetActive(true);
        spriteRenderer.enabled = false;
        cirCol.enabled = false;
    }

    public void LossPower(ref bool bPower, ref GameObject objPower)            //Called this after the duration of power of shield is finished
    {
        bPower = false;
        once = false;
        objPower.SetActive(false);
        PoolManager.instance.PoolDestroyObj(this.gameObject);
    }
}
