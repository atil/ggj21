﻿using System.Collections;
using TMPro;
using UnityEngine;

public class ShowTextAction : EventAction
{
    [TextArea(4, 4)]
    public string Text;
    public Color TextColor = Color.white;
    public float ShowDuration;
    public bool ShowOnce;
    public GameObject SpeechBubbleIcon;

    private const float kTextShownInterval = 0.05f;

    private GameObject _textParent;
    private TextMeshProUGUI _textUI;

    private bool _hasShown;
    private Coroutine _showTextCoroutine;
    
    void Start()
    {
        _textParent = FindObjectOfType<Game>().TextParent;

        _textUI = _textParent.GetComponentInChildren<TextMeshProUGUI>();
        _hasShown = false;
    }
    
    public override void Call(RoomEnteranceDirection direction, bool isFirstEntry)
    {
        if (CallOnEntranceDirection > 0 && direction != CallOnEntranceDirection)
        {
            return;
        }
        
        if (ShowOnce && _hasShown)
        {
            return;
        }
        _showTextCoroutine = StartCoroutine(ShowText());
    }

    public void CancelShow()
    {
        if (_showTextCoroutine != null)
        {
            StopCoroutine(_showTextCoroutine);
        }
        _textParent.gameObject.SetActive(false);
        _textUI.SetText("");
        _textUI.color = Color.white;
        if (SpeechBubbleIcon != null)
        {
            SpeechBubbleIcon.SetActive(false);
        }
    }
    
    private IEnumerator ShowText()
    {
        var currLength = 1;
        var currText = "";
        var lastPart = "";

        _hasShown = true;
        _textUI.SetText("");
        _textUI.color = TextColor;
        _textParent.gameObject.SetActive(true);
        if (SpeechBubbleIcon != null)
        {
            SpeechBubbleIcon.SetActive(true);
        }
        
        while (currLength <= Text.Length)
        {
            yield return new WaitForSeconds(kTextShownInterval);
            
            currText = Text.Substring(0, currLength);
            lastPart = "<color=#FFFFFF00>" + Text.Substring(currLength, Text.Length - currLength) + "</color>";
            _textUI.SetText(currText + lastPart);
            
            currLength += 1;
        }
        
        yield return new WaitForSeconds(ShowDuration);
        _textParent.gameObject.SetActive(false);
        _textUI.SetText("");
        _textUI.color = Color.white;
        if (SpeechBubbleIcon != null)
        {
            SpeechBubbleIcon.SetActive(false);
        }
    }
}
