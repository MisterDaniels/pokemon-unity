﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask longGrassLayer;

    private bool isMoving;
    private Vector2 input;
    private Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (!isMoving) {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // To not move in diagonal
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero) {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos)) {
                    StartCoroutine(Move(targetPos));
                }
            }
        }
    }

    IEnumerator Move(Vector3 targetPos) {
        isMoving = true;
        animator.SetBool("isMoving", isMoving);

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 
                moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
        animator.SetBool("isMoving", isMoving);

        CheckForEncounters();
    }

    private bool IsWalkable(Vector3 targetPos) {
        // Get center of the footer base
        targetPos.y -= 0.5f;

        if (Physics2D.OverlapCircle(targetPos, 0.01f, solidObjectsLayer) != null) {
            return false;
        }

        return true;
    }

    private void CheckForEncounters() {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, longGrassLayer) != null) {
            if (Random.Range(1, 101) <= 10) {
                Debug.Log("A wild pokémon appears");
            }
        }
    }

}