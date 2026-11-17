
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveSpeed = 2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float movement = Mathf.PingPong(
            Time.time * moveSpeed,
            moveDistance
        );

        transform.position = startPosition +
            Vector3.right * (movement - moveDistance / 2f);
    }
}
