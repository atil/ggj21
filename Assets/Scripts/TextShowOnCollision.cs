using TMPro;
using UnityEngine;

public class TextShowOnCollision : MonoBehaviour
{
    [TextArea(15, 20)]
    public string Text;

    private GameObject _textParent;
    private TextMeshProUGUI _textUI;
    private BoxCollider2D _collider;
    
    private bool _onTrigger;

    void Start()
    {

        _textParent = FindObjectOfType<Game>().TextParent;
        _textUI = _textParent.GetComponentInChildren<TextMeshProUGUI>();
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
            _textParent.gameObject.SetActive(true);
            _textUI.SetText(Text);
        }

        if (_onTrigger && !_onTriggerCurrentFrame)
        {
            _textParent.gameObject.SetActive(false);
            _textUI.SetText("");
            
        }

        _onTrigger = _onTriggerCurrentFrame;
    }
}
