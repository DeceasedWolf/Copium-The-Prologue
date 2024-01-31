using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float collisionOffset = 0.05f;
    private bool _isMovingSide;
    [SerializeField] ContactFilter2D movementFilter;
    private Vector2 _movementInput;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private Animator _animator;
    private List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();
    
    // Start is called before the first frame update
    private void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        // If movement input is not 0, try to move
        if (_movementInput != Vector2.zero) {
            bool success = TryMove(_movementInput);

            if (!success) {
                success = TryMove(new Vector2(_movementInput.x, 0));

                if (!success) {
                    TryMove(new Vector2(0, _movementInput.y));
                }
            }
        }
        UpdateAnimations();
    }

    private void UpdateAnimations() {
        
        if (_movementInput.x > 0 || _movementInput.x < 0) {
            _animator.SetBool("isMovingSide", true);
            _isMovingSide = true;
        } else if (_movementInput.x == 0){
            _animator.SetBool("isMovingSide", false);
            _isMovingSide = false;
        }

        if (_movementInput.y > 0) {
            _animator.SetBool("isMovingUp", true);
            _animator.SetBool("isMovingDown", false);
        } else if (_movementInput.y < 0) {
            _animator.SetBool("isMovingDown", true);
            _animator.SetBool("isMovingUp", false);
        } else if (_movementInput.y == 0){
            _animator.SetBool("isMovingUp", false);
            _animator.SetBool("isMovingDown", false);
        }

        // Set direction of sprite to movement direction
        if (_movementInput.x > 0 && _isMovingSide) {
            _spriteRenderer.flipX = false;
        } else if (_movementInput.x < 0 && _isMovingSide) {
            _spriteRenderer.flipX = true;
        } else {
            _spriteRenderer.flipX = false;
        }
    }
    
    private bool TryMove(Vector2 direction) {
        // Check for potential collisions
        int collisionCount = _rb.Cast(
            direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
            movementFilter, // The settings that determine where a collision can occur on, such as layers to collide with
            _castCollisions, // List of collisions to store the found collisions into after the Cast is finished
            moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset
        if (collisionCount == 0) {
            _rb.MovePosition(_rb.position + (direction * (moveSpeed * Time.fixedDeltaTime)));
            return true;
        } else {
            return false;
        }
    }

    private void OnMove(InputValue movementValue) {
        _movementInput = movementValue.Get<Vector2>();
    }
}
