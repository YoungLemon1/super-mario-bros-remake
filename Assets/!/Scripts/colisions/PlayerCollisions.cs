using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private Collider2D topCollider;
    private Collider2D bottomCollider;
    private Collider2D leftCollider;
    private Collider2D rightCollider;

    void Start()
    {
        // Find and store references to the player's child colliders
        topCollider = transform.Find("top").GetComponent<Collider2D>();
        bottomCollider = transform.Find("bottom").GetComponent<Collider2D>();
        leftCollider = transform.Find("left").GetComponent<Collider2D>();
        rightCollider = transform.Find("right").GetComponent<Collider2D>();
    }

    private bool CheckSideCollidersTouched(Collider2D collision)
    {
        return leftCollider.IsTouching(collision) || rightCollider.IsTouching(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
