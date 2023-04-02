using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagers : MonoBehaviour
{
    public GameObject pipePrefab;
    public float spawnRate = 2f;
    public float pipeMinHeight = -2f;
    public float pipeMaxHeight = 2f;
    public float pipeXPosition = 10f;
    public float pipeSpeed = 2f;
    public float groundX = -1.2f;

    private BirdController birdController;

    // Start is called before the first frame update
    void Start()
    {
        birdController = GameObject.FindObjectOfType<BirdController>();
        StartCoroutine(SpawnPipe());
    }

    // Update is called once per frame
    void Update()
    {
        MovePipes();
    }

    IEnumerator SpawnPipe()
    {
        while (true)
        {
            if (birdController.hasFlapped)
            {
                float pipeYPosition = Random.Range(pipeMinHeight, pipeMaxHeight);
                Vector2 pipePosition = new Vector2(pipeXPosition, pipeYPosition);
                Instantiate(pipePrefab, pipePosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }


    void MovePipes()
    {
        if (birdController.hasFlapped)
        {
            GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
            foreach (GameObject pipe in pipes)
            {
                pipe.transform.position += Vector3.left * pipeSpeed * Time.deltaTime;

                // Check if pipe is off-screen
                if (pipe.transform.position.x < -10f)
                {
                    // Destroy the pipe
                    Destroy(pipe);
                }
            }

            // Move the ground to the left
            GameObject ground = GameObject.FindGameObjectWithTag("Ground");
            ground.transform.position += Vector3.left * pipeSpeed * Time.deltaTime;

            // Check if the ground is off-screen
            if (ground.transform.position.x < groundX)
            {
                // Respawn the ground to the right
                ground.transform.position = new Vector3(2.51f, -8.67f, 0);
            }

            // Move the background to the left
            GameObject background = GameObject.FindGameObjectWithTag("Background");
            background.transform.position += Vector3.left * pipeSpeed * Time.deltaTime;

            // Check if the ground is off-screen
            if (background.transform.position.x < -54.28)
            {
                // Respawn the ground to the right
                background.transform.position = new Vector3(-14.22f, -5.05f, 0);
            }
        }
    }

}
