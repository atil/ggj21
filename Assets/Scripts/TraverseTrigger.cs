using UnityEngine;

public enum TraverseDirection
{
    None,
    Up,
    Down,
    Left,
    Right,
}

public class TraverseTrigger : MonoBehaviour
{
    public TraverseDirection Direction;
}