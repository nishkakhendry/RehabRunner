using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject obstacle;
    private float timePassed = 0;
    public float spawnFrequency = 3;
    public static List<GameObject> obstacles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        spawnObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > spawnFrequency)
        {
            spawnObstacle();
            timePassed = 0;
        }

    }

    void spawnObstacle()
    {
        float xPosition = Random.Range(-3.5f, 3.5f);
        float yPosition = -1;
        float height = 1;
        Vector3 scale;
        float size = (xPosition < 0) ? (Random.Range(3.5f, 2 * (xPosition + 6))) : (Random.Range(3.5f, 2 * (6 - xPosition)));
        if (size < 8)
        {
            float rng = Random.Range(0f, 1f);
            if (rng < 0.3)
            {
                height = 4;
            }
            else if (rng < 0.6)
            {
                height = 1.8f;
            }
            else
            {
                height = 1;
            }

        }
        else
        {
            scale = new Vector3(size, 1, 1);
        }
        yPosition = height / 2 - 1.5f;
        scale = new Vector3(size, height, 1);
        GameObject currObstacle = Instantiate(obstacle, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
        currObstacle.transform.localScale = scale;
        obstacles.Add(currObstacle);
    }
}
