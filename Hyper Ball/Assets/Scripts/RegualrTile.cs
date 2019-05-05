using UnityEngine;

public class RegualrTile : MonoBehaviour,IPooledObject
{
    bool risingTile;
    bool fall, rotateOnce;

    float origHeight;
    Quaternion origRot, targetRot;

    public float fallOffsetTimer;

    PlatformManager platMan;
    Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
        platMan = PlatformManager.instance;

        origHeight = transform.position.y;
        origRot = transform.rotation;
    }

    public void OnObjectSpawn() 
    {
        if (platMan)
            platMan.tilesCount++;
        else
            PlatformManager.instance.tilesCount++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        fallOffsetTimer += Time.time;

        if (!animator)
            return;

        if (risingTile)
            animator.SetTrigger("Rise");

        else
            fall = true;
    }

    private void FixedUpdate()
    {
        if (fall)
        {
            if (Time.time <= fallOffsetTimer)
                return;

            animator.SetTrigger("Fall");

            transform.Translate(-Vector3.up / 10);

            if (!rotateOnce)
            {
                targetRot = Quaternion.Euler(Random.Range(-60, 0), Random.Range(-60, 60), Random.Range(-60, 60));
                rotateOnce = true;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 2);
        }
    }

    void DisableGameObject() 
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (platMan)
            platMan.tilesCount--;

        fall = false;
        rotateOnce = false;
        transform.rotation = origRot;
        transform.position = new Vector3(transform.position.x, origHeight, transform.position.z);

        if (animator)
        {
            animator.ResetTrigger("Fall");
            animator.SetTrigger("Reset");
        }
    }
}
