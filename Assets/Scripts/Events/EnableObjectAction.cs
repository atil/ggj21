using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectAction : EventAction
{
	public GameObject TargetObject;
	public override void Call(RoomEnteranceDirection direction, bool isFirstEntry)
	{
		TargetObject.SetActive(true);
	}
}