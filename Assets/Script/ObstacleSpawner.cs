using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] LeftBranch;
    public GameObject[] RightBranch;
    public GameObject[] obstacles;
    public GameObject score;
    public GameObject JellyFish;
    public float maxtime;
    private float timer = 0;
    private float xRange = 1.8f;
    public float yRange;
    public float ObstacleDis;
    public float PlayerWidth;
    public float JellySpawnTime;
    private float JellySpawnTimer = 0;
    float WorldWidth;

    Vector2 BottomLeftEdge;

    // Update is called once per frame

    private void Start()
    {
        Vector2 topCorner = new Vector2(Screen.width, Screen.height);
        Vector2 someCorner = Camera.main.ScreenToWorldPoint(topCorner);
        float WorldHeight = someCorner.y;
        WorldWidth = someCorner.x;
        transform.position = new Vector2(0, WorldHeight + 1);
        BottomLeftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero);
    }


    void Update()
    {
        if (JellySpawnTimer > Random.Range(JellySpawnTime*0.75f, JellySpawnTime*1.50f))
        {
            float posx = Random.Range(BottomLeftEdge.x*0.9f, Mathf.Abs(BottomLeftEdge.x*0.9f));
            GameObject newJellyFish = Instantiate(JellyFish);
            newJellyFish.transform.position = transform.position + new Vector3(posx, 1, 0);
            FindObjectOfType<GameManager>().scored = false;
            JellySpawnTimer = 0;
        }
        if(timer > Random.Range( maxtime*0.75f, maxtime*1.25f))
        {
            float posx = Random.Range(BottomLeftEdge.x + PlayerWidth/2 , WorldWidth - PlayerWidth/2);
            GameObject newObstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)]);
            newObstacle.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            if (posx > 0)
            {
                newObstacle.transform.position = new Vector2(posx - (PlayerWidth / 2 + newObstacle.GetComponent<Collider2D>().bounds.size.x/2), transform.position.y);
            }
            else
            {
                newObstacle.transform.position = new Vector2(posx + (PlayerWidth / 2 + newObstacle.GetComponent<Collider2D>().bounds.size.x/2), transform.position.y);

            }


            //if (posx > -xRange*.80f)
            //{
            //    GameObject newLeftBranch = Instantiate(LeftBranch[Random.Range(0, LeftBranch.Length)]);
            //    newLeftBranch.transform.position = transform.position + new Vector3(posx - ObstacleDis / 2, Random.Range(0, yRange), 0);
            //}

            //if (posx < xRange*.80f)
            //{
            //    GameObject newRightBranch = Instantiate(RightBranch[Random.Range(0, RightBranch.Length)]);
            //    newRightBranch.transform.position = transform.position + new Vector3(posx + ObstacleDis / 2, Random.Range(0, yRange), 0);
            //}



            timer = 0;
        }

        timer += Time.deltaTime;
        JellySpawnTimer += Time.deltaTime;

        Message.MessageSent1?.Invoke(xRange.ToString());
    }
}
