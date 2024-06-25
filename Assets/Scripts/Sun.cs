using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sun : MonoBehaviour
{
    private Vector3 initialPosition;
    private bool sinking = false;
    private float sinkingDistance;
    private float sinkingTime;
    public float maxIntensity = 0.8f;
    public float minIntensity = 0.2f;
    private Light2D pointLight;

    private void Start()
    {
        this.initialPosition = transform.position;
        this.pointLight = GameObject.Find("AmbientLight").GetComponentInChildren<Light2D>();
        pointLight.intensity = maxIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (sinking)
        {
            Vector3 endPosition = this.transform.position + new Vector3(0, -this.sinkingDistance, 0);
            float sinkingSpeed = this.sinkingDistance / this.sinkingTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, endPosition, sinkingSpeed * Time.deltaTime);

            float percentage = 1 - ((this.initialPosition.y - this.transform.position.y) / this.sinkingDistance);
            float intensity = Mathf.Clamp(0.2f + (percentage * (maxIntensity - minIntensity)), this.minIntensity, this.maxIntensity);
            this.pointLight.intensity = intensity;

            if (endPosition == this.transform.position)
            {
                sinking = false;
            }
        }
    }

    public void Reset()
    {
        this.transform.position = initialPosition;
        this.sinking = false;
    }

    public void StartSinking(float sinkingDistance, float sinkingTime)
    {
        this.sinkingDistance = sinkingDistance;
        this.sinkingTime = sinkingTime;
        this.sinking = true;
    }
}
