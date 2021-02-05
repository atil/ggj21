using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OpenDoorByTravelHistory : EventAction
{

	public List<string> ExpectedTravel;
	public Game Game;
	public GameObject LightRay;
	
	public Tilemap DoorTilemap;
	public Vector2Int DoorStart;
	public Vector2Int DoorEnd;

	private bool _isOpened;

	private void Start()
	{
		_isOpened = false;
	}

	public override void Call(RoomEnteranceDirection direction = RoomEnteranceDirection.Any, bool isFirstEntry = false)
	{
		if (_isOpened)
		{
			return;
		}
		
		var len = Game.TravelHistory.Count;
		
		if (Equals(Game.TravelHistory[len - 3], ExpectedTravel[0]) && Equals(Game.TravelHistory[len - 2], ExpectedTravel[1]) && Equals(Game.TravelHistory[len - 1], ExpectedTravel[2]))
		{
			for (int i = DoorStart.x; i <= DoorEnd.x; i++)
			{
				for (int j = DoorStart.y; j <= DoorEnd.y; j++)
				{
					DoorTilemap.SetTile(new Vector3Int(i, j, 0), null);
				}
			}

			_isOpened = true;
			LightRay.SetActive(true);
		}
	}
}
