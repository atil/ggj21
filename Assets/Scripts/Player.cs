using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public BoxCollider2D Collider;

    private Vector2 _velocity;
    private float _acceleration = 1f;

    void Update()
    {
        float dt = Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
        {
            _velocity += Vector2.up * _acceleration * dt;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _velocity += Vector2.left * _acceleration * dt;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _velocity += Vector2.down * _acceleration * dt;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _velocity += Vector2.right * _acceleration * dt;
        }

        _velocity *= 0.9f;
        if (_velocity.sqrMagnitude < 0.000001f)
        {
            _velocity = Vector2.zero;
        }

        transform.position += (Vector3)_velocity;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, Collider.size , 0);
        foreach (Collider2D c in colliders)
        {
            if (c == Collider)
            {
                continue;
            }

            ColliderDistance2D dist = c.Distance(Collider);
            Vector2 penet = dist.distance * dist.normal;
            transform.position -= (Vector3)penet;
            _velocity = Vector2.zero;
        }

    }
}
