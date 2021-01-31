using System.Collections;
using TMPro;
using UnityEngine;

public class ShowTextAction : EventAction
{
    public string Text;
    public float ShowDuration;
    public bool ShowOnce;

    public GameObject TextParent;
    private TextMeshProUGUI _textUI;

    private bool _hasShown;
    
    void Start()
    {
        TextParent = FindObjectOfType<Game>().TextParent;

        _textUI = TextParent.GetComponentInChildren<TextMeshProUGUI>();
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
        TextParent.gameObject.SetActive(true);
        _textUI.SetText(Text);
        _hasShown = true;
        yield return new WaitForSeconds(ShowDuration);
        TextParent.gameObject.SetActive(false);
        _textUI.SetText("");
    }
}
