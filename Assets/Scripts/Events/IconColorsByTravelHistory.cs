using System;
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

    public AudioSource LightSound01;
    public AudioSource LightSound02;
    public AudioSource LightSound03;
    public AudioSource LightSoundReset;

    private static int _state;

    private void Start()
    {
        _state = 0;
    }

    public override void Call(RoomEnteranceDirection direction = RoomEnteranceDirection.Any, bool isFirstEntry = false)
    {
        if (_state > 2)
        {
            return;
            
        }
        
        var len = Game.TravelHistory.Count;

        if (Equals(Game.TravelHistory[len - 3], ExpectedTravel[0]) && Equals(Game.TravelHistory[len - 2], ExpectedTravel[1]) && Equals(Game.TravelHistory[len - 1], ExpectedTravel[2]))
        {
            IconLists[0].color = ActiveIconColor;
            IconLists[1].color = ActiveIconColor;
            IconLists[2].color = ActiveIconColor;
            LightSound03.Play();
            _state = 3;
        }
        else if (Equals(Game.TravelHistory[len - 2], ExpectedTravel[0]) && Equals(Game.TravelHistory[len - 1], ExpectedTravel[1]))
        {
            IconLists[0].color = ActiveIconColor;
            IconLists[1].color = ActiveIconColor;
            IconLists[2].color = DefaultIconColor;
            LightSound02.Play();
            _state = 2;
        }
        else if (Equals(Game.TravelHistory[len - 1], ExpectedTravel[0]))
        {
            IconLists[0].color = ActiveIconColor;
            IconLists[1].color = DefaultIconColor;
            IconLists[2].color = DefaultIconColor;
            LightSound01.Play();
            _state = 1;
        }
        else
        {
            IconLists[0].color = DefaultIconColor;
            IconLists[1].color = DefaultIconColor;
            IconLists[2].color = DefaultIconColor;

            if (_state > 0)
            {
                LightSoundReset.Play();
            }
            _state = 0;
        }
    }
}
