using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextShowOnCollision : MonoBehaviour
{
    public string _text;
    
    private GameObject _textGo;
    private Text _textUI;
    private BoxCollider2D _collider;
    
    private bool _onTrigger;

    void Start()
    {
        _textGo = GameObject.Find("Textbox"); 
        _textUI = _textGo.GetComponent<Text>();
        _collider = GetComponent<BoxCollider2D>();
        _onTrigger = false;

        _textGo.SetActive(false);
    }

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, _collider.size , 0);

        var _onTriggerCurrentFrame = false;
        foreach (Collider2D c in colliders)
        {
            if (c == _collider)
            {
                continue;
            }

            if (c.name == "Player")
            {
                _onTriggerCurrentFrame = true;
            }
        }

        if (!_onTrigger && _onTriggerCurrentFrame)
        {
            _textGo.SetActive(true);
            _textUI.text = _text;
        }

        if (_onTrigger && !_onTriggerCurrentFrame)
        {
            _textGo.SetActive(false);
            _textUI.text = "";
        }

        _onTrigger = _onTriggerCurrentFrame;
    }
}
