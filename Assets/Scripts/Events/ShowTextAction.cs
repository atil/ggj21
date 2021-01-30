using System.Collections;
using TMPro;
using UnityEngine;

public class ShowTextAction : EventAction
{
    public string Text;
    public bool ShowOnUp;
    public float ShowDuration;
    public bool ShowOnce;
    
    public GameObject TextUpGo;
    public GameObject TextBottomGo;

    private GameObject _textGo;
    private TextMeshProUGUI _textUI;

    private bool _hasShown;
    
    void Start()
    {
        if (ShowOnUp)
        {
            _textGo = TextUpGo;    
        }
        else
        {
            _textGo = TextBottomGo;
        }

        _textUI = _textGo.GetComponent<TextMeshProUGUI>();
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
        _textGo.SetActive(true);
        _textUI.SetText(Text);
        _hasShown = true;
        yield return new WaitForSeconds(ShowDuration);
        _textGo.SetActive(false);
        _textUI.SetText("");
    }
}
