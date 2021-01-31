using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAction : EventAction
{
	public AudioSource Sound;
	public bool PlayOnce;

	private bool _isPlayed;

	private void Start()
	{
		_isPlayed = false;
	}

	public override void Call(RoomEnteranceDirection direction = RoomEnteranceDirection.Any, bool isFirstEntry = false)
	{
		if (PlayOnce && _isPlayed)
		{
			return;
		}
		Sound.Play();
		_isPlayed = true;
	}
}
