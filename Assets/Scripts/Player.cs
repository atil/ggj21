﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public Game Game;
    public AnimationCurve MovementCurve;
    public GameObject ParticlePrefab;
    public Transform ParticlesParent;
    
    private BoxCollider2D _collider;
    public bool CanTraverse = true;

    public Animator Animator;

    private bool _isMoving;
    private Coroutine _moveCoroutine;

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Vector3Int cellPos = Game.Grid.WorldToCell(transform.position);
        // Debug.Log(cellPos);
        
        if (Game.IsTraversing || Game.EndGameTriggered)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine(Vector2Int.up));
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine(Vector2Int.left));
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine(Vector2Int.down));
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _moveCoroutine = StartCoroutine(MoveCoroutine(Vector2Int.right));
        }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, _collider.size, 0);
        HandleTriggers(colliders);

    }

    private IEnumerator MoveCoroutine(Vector2Int dir)
    {
        if (_isMoving)
        {
            yield break;
        }

        _isMoving = true;
        Vector2 src = transform.position;
        
        Vector3 offset = new Vector3(0.49f, -0.49f, 0.0f); // From top left to the middle
        Vector3 offsetPosition = transform.position + offset;
        offsetPosition.x = Mathf.RoundToInt(offsetPosition.x);
        offsetPosition.y = Mathf.RoundToInt(offsetPosition.y);
        offsetPosition.z = Mathf.RoundToInt(offsetPosition.z);
        
        Vector3Int srcCellPos = Game.Grid.WorldToCell(offsetPosition); // Get the cellPos using the middle
        Vector3Int targetCellPos = srcCellPos + (Vector3Int) dir;
        Vector2 target = Game.Grid.CellToWorld(targetCellPos) - offset;
        
        TilemapCollider2D currentCollider = Game.CurrentRoom.transform.Find("Collision").GetComponent<TilemapCollider2D>();
        if (currentCollider.OverlapPoint(target))
        {
            _isMoving = false; // Can't move into the collider
            yield break;
        }
        
        TilemapCollider2D currentCollider2 = Game.CurrentRoom.transform.Find("Doors").GetComponent<TilemapCollider2D>();
        if (currentCollider2.OverlapPoint(target))
        {
            _isMoving = false; // Can't move into the collider
            yield break;
        }

        if (target.x > 4f || target.x < -6 || target.y > 5 || target.y < -5)
        {
            _isMoving = false; // Out of bounds
            yield break;
        }

        StartCoroutine(PlayFootstepParticleCoroutine());
        Game.Sfx.PlayFootstep();

        Animator.speed = 0;

        const float duration = 0.15f;
        for (float f = 0; f < duration; f += Time.deltaTime)
        {
            float t = MovementCurve.Evaluate(f / duration);
            transform.position = Vector2.Lerp(src, target, t);
            yield return null;
        }
        
        Animator.speed = 1;

        transform.position = target;
        _isMoving = false;
        ClampIntoBounds();
    }

    private IEnumerator PlayFootstepParticleCoroutine()
    {
        GameObject particleGo = Instantiate(ParticlePrefab, transform.position, Quaternion.identity);
        particleGo.SetActive(true);
        particleGo.transform.SetParent(ParticlesParent);
        yield return new WaitForSeconds(1f);
        Destroy(particleGo);
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
                if (CanTraverse && !Game.EndGameTriggered)
                {
                    CanTraverse = false;
                    if (_moveCoroutine != null)
                    {
                        _isMoving = false;
                        StopCoroutine(_moveCoroutine);
                    }
                    Game.Traverse(traverseTrigger.Direction);
                }
            }
        }
        if (!isInTraverseTrigger)
        {
            CanTraverse = true;
        }
    }
}
