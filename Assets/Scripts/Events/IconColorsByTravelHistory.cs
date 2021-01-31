using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;

public class IconColorsByTravelHistory : EventAction
{
    public SpriteRenderer[] IconLists;

    public List<string> ExpectedTravel;
    public Game Game;

    public Color DefaultIconColor;
    public Color ActiveIconColor;
    
    public override void Call(RoomEnteranceDirection direction = RoomEnteranceDirection.Any, bool isFirstEntry = false)
    {
        var travelHistory = Game.TravelHistory;
        var len = travelHistory.Count;
        
                
        if (Equals(travelHistory[len - 3], ExpectedTravel[0]) && Equals(travelHistory[len - 2], ExpectedTravel[1]) && Equals(travelHistory[len - 1], ExpectedTravel[2]))
        {
            IconLists[0].color = ActiveIconColor;
            IconLists[1].color = ActiveIconColor;
            IconLists[2].color = ActiveIconColor;
        }
        else if (Equals(travelHistory[len - 2], ExpectedTravel[0]) && Equals(travelHistory[len - 1], ExpectedTravel[1]))
        {
            IconLists[0].color = ActiveIconColor;
            IconLists[1].color = ActiveIconColor;
            IconLists[2].color = DefaultIconColor;
        }
        else if (Equals(travelHistory[len - 1], ExpectedTravel[0]))
        {
            IconLists[0].color = ActiveIconColor;
            IconLists[1].color = DefaultIconColor;
            IconLists[2].color = DefaultIconColor;
        }
        else
        {
            IconLists[0].color = DefaultIconColor;
            IconLists[1].color = DefaultIconColor;
            IconLists[2].color = DefaultIconColor;
        }
    }
}
