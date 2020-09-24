using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{

    float rateOfChange = 0.2f;
    
    bool complete = false;
    bool stall = true;
    Color tmp; 

    [SerializeField] GameObject cutScene;
    [SerializeField] float timeWait = 2f;
    [SerializeField] bool run = false;
    [SerializeField] AudioClip soundVFX;
    [SerializeField] [Range(0,1)] float soundVolume;

    bool call = true;
    // Start is called before the first frame update
    void Start()
    {
        tmp = cutScene.GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        cutScene.GetComponent<SpriteRenderer>().color = tmp;
    }

    // Update is called once per frame
    void Update()
    {
        if (run)
        {
            if (call)
            {
                AudioSource.PlayClipAtPoint(soundVFX, Camera.main.transform.position, soundVolume);
                call = false;
            }
            StartCutscene();
        }
        
    }

    public void SetRun()
    {
        run = true;
    }

    public void resetBool()
    {
        run = false;
        call = true;
        complete = false;
        stall = true;
    }

    public void StartCutscene()
    {
        if (tmp.a >= 0 && tmp.a <= 1 && !complete)
        {
            tmp.a += (rateOfChange * Time.deltaTime * 5f) / 255;
            rateOfChange += 0.1f;
        }
        else if (tmp.a > 1 && !complete)
        {
            complete = true;
        }
        else if (tmp.a > 0 && complete)
        {
            if (stall)
            {
                StartCoroutine(WaitForTime());
                stall = false;
            }
            else
            {
                tmp.a -= (rateOfChange * Time.deltaTime * 5f) / 255;
                rateOfChange -= 0.1f / 255;
            }
        }
        else
        {
            ResetCutscene();
        }

        cutScene.GetComponent<SpriteRenderer>().color = tmp;
    }

    private void ResetCutscene()
    {
        tmp.a = 0f;
        rateOfChange = 0.2f;
        run = false;
    }

    IEnumerator WaitForTime()
    {
        run = false;
        yield return new WaitForSeconds(timeWait);
        run = true;
        tmp.a -= (rateOfChange * Time.deltaTime * 5f) / 255;
        rateOfChange -= 0.1f / 255;
    }

}
