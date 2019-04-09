using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    int minRange, maxRange;
    Vector3 targetDirection, targetPos;

    public float obstacleSpeed = 1;

    private void Start()
    {
        if (Random.Range(0f, 10f) > 5) // Use min as range
        {
            minRange = Random.Range(-2, 2);
            maxRange = Random.Range(minRange, 3);
        }
        else                        // Use max as range
        {
            maxRange = Random.Range(-1, 3);
            minRange = Random.Range(-2, maxRange);
        }

        targetDirection = (Random.Range(0f, 10f) > 5) ? Vector3.right : Vector3.left;
    }


    private void Update()
    {
        if (transform.position.x <= minRange)
            targetDirection = Vector3.right;
        else if (transform.position.x >= maxRange)
            targetDirection = Vector3.left;

        transform.Translate(targetDirection * obstacleSpeed / 40);
    }
}
