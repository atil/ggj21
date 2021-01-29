using System;
using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Player Player;

    public Room CurrentRoom;

    public Transform RoomTargetLeft;
    public Transform RoomTargetRight;
    public Transform RoomTargetMiddle;
    public Transform RoomTargetUp;
    public Transform RoomTargetDown;

    public AnimationCurve TraverseCurve;
    public float TraverseDuration = 1f;

    public bool IsTraversing { get; private set; }

    public void Traverse(TraverseDirection direction)
    {
        switch (direction)
        {
            case TraverseDirection.Up:
                StartCoroutine(DoRoomTransition(CurrentRoom.RoomUp, RoomTargetDown, RoomTargetUp));
                break;
            case TraverseDirection.Down:
                StartCoroutine(DoRoomTransition(CurrentRoom.RoomDown, RoomTargetUp, RoomTargetDown));
                break;
            case TraverseDirection.Left:
                StartCoroutine(DoRoomTransition(CurrentRoom.RoomLeft, RoomTargetRight, RoomTargetLeft));
                break;
            case TraverseDirection.Right:
                StartCoroutine(DoRoomTransition(CurrentRoom.RoomRight, RoomTargetLeft, RoomTargetRight));
                break;
            default:
                Debug.LogError($"Undefined traverse direction {direction}");
                break;
        }

    }

    private IEnumerator DoRoomTransition(Room targetRoom, Transform currentRoomTarget, Transform toRoomTarget)
    {
        IsTraversing = true;
        if (targetRoom != null)
        {
            StartCoroutine(RoomMoveCoroutine(CurrentRoom, RoomTargetMiddle, currentRoomTarget, true, false));
            StartCoroutine(RoomMoveCoroutine(targetRoom, toRoomTarget, RoomTargetMiddle, true, true));
            CurrentRoom = targetRoom;
        }
        else
        {
            Debug.LogError($"No target room defined for room {CurrentRoom}");
        }
        Player.transform.SetParent(currentRoomTarget);
        yield return new WaitForSeconds(TraverseDuration / 2f);
        Player.transform.localPosition = Vector3.zero;
        //Player.transform.SetParent();
        yield return new WaitForSeconds(TraverseDuration / 2f);
        IsTraversing = false;
    }

    private IEnumerator RoomMoveCoroutine(Room room, Transform moveSource, Transform moveTarget, bool activeBeforeMove, bool activeAfterMove)
    {
        Vector3 start = moveSource.position;
        Vector3 end = moveTarget.position;

        room.gameObject.SetActive(activeBeforeMove);

        for (float f = 0; f < TraverseDuration; f += Time.deltaTime)
        {
            float t = TraverseCurve.Evaluate(f / TraverseDuration);
            room.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        room.gameObject.SetActive(activeAfterMove);
    }
}
