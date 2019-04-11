using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int lane = 0;
    public float lanesPosMultiplier = 2;

    public float jumpForce;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    public GameObject projectile;


    #region Singelton
    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }
    #endregion

    Rigidbody rb;
    SwipeController swpCon;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        swpCon = GetComponent<SwipeController>();
    }

    private void Update()
    {
        if (swpCon.SwipeRight)
        {
            if (lane >= 2)
                return;

            lane++;
        }
        else if (swpCon.SwipeLeft)
        {
            if (lane <= -2)
                return;

            lane--;
        }

        if (swpCon.SwipeUp)
        {
            if (IsGrounded())
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (swpCon.Tap)
        {
            GameObject obj = ObjectPooler.instance.SpawnFromPool("Projectile", transform.position + transform.forward * 1.3f, Quaternion.Euler(90, 0, 0));
        }
        Vector3 targetPos = Vector3.up * transform.position.y + transform.right * (lane * lanesPosMultiplier);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10);

        if (rb.velocity.y > 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        if (rb.velocity.y > 0 && !swpCon.IsTouching)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, 1.2f))
            return true;

        return false;
    }
}
