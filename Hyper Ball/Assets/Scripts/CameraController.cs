using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 DistanceFromPlayer;

    Vector3 originalPos;
    void Start()
    {
        transform.position = player.position + Vector3.up * DistanceFromPlayer.y + player.forward * DistanceFromPlayer.z;

        originalPos = transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 targetPos = originalPos + (Vector3.right * player.position.x) / 2.5f + (Vector3.up * player.position.y) / 2;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 3);

    }
}
