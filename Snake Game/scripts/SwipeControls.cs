using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControls : MonoBehaviour
{
    Vector2 swipeStart;
    Vector2 swipeEnd;
    float minDistance = 10;

    public static event System.Action<swipeDirection> onSwipe = delegate { };

    public enum swipeDirection
    {
        Up,
        Down,
        Left,
        Right
    };
    void Start()
    {

    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                swipeStart = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swipeEnd = touch.position;
                processSwipe();
            }
        }

        //moousetouchsimulation
        if(Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            swipeEnd= Input.mousePosition;
            processSwipe();
        }

    }

    void processSwipe()
    {
        float distance = Vector2.Distance(swipeStart, swipeEnd);
        if (distance > minDistance)
        {
            if (isVerticalSwipe()) //vertical
            {
                if(swipeEnd.y > swipeStart.y)
                {
                    onSwipe(swipeDirection.Up);
                }
                else
                {
                    onSwipe(swipeDirection.Down);
                }
            }
            else //horizontal
            {
                if (swipeEnd.x > swipeStart.x)
                {
                    onSwipe(swipeDirection.Right);
                }
                else
                {
                    onSwipe(swipeDirection.Left);
                }
            }
        }
    }

    bool isVerticalSwipe()
    {
        float vertical = Mathf.Abs(swipeEnd.y - swipeStart.y);
        float horizaontal = Mathf.Abs(swipeEnd.x - swipeStart.x);
        if (vertical > horizaontal)
            return true;
        return false;
    }

}