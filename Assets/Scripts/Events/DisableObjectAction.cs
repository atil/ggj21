using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjectAction : EventAction
{
	public GameObject TargetObject;
	public override void Call(RoomEnteranceDirection direction, bool isFirstEntry)
	{
		TargetObject.SetActive(false);
	}
}
