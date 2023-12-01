using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBG : MonoBehaviour
{
    public float endX;
    private float length;
    private PlayerController playerController;

    void Start () {
        length = GetComponent<SpriteRenderer>().bounds.size.x * 2;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            playerController = player.GetComponent<PlayerController>();
        }
    }
 
    void Update () {
        if (playerController != null) {
            float speed = playerController.speedVal / 2;

            transform.Translate(Vector2.left * speed * 0.2f * Time.deltaTime);

            if (transform.position.x < endX ) {
                transform.position = new Vector2(transform.position.x + length, transform.position.y);
            }
        }
    }
}
