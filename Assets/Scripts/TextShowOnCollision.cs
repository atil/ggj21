using TMPro;
using UnityEngine;

public class TextShowOnCollision : MonoBehaviour
{
    public string Text;
    public bool ShowOnUp;
    public GameObject TextUpGo;
    public GameObject TextBottomGo;

    private GameObject _textGo;
    private TextMeshProUGUI _textUI;
    private BoxCollider2D _collider;
    
    private bool _onTrigger;

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
        _collider = GetComponent<BoxCollider2D>();
        _onTrigger = false;
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
            _textUI.SetText(Text);
        }

        if (_onTrigger && !_onTriggerCurrentFrame)
        {
            _textGo.SetActive(false);
            _textUI.SetText("");
            
        }

        _onTrigger = _onTriggerCurrentFrame;
    }
}
