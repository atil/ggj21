using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextAction : EventAction
{
	public TextShowOnCollision TargetTextTrigger;
	public string NewText;

	public override void Call(RoomEnteranceDirection direction, bool isFirstEntry)
	{
		TargetTextTrigger.Text = NewText;
	}
}
