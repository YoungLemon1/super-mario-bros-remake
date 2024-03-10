using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mario"))
        {
            Debug.Log(collision.tag);
            PlayerState.Instance.DisablePlayerInput();
        }
    }
}
