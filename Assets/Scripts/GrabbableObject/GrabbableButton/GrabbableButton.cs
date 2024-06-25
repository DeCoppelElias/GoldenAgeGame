using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrabbableButton : GrabbableObject
{
    public int duration = 2;
    protected Transform slider;
    protected List<NormalGrapple> grapples = new List<NormalGrapple>();

    private void Start()
    {
        this.slider = this.transform.Find("Slider");
    }
    protected override void OnTriggerEnter2DCustom(Grapple grapple)
    {
        if (grapple is NormalGrapple normalGrapple)
        {
            normalGrapple.OnGrabButton(this);
            this.grapples.Add(normalGrapple);
        }
    }

    protected virtual bool ButtonCondition()
    {
        return true;
    }
    protected abstract void ButtonEffect();

    private void Update()
    {
        this.ButtonUpdate();
    }

    public abstract void ButtonUpdate();

    public void ReleaseButton(NormalGrapple grapple)
    {
        slider.localScale = new Vector3(0, 1, 1);
        this.grapples.Remove(grapple);
    }
}
