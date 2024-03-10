using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace Assets.__.Scripts
{
    class SideCollision: MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("FlagPole"))
                Debug.Log("Level Complete!");
            else if (collision.CompareTag("Castle"))
                Debug.Log("Next Level");
        }
    }
}
