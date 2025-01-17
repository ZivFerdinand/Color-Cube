using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Database;

public class InputManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public enum Direction 
    { 
        Left, 
        Up, 
        Right, 
        Down, 
        None 
    }

    Direction direction;
    Vector2 startPos, endPos;
    private float swipeThreshold;
    bool draggingStarted;
    public Action<Direction> onSwipeDetected;


#region Audio
    //private AudioPlayer audioPlayer;
    //private void OnEnable()
    //{
    //    audioPlayer = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioPlayer>();
    //}
#endregion


    void Awake()
    {
        draggingStarted = false;
        direction = Direction.None;
    }
    void Start()
    {
        swipeThreshold = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AnimationManager>().swipeThreshold;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingStarted = true;
        startPos = eventData.pressPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingStarted)
        {
            endPos = eventData.position;

            Vector2 difference = endPos - startPos; // difference vector between start and end positions.
            float X1 = startPos.x, Y1 = startPos.y;
            float X2 = endPos.x, Y2 = endPos.y;

            if (difference.magnitude > swipeThreshold)
            {
                float dir = 1;
                if(Database.Cameras.IsInverted)
                    dir = -1;
                
                if(dir * (X2 - X1)>=0)
                {
                    direction = (dir * (Y2 - Y1) >= 0) ? Direction.Right: Direction.Down;
                    
                }
                else
                {
                    direction = (dir * (Y2 - Y1) >= 0) ? Direction.Up : Direction.Left;
                    
                }
            }
            else
            {
                direction = Direction.None;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingStarted && direction != Direction.None)
        {
            //A swipe is detected
            if (onSwipeDetected != null)
            {
                //audioPlayer.Play("CubeMoving");
                onSwipeDetected.Invoke(direction);
                
            }
        }

        //reset the variables
        startPos = Vector2.zero;
        endPos = Vector2.zero;
        draggingStarted = false;
    }
}