using UnityEngine;
using UnityEngine.Tilemaps;

public class OpenDoorAction : EventAction
{
	public Tilemap DoorTilemap;
	public Vector2Int DoorStart;
	public Vector2Int DoorEnd;
	
	public override void Call(RoomEnteranceDirection direction, bool isFirstEntry)
	{
		if (!isFirstEntry)
		{
			return;
		}
		
		if (CallOnEntranceDirection > 0 && direction != CallOnEntranceDirection)
		{
			return;
		}

		for (int i = DoorStart.x; i <= DoorEnd.x; i++)
		{
			for (int j = DoorStart.y; j <= DoorEnd.y; j++)
			{
				DoorTilemap.SetTile(new Vector3Int(i, j, 0), null);
			}
		}
	}
}