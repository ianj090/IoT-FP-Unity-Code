using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float speed;
    private PlayerController playerController;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            playerController = player.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (playerController != null) {
            speed = playerController.speedVal / 2;

            transform.Translate(Vector2.left * speed * 0.2f * Time.deltaTime);

            if (transform.position.x < -12) {
                Destroy(gameObject);
            }
        }
    }
}
