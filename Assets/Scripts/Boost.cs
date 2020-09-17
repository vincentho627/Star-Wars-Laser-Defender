using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    [SerializeField] string effect;
    [SerializeField] AudioClip boostUpVFX;
    [SerializeField] [Range(0, 1)] float volume;

    public string GetEffect()
    {
        return effect;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            AudioSource.PlayClipAtPoint(boostUpVFX, Camera.main.transform.position, volume);

        }
    }

}
