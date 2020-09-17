using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostStats : MonoBehaviour
{

    [SerializeField] GameObject electricPrefab;
    float currentPosition;
    float offset = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEnergyStat()
    {
        Vector2 electricPosition = new Vector2(currentPosition, transform.position.y);
        currentPosition += offset;
        Instantiate(electricPrefab, electricPosition, transform.rotation);
    }
}
