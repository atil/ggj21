using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVisibilityOnCollision : MonoBehaviour
{
    public GameObject ObjectToEnable = null;
    public GameObject ObjectToDisable = null;
    
    public bool PlayOnce;

    private BoxCollider2D _collider;
    
    private bool _onTrigger;
    private bool _isTriggerToEnableNotNull;
    private bool _isTriggerToDisableNotNull;
    private bool _isPlayed;

    void Start()
    {
        _isTriggerToDisableNotNull = ObjectToDisable != null;
        _isTriggerToEnableNotNull = ObjectToEnable != null;
        
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


            if (!PlayOnce || !_isPlayed)
            {
                if (_isTriggerToEnableNotNull)
                {
                    ObjectToEnable.gameObject.SetActive(true);
                }
            
                if (_isTriggerToDisableNotNull)
                {
                    ObjectToDisable.gameObject.SetActive(false);
                }
                _isPlayed = true;
            }
        }

        _onTrigger = onTriggerCurrentFrame;
    }

}
