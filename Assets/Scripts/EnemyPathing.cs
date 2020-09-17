using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{

    WaveConfig waveConfig;
    float moveSpeed = 2f;
    List<Transform> waypoints;
    int waypointIndex = 0;
    [SerializeField] bool death = true;


    // Start is called before the first frame update
    void Start()
    {

        moveSpeed = waveConfig.GetMoveSpeed();
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (waypointIndex < waypoints.Count)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            if (this.gameObject.GetComponent<Enemy>().IsBoss())
            {
                if (death)
                {
                    Destroy(gameObject);
                }
            } else
            {
                waypointIndex = 0;
            }
          
        }
    }

    public void setWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

}
