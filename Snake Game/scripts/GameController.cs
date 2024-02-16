using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    const float width  = 3.7f;
    const float height = 7f;

    public float snakeSpeed = 1f;

    public bool alive         = true;
    public bool waitingToPlay = true;

    public Sprite tailSprite = null;
    public Sprite bodySprite = null;

    public SnakeHead snakeHead = null;

    List<Egg> eggs = new List<Egg>();

    List<Spike> spikes = new List<Spike>();

    int level                = 0;
    int noOfEggsForNextLevel = 0;
    public int score         = 0;
    public int highScore     = 0;
    public TextMeshProUGUI scoreText     = null;
    public TextMeshProUGUI highScoreText = null;
    public TextMeshProUGUI tapToPlayText = null;
    public TextMeshProUGUI gameOverText  = null;

    public BodyPart bodyPrefab      = null;
    public GameObject rockPrefab    = null;
    public GameObject eggPrefab     = null;
    public GameObject goldEggPrefab = null;
    public GameObject spikePrefab = null;

    void Start()
    {
        instance = this;
        Debug.Log("starting snake game");
        createWalls();
        alive = false;
    }
    
    void Update()
    {
        if (waitingToPlay)
        {
            foreach (Touch touch in Input.touches)
            {
                if(touch.phase == TouchPhase.Ended)
                {
                    startGamePlay();
                }
            }
            if (Input.GetMouseButtonUp(0))
                startGamePlay();
        }
    }

    public void gameOver()
    {
        alive = false;
        waitingToPlay = true;

        gameOverText.gameObject.SetActive(true);
        tapToPlayText.gameObject.SetActive(true);
    }

    public void startGamePlay()
    {
        score = 0;
        level = 0;

        scoreText.text = "Score = " + score;
        highScoreText.text = "High Score = " + highScore;

        gameOverText.gameObject.SetActive(false);
        tapToPlayText.gameObject.SetActive(false);

        waitingToPlay = false;
        alive = true;

        killOldEggs();
        killOldSpikes();

        levelUp();
    }

    void levelUp()
    {
        level++;

        noOfEggsForNextLevel = 4 + (level * 2);

        snakeSpeed = 1f + (level / 3f);
        if(snakeSpeed > 8)
            snakeSpeed = 8;

        snakeHead.resetSnake();
        createEgg();

        killOldSpikes();

        for (int i = 0; i < level; i++)
        {
            createSpike();
        }
    }

    void createSpike()
    {
        Vector3 position;
        position.x = -width + Random.Range(1, (width * 2) - 2f);
        position.y = -height + Random.Range(1, (height * 2) - 2f);
        position.z = -1f;
        Spike spike = null;
        
        spike = Instantiate(spikePrefab, position, Quaternion.identity).GetComponent<Spike>();

        spikes.Add(spike);
    }

    void killOldSpikes()
    {
        foreach (Spike spike in spikes)
        {
            Destroy(spike.gameObject);
        }
        spikes.Clear();
    }

    void createWalls()
    {
        float z = -1f;
        Vector3 start  = new Vector3(-width, -height, z);
        Vector3 finish = new Vector3(-width, +height, z);
        createWall(start, finish);

        start  = new Vector3(+width, -height,z);
        finish = new Vector3(+width, +height, z);
        createWall(start, finish);

        start  = new Vector3(-width, -height, z);
        finish = new Vector3(+width, -height, z);
        createWall(start, finish);

        start  = new Vector3(-width, +height, z);
        finish = new Vector3(+width, +height, z);
        createWall(start, finish);
    }

    void createWall(Vector3 start, Vector3 finish)
    {
        float distance = Vector3.Distance(start, finish);
        int noOfRocks = (int)(distance * 3f);
        Vector3 delta = (finish - start) / noOfRocks;

        Vector3 position = start;
        for (int rock = 0; rock <= noOfRocks; rock++)
        {
            float rotation = Random.Range(0, 360f);
            float scale    = Random.Range(1.5f, 2f);
            createRock(position, scale, rotation);
            position = position + delta;
        }
    }

    void createRock(Vector3 position, float scale, float rotation)
    {
        GameObject rock = Instantiate(rockPrefab, position, Quaternion.Euler(0, 0, rotation));
        rock.transform.localScale = new Vector3(scale, scale, 1);
    }

    void createEgg(bool golden = false)
    {
        Vector3 position;
        position.x = -width + Random.Range(1, (width*2)-2f);
        position.y = -height + Random.Range(1, (height * 2) - 2f);
        position.z = -1f;
        Egg egg = null;
        if(golden)
            egg = Instantiate(goldEggPrefab, position, Quaternion.identity).GetComponent<Egg>();
        else
            egg = Instantiate(eggPrefab, position, Quaternion.identity).GetComponent<Egg>();

        eggs.Add(egg);
    }

    void killOldEggs()
    {
        foreach (Egg egg in eggs)
        {
            Destroy(egg.gameObject);
        }
        eggs.Clear();
    }

    public void eggEaten(Egg egg)
    {
        score++;
        noOfEggsForNextLevel--;
        if (noOfEggsForNextLevel == 0)
        {
            score += 10;
            levelUp();
        }
        else if(noOfEggsForNextLevel == 1)
            createEgg(true);
        else
            createEgg(false);

        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "High Score = " + highScore;
        }

        scoreText.text = "Score = " + score;

        eggs.Remove(egg);
        Destroy(egg.gameObject);
    }
}
