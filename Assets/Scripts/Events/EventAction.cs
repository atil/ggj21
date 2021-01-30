using UnityEngine;

public abstract class EventAction : MonoBehaviour
{
    public RoomEnteranceDirection CallOnEntranceDirection;
    public abstract void Call(RoomEnteranceDirection direction = RoomEnteranceDirection.Any, bool isFirstEntry = false);
}
