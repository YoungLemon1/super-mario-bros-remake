using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.CompareTag("SideCollider") || collision.CompareTag("TopCollider"))
        {
            Debug.Log("Hit!");
            PlayerState.Instance.DisablePlayerInput();
        }
        else if (collision.CompareTag("BottomCollider"))
        {
            var marioRB = GameObject.FindGameObjectWithTag("Mario").GetComponent<Rigidbody2D>();
            marioRB.AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
            if (gameObject.CompareTag("Goomba"))
            {
                CustomAnimator.Instance.AnimateKillGoomba(gameObject);
                Destroy(gameObject.GetComponent<BoxCollider2D>());
                Destroy(gameObject, 0.2f);
            }
        }
    }
}
