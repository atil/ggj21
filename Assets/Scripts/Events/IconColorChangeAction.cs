using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconColorChangeAction : EventAction
{
    public SpriteRenderer Icon;

    public Color NewIconColor;
    
    public override void Call(RoomEnteranceDirection direction, bool isFirstEntry)
    {
        Icon.color = NewIconColor;
    }
}
