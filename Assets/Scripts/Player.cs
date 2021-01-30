using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public Game Game;
    public AnimationCurve MovementCurve;
    
    private BoxCollider2D _collider;
    private bool _canTraverse = true;

    private bool _isMoving;
    private Coroutine _moveCoroutine;

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Game.IsTraversing)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine(Vector2.up));
        }
        if (Input.GetKey(KeyCode.A))
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine(Vector2.left));
        }
        if (Input.GetKey(KeyCode.S))
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine(Vector2.down));
        }
        if (Input.GetKey(KeyCode.D))
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine(Vector2.right));
        }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, _collider.size, 0);
        HandleTriggers(colliders);

        foreach (Collider2D c in colliders)
        {
            if (c == _collider
                || c.gameObject.layer == LayerMask.NameToLayer("Trigger"))
            {
                continue;
            }

            ColliderDistance2D dist = c.Distance(_collider);
            Vector2 penet = dist.distance * dist.normal;
            transform.position -= (Vector3)penet;
        }

        ClampIntoBounds();
    }

    private IEnumerator MoveCoroutine(Vector2 dir)
    {
        if (_isMoving)
        {
            yield break;
        }

        _isMoving = true;

        Vector2 src = transform.position;
        Vector2 target = (Vector2)transform.position + dir * 1f;
        const float duration = 0.15f;
        for (float f = 0; f < duration; f += Time.deltaTime)
        {
            float t = MovementCurve.Evaluate(f / duration);
            transform.position = Vector2.Lerp(src, target, t);
            yield return null;
        }

        transform.position = target;
        _isMoving = false;
    }

    private void ClampIntoBounds()
    {
        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -5.4f, 4.4f);
        pos.y = Mathf.Clamp(pos.y, -4.4f, 4.4f);
        transform.position = pos;
    }

    private void HandleTriggers(Collider2D[] colliders)
    {
        bool isInTraverseTrigger = false;
        foreach (Collider2D c in colliders)
        {
            if (c.TryGetComponent(out TraverseTrigger traverseTrigger))
            {
                isInTraverseTrigger = true;
                if (_canTraverse)
                {
                    _canTraverse = false;
                    if (_moveCoroutine != null)
                    {
                        StopCoroutine(_moveCoroutine);
                    }
                    Game.Traverse(traverseTrigger.Direction);
                }
            }
        }
        if (!isInTraverseTrigger)
        {
            _canTraverse = true;
        }
    }
}
