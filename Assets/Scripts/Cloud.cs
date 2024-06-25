using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float leftSceneBound;
    public float rightSceneBound;
    public bool moveRight = true;
    public float speed;

    private bool initialized = false;

    public void Initialise(float leftSceneBound, float rightSceneBound, float speed)
    {
        this.leftSceneBound = leftSceneBound;
        this.rightSceneBound = rightSceneBound;
        this.speed = speed;
        this.initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
            if (moveRight)
            {
                Vector3 direction = new Vector3(1, 0, 0);
                this.transform.position += Time.deltaTime * speed * direction;
                if (this.transform.position.x > rightSceneBound)
                {
                    this.transform.position = new Vector3(leftSceneBound, this.transform.position.y, 0);
                }
            }
            else
            {
                Vector3 direction = new Vector3(-1, 0, 0);
                this.transform.position += Time.deltaTime * speed * direction;
                if (this.transform.position.x < leftSceneBound)
                {
                    this.transform.position = new Vector3(rightSceneBound, this.transform.position.y, 0);
                }
            }
        }
    }
}
