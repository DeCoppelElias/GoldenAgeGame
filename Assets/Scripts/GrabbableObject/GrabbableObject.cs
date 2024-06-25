using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrabbableObject : MonoBehaviour
{
    public int size;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Grapple grapple = collision.transform.GetComponent<Grapple>();
        if (grapple != null)
        {
            this.OnTriggerEnter2DCustom(grapple);
        }
    }
    protected abstract void OnTriggerEnter2DCustom(Grapple grapple);
}
