using UnityEngine;

public class RabbitController : MonoBehaviour
{
    private GameController gameController;
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void OnMouseDown()
    {
        gameController.RabbitHit(this.gameObject);
    }
}
