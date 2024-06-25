using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    public float speed = 20f;
    public float degreesPerSec = 360f;
    public Grapple target;
    public GameObject smallExplosionPrefab;

    private void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
        float rotAmount = degreesPerSec * Time.deltaTime;
        if (rotAmount > 360)
        {
            rotAmount -= 360;
        }
        float curRot = transform.localRotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));
        if (Vector3.Distance(this.transform.position, target.transform.position) < 0.1f)
        {
            SpawnExplosion();
            target.OnDynamite();
            Destroy(this.gameObject);
        }
    }

    private void SpawnExplosion()
    {
        GameObject o = Instantiate(smallExplosionPrefab, this.transform.position, Quaternion.identity, GameObject.Find("Explosions").transform);
        o.GetComponent<Explosion>().range = 0;
    }
}
