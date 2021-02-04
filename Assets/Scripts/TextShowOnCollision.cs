using System.Collections;
using TMPro;
using UnityEngine;

public class TextShowOnCollision : MonoBehaviour
{
    [TextArea(4, 4)]
    public string Text;

    public AudioSource Sound;
    public bool PlayOnce;

    public BoxCollider2D TriggerToEnable = null;

    private const float kTextShownInterval = 0.05f;

    private GameObject _textParent;
    private TextMeshProUGUI _textUI;
    private BoxCollider2D _collider;
    
    private bool _onTrigger;
    private bool _isPlayed;

    void Start()
    {

        _textParent = FindObjectOfType<Game>().TextParent;
        _textUI = _textParent.GetComponentInChildren<TextMeshProUGUI>();
        _collider = GetComponent<BoxCollider2D>();
        _onTrigger = false;
        _isPlayed = false;
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
            StartCoroutine(ShowText());
            
            if (!PlayOnce || !_isPlayed)
            {
                Sound.Play();
                _isPlayed = true;
                if (TriggerToEnable != null)
                {
                    TriggerToEnable.gameObject.SetActive(true);
                }
            }
        }

        if (_onTrigger && !onTriggerCurrentFrame)
        {
            _textParent.gameObject.SetActive(false);
            _textUI.SetText("");
        }

        _onTrigger = onTriggerCurrentFrame;
    }

    private IEnumerator ShowText()
    {
        var currLength = 1;
        var currText = "";
        var lastPart = "";
        
        _textUI.SetText("");
        _textParent.gameObject.SetActive(true);
        
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
