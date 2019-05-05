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

    bool isGrounded;

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
        CheckIfGrounded();

        if (swpCon.SwipeRight)
        {
            if (lane >= 2)
                return;

            lane++;
        }
        if (swpCon.SwipeLeft)
        {
            if (lane <= -2)
                return;

            lane--;
        }
        if (swpCon.SwipeUp)
        {
            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                touchedTiles = false;
            }
        }
        if (swpCon.Tap)
        {
            GameObject obj = ObjectPooler.instance.SpawnFromPool("Projectile", transform.position + transform.forward * 1.3f, Quaternion.Euler(90, 0, 0));
        }

        Vector3 targetPos = Vector3.up * transform.position.y + transform.right * (lane * lanesPosMultiplier);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10);

        if (rb.velocity.y > 0)
        {
            jumpOnce = true;
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        if (rb.velocity.y > 0 && !swpCon.IsTouching)
        {
            jumpOnce = true;
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public bool touchedTiles, jumpOnce;

    public void CheckIfGrounded()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out RaycastHit hit, 1.2f))
        {
            if (hit.transform.tag == "Tiles")
            {
                jumpOnce = false;
                touchedTiles = true;
                isGrounded = true;
            }
        }

        else if (touchedTiles && !jumpOnce)
        {
            isGrounded = true;
        }

        else 
            isGrounded = false;
    }
}
