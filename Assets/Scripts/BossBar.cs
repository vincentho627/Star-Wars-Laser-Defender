using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{

    Vector3 localScale;

    private void Start()
    {
        localScale = transform.localScale;

    }

    public void TakeDamageScale(float health)
    {
        localScale.x -= health;
    }
}
