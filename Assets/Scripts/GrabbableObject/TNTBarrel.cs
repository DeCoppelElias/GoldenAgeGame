using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTBarrel : GrabbableObject
{
    public int range = 5;
    public GameObject explosion;
    protected override void OnTriggerEnter2DCustom(Grapple grapple)
    {
        Explode();
        grapple.OnGrabEffect();
    }

    public void Explode()
    {
        SpawnExplosion();
        RaycastHit2D[] hits = Physics2D.CircleCastAll(new Vector2(this.transform.position.x, this.transform.position.y), range, new Vector2(0,0));
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.GetComponent<Pickup>())
            {
                Destroy(hit.transform.gameObject);
            }
        }
        Destroy(this.gameObject);
    }

    private void SpawnExplosion()
    {
        GameObject o = Instantiate(explosion, this.transform.position, Quaternion.identity, GameObject.Find("Explosions").transform);
        o.GetComponent<Explosion>().range = range;
    }
}
