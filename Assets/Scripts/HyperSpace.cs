using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperSpace : MonoBehaviour
{

    [SerializeField] GameObject particle;
    [SerializeField] float length = 1f; // max 35
    [SerializeField] float rateOfLength = 0.2f;

    [SerializeField] bool complete = false;
    ParticleSystemRenderer particleRenderer;

    [SerializeField] AudioClip zooming;
    [SerializeField] [Range(0, 1)] float zoomingVolume;

    bool callHyperSpace = false;

    // Start is called before the first frame update
    void Start()
    {
        particleRenderer = GetComponent<ParticleSystemRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (callHyperSpace)
        {
            RunHyperSpeed();
        }
    }

    public void PlayHyperSpaceSound()
    {
        AudioSource.PlayClipAtPoint(zooming, Camera.main.transform.position, zoomingVolume);
    }

    public void SetHyperSpace()
    {
        callHyperSpace = true;
    }

    public void ResetSpeed()
    {
        particleRenderer.lengthScale = 0;
        rateOfLength = 0.2f;
    }


    public void RunHyperSpeed()
    {
        if (particleRenderer.lengthScale < 35f && !complete)
        {
            particleRenderer.lengthScale += 0.5f * rateOfLength * Time.deltaTime;
            rateOfLength += 0.1f;
        }
        else if (particleRenderer.lengthScale >= 35f && !complete)
        {
            complete = true;
        } else
        {
            if (particleRenderer.lengthScale > 0)
            {
                particleRenderer.lengthScale -= 0.5f * rateOfLength * Time.deltaTime;
                if (rateOfLength > 1.1f)
                {
                    rateOfLength -= 0.1f;
                }
            }
            else
            {
                ResetSpeed();
                callHyperSpace = false;
            }
        }
        
    }

}
