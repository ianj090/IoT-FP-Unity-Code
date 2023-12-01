using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speedDecrement = 10.0f;
    private PlayerController playerController;

    void Start() {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            playerController = player.GetComponent<PlayerController>();
        }

        if (playerController != null) {
            PlayerController.OnPlayerDeath += PlayerDestroyed;
        }
    }

    void OnDestroy() {
        PlayerController.OnPlayerDeath -= PlayerDestroyed;
    }

    void Update() {
        if (playerController == null) {
            return;
        }

        transform.Translate(Vector2.left * playerController.speedVal * 0.2f * Time.deltaTime);

        if (transform.position.x <= -12.0f) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && playerController != null) {
            playerController.speedVal -= speedDecrement;
            playerController.UpdateSpeedDisplay(playerController.speedVal);

            playerController.lives--;
            playerController.UpdateLivesDisplay();

            Destroy(gameObject);
        }
    }

    private void PlayerDestroyed() {
        Destroy(gameObject);
    }
}
