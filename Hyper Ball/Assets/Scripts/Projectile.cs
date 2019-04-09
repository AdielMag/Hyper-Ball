using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    public float timer;
    float startTime;

    Rigidbody rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>(); 
    }

    public void OnObjectSpawn() 
    {
        startTime = Time.time;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(transform.up * 20, ForceMode.Impulse);
    }

    void FixedUpdate() 
    {
        if (Time.time > startTime + timer)
            gameObject.SetActive(false);
    }
}
