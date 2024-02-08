using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BG_Scroller : MonoBehaviour
{
    // Start is called before the first frame update
    private RawImage rImg;
    [SerializeField] private Vector2 movVelocity;

    private void OnEnable()
    {
        rImg = GetComponent<RawImage>();    
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( rImg != null )
        {
            rImg.uvRect = new Rect(rImg.uvRect.position + movVelocity * Time.deltaTime, rImg.uvRect.size);
        }
    }
}
