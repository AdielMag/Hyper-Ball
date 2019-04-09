using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private bool tap, swipeRight, swipeLeft, swipeUp, swipeDown;
    private bool isDraging, isTouching;
    private Vector2 startTouch, swipeDelta;

    private void Update()
    {
        tap = swipeRight = swipeLeft = swipeUp = swipeDown = false;

        isTouching = (Input.touchCount != 0 || Input.GetMouseButton(0)) ? true : false;

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            isDraging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (startTouch == (Vector2)Input.mousePosition)
                tap = true;
            Reset();
        }
        #endregion

        #region Mobile Inputs
        if (Input.touchCount != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                if (startTouch == Input.touches[0].position)
                    tap = true;
                Reset();
            }
        }
        #endregion

        // Calculate the distance
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touchCount != 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

        // Did we cross the dead zone?

        if (swipeDelta.magnitude > 125)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // Left or Right
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                //Up or Down
                if (y < 0)
                    swipeDown = true;
                else
                {
                    swipeUp = true;
                }
            }
            Reset();
        }
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }

    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool IsTouching { get { return isTouching; } }
    public bool Tap { get { return tap; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
}
