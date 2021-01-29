using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour
{
    public Room CurrentRoom;

    public Transform RoomTargetLeft;
    public Transform RoomTargetRight;
    public Transform RoomTargetMiddle;
    public Transform RoomTargetUp;
    public Transform RoomTargetDown;

    public AnimationCurve TransitionCurve;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Room targetRoom = CurrentRoom.RoomLeft;
            if (targetRoom != null)
            {
                StartCoroutine(RoomMoveCoroutine(CurrentRoom, RoomTargetMiddle, RoomTargetRight, true, false));
                StartCoroutine(RoomMoveCoroutine(targetRoom, RoomTargetLeft, RoomTargetMiddle, true, true));
                CurrentRoom = targetRoom;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Room targetRoom = CurrentRoom.RoomRight;
            if (targetRoom != null)
            {
                StartCoroutine(RoomMoveCoroutine(CurrentRoom, RoomTargetMiddle, RoomTargetLeft, true, false));
                StartCoroutine(RoomMoveCoroutine(targetRoom, RoomTargetRight, RoomTargetMiddle, true, true));
                CurrentRoom = targetRoom;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Room targetRoom = CurrentRoom.RoomUp;
            if (targetRoom != null)
            {
                StartCoroutine(RoomMoveCoroutine(CurrentRoom, RoomTargetMiddle, RoomTargetDown, true, false));
                StartCoroutine(RoomMoveCoroutine(targetRoom, RoomTargetUp, RoomTargetMiddle, true, true));
                CurrentRoom = targetRoom;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Room targetRoom = CurrentRoom.RoomDown;
            if (targetRoom != null)
            {
                StartCoroutine(RoomMoveCoroutine(CurrentRoom, RoomTargetMiddle, RoomTargetUp, true, false));
                StartCoroutine(RoomMoveCoroutine(targetRoom, RoomTargetDown, RoomTargetMiddle, true, true));
                CurrentRoom = targetRoom;
            }
        }
    }

    private IEnumerator RoomMoveCoroutine(Room room, Transform moveSource, Transform moveTarget, bool activeBeforeMove, bool activeAfterMove)
    {
        Vector3 start = moveSource.position;
        Vector3 end = moveTarget.position;

        room.gameObject.SetActive(activeBeforeMove);

        const float duration = 1f;
        for (float f = 0; f < duration; f += Time.deltaTime)
        {
            float t = TransitionCurve.Evaluate(f / duration);
            room.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        room.gameObject.SetActive(activeAfterMove);
    }
}
