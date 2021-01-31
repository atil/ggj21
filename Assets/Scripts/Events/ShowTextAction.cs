using System.Collections;
using TMPro;
using UnityEngine;

public class ShowTextAction : EventAction
{
    [TextArea(4, 4)]
    public string Text;
    public float ShowDuration;
    public bool ShowOnce;

    private GameObject _textParent;
    private TextMeshProUGUI _textUI;

    private bool _hasShown;
    
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
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        _textParent.gameObject.SetActive(true);
        _textUI.SetText(Text);
        _hasShown = true;
        yield return new WaitForSeconds(ShowDuration);
        _textParent.gameObject.SetActive(false);
        _textUI.SetText("");
    }
}
