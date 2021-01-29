using System.Collections;
using UnityEngine;

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
            DoRoomTransition(CurrentRoom.RoomUp, RoomTargetRight, RoomTargetLeft);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            DoRoomTransition(CurrentRoom.RoomUp, RoomTargetLeft, RoomTargetRight);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            DoRoomTransition(CurrentRoom.RoomUp, RoomTargetDown, RoomTargetUp);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DoRoomTransition(CurrentRoom.RoomDown, RoomTargetUp, RoomTargetDown);
        }
    }

    private void DoRoomTransition(Room targetRoom, Transform currentRoomTarget, Transform toRoomTarget)
    {
        if (targetRoom == null)
        {
            return;
        }

        StartCoroutine(RoomMoveCoroutine(CurrentRoom, RoomTargetMiddle, currentRoomTarget, true, false));
        StartCoroutine(RoomMoveCoroutine(targetRoom, toRoomTarget, RoomTargetMiddle, true, true));
        CurrentRoom = targetRoom;
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
