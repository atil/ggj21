using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public float Acceleration = 1f;
    public float Friction = 1.1f;
    public Game Game;

    private BoxCollider2D _collider;
    private bool _canTraverse = true;
    private Vector2 _velocity;

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

        float dt = Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
        {
            _velocity += Vector2.up * Acceleration * dt;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _velocity += Vector2.left * Acceleration * dt;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _velocity += Vector2.down * Acceleration * dt;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _velocity += Vector2.right * Acceleration * dt;
        }

        _velocity /= Friction;
        if (_velocity.sqrMagnitude < 0.000001f)
        {
            _velocity = Vector2.zero;
        }

        transform.position += (Vector3)_velocity;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, _collider.size, 0);
        foreach (Collider2D c in colliders)
        {
            if (c == _collider
                && c.gameObject.layer == LayerMask.NameToLayer("Trigger"))
            {
                continue;
            }

            ColliderDistance2D dist = c.Distance(_collider);
            Vector2 penet = dist.distance * dist.normal;
            transform.position -= (Vector3)penet;
            _velocity = Vector2.zero;
        }

        bool isInTraverseTrigger = false;
        foreach (Collider2D c in colliders)
        {
            if (c.TryGetComponent(out TraverseTrigger traverseTrigger))
            {
                isInTraverseTrigger = true;
                if (_canTraverse)
                {
                    _canTraverse = false;
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
