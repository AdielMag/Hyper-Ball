using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathBox : MonoBehaviour
{
    PlatformManager pMan;

    public GameObject temporrayPlayButton;

    private void Start()
    {
        pMan = PlatformManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        pMan.LostRun();
        temporrayPlayButton.SetActive(true);    
        }
}
