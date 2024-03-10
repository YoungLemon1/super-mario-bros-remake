using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Brick"))
        {
            Debug.Log("Top collider of player hit a brick");
        }
        else if (collision.CompareTag("MysteryBlock"))
        {
            Debug.Log("Top collider of player hit a mystery block");
        }
    }
}
