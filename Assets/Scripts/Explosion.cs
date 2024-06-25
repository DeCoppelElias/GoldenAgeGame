using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Explosion : MonoBehaviour
{
    private float destroyDelay = 1;
    public int range;
    private Light2D spotLight;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        TriggerOtherTNTBarrels();
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + destroyDelay);
        this.startTime = Time.time;
        this.spotLight = this.GetComponentInChildren<Light2D>();
    }

    private void TriggerOtherTNTBarrels()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(new Vector2(this.transform.position.x, this.transform.position.y), range, new Vector2(0, 0));
        foreach (RaycastHit2D hit in hits)
        {
            TNTBarrel tntBarrel = hit.transform.GetComponent<TNTBarrel>();
            if (tntBarrel != null)
            {
                StartCoroutine(ExplodeOtherTNTBarrel(tntBarrel));
            }
        }
    }

    private void Update()
    {
        float percentage = (Time.time - startTime) / this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        float intensity = Mathf.Clamp(1 - percentage, 0f, 0.8f);
        this.spotLight.intensity = intensity;
    }

    IEnumerator ExplodeOtherTNTBarrel(TNTBarrel tntBarrel)
    {
        yield return new WaitForSeconds(0.2f);
        if (tntBarrel != null) tntBarrel.Explode();
    }
}
