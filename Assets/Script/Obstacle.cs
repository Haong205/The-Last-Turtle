using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float speed = 2f;
    private float BottomEdge;

    private void Start()
    {
        BottomEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).y - 1f;
    }

    private void Update()
    {
        transform.position += Vector3.down * Random.Range(speed*0.8f, speed*1.2f) * Time.deltaTime;

        if (transform.position.y < BottomEdge)
        {
            Destroy(gameObject);
        }
        Message.MessageSent2?.Invoke(BottomEdge.ToString());
    }
}
