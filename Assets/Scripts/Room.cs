using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RoomEnteranceDirection
{
    Any,
    Down,
    Up,
    Right,
    Left,
}

public class Room : MonoBehaviour
{
    public string RoomId;
    public Room RoomUp;
    public Room RoomDown;
    public Room RoomLeft;
    public Room RoomRight;

    public List<EventAction> EventActionsOnEnter;
    
    public void OnRoomEnter(TraverseDirection direction, bool isFirstEntry)
    {
        foreach (var e in EventActionsOnEnter)
        {
            e.Call( (RoomEnteranceDirection)((int)direction));
        }
    }
}