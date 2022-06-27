using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorer : MonoBehaviour
{
    private float speed = 5f;
    private float BottomEdge;

    private void Start()
    {
        BottomEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).y - 1f;
    }

    private void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y < BottomEdge)
        {
            Destroy(gameObject);
        }
        Message.MessageSent2?.Invoke(BottomEdge.ToString());
    }
}
