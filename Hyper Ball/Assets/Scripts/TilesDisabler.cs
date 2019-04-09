using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesDisabler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        other.gameObject.SetActive(false);
    }
}
