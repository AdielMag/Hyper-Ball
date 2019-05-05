using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{

    Text text;
    public int avgFrameRate;

    private void Start()
    {
        Application.targetFrameRate = 300;
        text = GetComponent<Text>();
    }

    private void Update()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        text.text = avgFrameRate.ToString() + " FPS";
    }
}
