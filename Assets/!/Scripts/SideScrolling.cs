using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;
    private float leftCameraBorder;
    private void Awake()
    {
        player = GameObject.FindWithTag("Mario").transform;
    }

    public float minimumXOffset; // Minimum x offset from the center of the camera to the player

    private void LateUpdate()
    {
        float cameraCenter = Camera.main.transform.position.x;
        float offsetFromCenter = player.position.x - cameraCenter;

        if (offsetFromCenter > minimumXOffset)
        {
            // Move the camera to follow the player, maintaining the offset
            transform.position = new Vector3(player.position.x - minimumXOffset, transform.position.y, transform.position.z);
        }
    }
}
