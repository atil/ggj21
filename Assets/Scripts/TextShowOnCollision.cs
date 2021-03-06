﻿using System.Collections;
using TMPro;
using UnityEngine;

public class TextShowOnCollision : MonoBehaviour
{
    [TextArea(4, 4)]
    public string Text;
    public Color TextColor = Color.white;
    public bool ShowOnce;

    public AudioSource Sound;
    public bool PlaySoundOnce;

    public GameObject SpeechBubbleIcon;

    private const float kTextShownInterval = 0.05f;

    private GameObject _textParent;
    private TextMeshProUGUI _textUI;
    private BoxCollider2D _collider;
    
    private bool _onTrigger;
    private bool _isPlayed;
    private bool _hasShown;

    void Start()
    {
        _textParent = FindObjectOfType<Game>().TextParent;
        _textUI = _textParent.GetComponentInChildren<TextMeshProUGUI>();
        _collider = GetComponent<BoxCollider2D>();
        _onTrigger = false;
        _isPlayed = false;
        _hasShown = false;
    }

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, _collider.size , 0);

        var onTriggerCurrentFrame = false;
        foreach (Collider2D c in colliders)
        {
            if (c == _collider)
            {
                continue;
            }

            if (c.name == "Player")
            {
                onTriggerCurrentFrame = true;
            }
        }

        if (!_onTrigger && onTriggerCurrentFrame)
        {
            _onTrigger = true;
            if (!ShowOnce || !_hasShown)
            {
                StartCoroutine(ShowText());
                _hasShown = true;
            }

            if (!PlaySoundOnce || !_isPlayed)
            {
                Sound.Play();
                _isPlayed = true;
            }
        }

        if (_onTrigger && !onTriggerCurrentFrame)
        {
            _textParent.gameObject.SetActive(false);
            _textUI.SetText("");
            _textUI.color = Color.white;
            if (SpeechBubbleIcon != null)
            { 
                SpeechBubbleIcon.SetActive(false);
            }
        }

        _onTrigger = onTriggerCurrentFrame;
    }

    private IEnumerator ShowText()
    {
        foreach (var textShow in FindObjectsOfType<ShowTextAction>())
        {
            textShow.CancelShow();
        }
        
        var currLength = 1;
        var currText = "";
        var lastPart = "";
        
        _textUI.SetText("");
        _textUI.color = TextColor;
        _textParent.gameObject.SetActive(true);
        if (SpeechBubbleIcon != null)
        {
            SpeechBubbleIcon.SetActive(true);
        }
        
        while (currLength <= Text.Length && _onTrigger)
        {
            yield return new WaitForSeconds(kTextShownInterval);
            
            currText = Text.Substring(0, currLength);
            lastPart = "<color=#FFFFFF00>" + Text.Substring(currLength, Text.Length - currLength) + "</color>";
            _textUI.SetText(currText + lastPart);
            
            currLength += 1;
        }
    }
}
