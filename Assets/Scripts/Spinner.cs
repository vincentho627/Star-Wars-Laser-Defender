using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{

    [SerializeField] float rotationAmount = 360f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationAmount * Time.deltaTime);
    }

}
