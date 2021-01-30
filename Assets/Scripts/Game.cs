﻿using System.Collections;
using UnityEngine;

public enum PlayerFadeType
{
    Show, Hide
}

public class Game : MonoBehaviour
{
    public Player Player;

    public Room CurrentRoom;
    public GridLayout Grid;

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
                StartCoroutine(TeleportPlayer(direction, CurrentRoom.RoomUp));
                StartCoroutine(DoRoomTransition(CurrentRoom.RoomUp, RoomTargetDown, RoomTargetUp));
                break;
            case TraverseDirection.Down:
                StartCoroutine(TeleportPlayer(direction, CurrentRoom.RoomDown));
                StartCoroutine(DoRoomTransition(CurrentRoom.RoomDown, RoomTargetUp, RoomTargetDown));
                break;
            case TraverseDirection.Left:
                StartCoroutine(TeleportPlayer(direction, CurrentRoom.RoomLeft));
                StartCoroutine(DoRoomTransition(CurrentRoom.RoomLeft, RoomTargetRight, RoomTargetLeft));
                break;
            case TraverseDirection.Right:
                StartCoroutine(TeleportPlayer(direction, CurrentRoom.RoomRight));
                StartCoroutine(DoRoomTransition(CurrentRoom.RoomRight, RoomTargetLeft, RoomTargetRight));
                break;
            default:
                Debug.LogError($"Undefined traverse direction {direction}");
                break;
        }
    }

    private IEnumerator TeleportPlayer(TraverseDirection direction, Room targetRoom)
    {
        Player.transform.SetParent(CurrentRoom.transform);
        StartCoroutine(FadePlayer(PlayerFadeType.Hide, TraverseDuration * 0.3f));
        yield return new WaitForSeconds(TraverseDuration / 2f);
        Vector3 offset = new Vector3(0.49f, -0.49f, 0.0f);
        Vector3Int cellPos = Grid.WorldToCell(Player.transform.position + offset);
        Debug.Log(Player.transform.position);
        switch (direction)
        {
            case TraverseDirection.Up:
                cellPos = new Vector3Int(cellPos.x, -5, 0);
                break;
            case TraverseDirection.Down:
                cellPos = new Vector3Int(cellPos.x, 4, 0);
                break;
            case TraverseDirection.Left:
                cellPos = new Vector3Int(4, cellPos.y, 0);
                break;
            case TraverseDirection.Right:
                cellPos = new Vector3Int(-5, cellPos.y, 0);
                break;
        }

        Player.transform.localPosition = Grid.CellToWorld(cellPos) - offset;
        Player.transform.SetParent(targetRoom.transform, false);
        StartCoroutine(FadePlayer(PlayerFadeType.Show, TraverseDuration * 0.3f));
        yield return new WaitForSeconds(TraverseDuration / 2f);
        Player.transform.SetParent(null);
        CurrentRoom.OnRoomEnter(direction);
    }

    private IEnumerator FadePlayer(PlayerFadeType type, float duration)
    {
        SpriteRenderer renderer = Player.GetComponent<SpriteRenderer>();
        float srcAlpha = type == PlayerFadeType.Show ? 0f : 1f;
        float targetAlpha = type == PlayerFadeType.Show ? 1f : 0f;

        for (float f = 0; f < duration; f += Time.deltaTime)
        {
            Color c = renderer.color;
            c.a = Mathf.Lerp(srcAlpha, targetAlpha, f / duration);
            renderer.color = c;
            yield return null;
        }
    }

    private IEnumerator DoRoomTransition(Room targetRoom, Transform currentRoomTarget, Transform destinationRoomTarget)
    {
        IsTraversing = true;
        if (targetRoom != null)
        {
            StartCoroutine(RoomMoveCoroutine(CurrentRoom, RoomTargetMiddle, currentRoomTarget, true, false));
            StartCoroutine(RoomMoveCoroutine(targetRoom, destinationRoomTarget, RoomTargetMiddle, true, true));
            StartCoroutine(CoverCoroutine(targetRoom));
            CurrentRoom = targetRoom;
        }
        else
        {
            Debug.LogError($"No target room defined for room {CurrentRoom}");
        }
        yield return new WaitForSeconds(TraverseDuration);
        IsTraversing = false;
    }

    private IEnumerator CoverCoroutine(Room targetRoom)
    {
        SpriteRenderer currentCover = CurrentRoom.transform.Find("Cover").GetComponent<SpriteRenderer>();
        SpriteRenderer targetCover = targetRoom.transform.Find("Cover").GetComponent<SpriteRenderer>();

        StartCoroutine(DoCoverCoroutine(currentCover, true));
        yield return new WaitForSeconds(TraverseDuration / 2f);
        StartCoroutine(DoCoverCoroutine(targetCover, false));
    }

    private IEnumerator DoCoverCoroutine(SpriteRenderer cover, bool enableCover)
    {
        float srcAlpha = enableCover ? 0f : 1f;
        float targetAlpha = enableCover ? 1f : 0f;
        for (float f = 0; f < TraverseDuration; f += Time.deltaTime)
        {
            Color c = cover.color;
            c.a = Mathf.Lerp(srcAlpha, targetAlpha, f / TraverseDuration);
            cover.color = c;
            yield return null;
        }
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
