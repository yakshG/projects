using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SnakeHead : BodyPart
{
    Vector2 movement;

    private BodyPart tail = null;

    const float TIMETOADDBODYPART = 0.1f;
    float addTimer = TIMETOADDBODYPART;

    public int partsToAdd = 0;

    public AudioSource[] gulpSounds = new AudioSource[3];
    public AudioSource dieSound = null;

    List<BodyPart> parts = new List<BodyPart>();

    void Start()
    {
        SwipeControls.onSwipe += swipeDetection;
    }

    override public void Update()
    {
        if (!GameController.instance.alive)
            return; 

        base.Update();

        setMovement(movement * Time.deltaTime);
        updateDirection();
        updatePosition();

        if(partsToAdd > 0)
        {
            addTimer -= Time.deltaTime;
            if(addTimer <= 0)
            {
                addTimer = TIMETOADDBODYPART;
                addBodyPart();
                partsToAdd--;
            }
        }
    }

    void addBodyPart()
    {
        if(tail == null)
        {
            Vector3 newPosition = transform.position;
            newPosition.z = newPosition.z + 0.001f;

            BodyPart newPart = Instantiate(GameController.instance.bodyPrefab, newPosition, Quaternion.identity);
            newPart.following = this;
            tail = newPart;
            newPart.turnIntoTail();

            parts.Add(newPart);
        }
        else
        {
            Vector3 newPosition = tail.transform.position;
            newPosition.z = newPosition.z + 0.001f;

            BodyPart newPart = Instantiate(GameController.instance.bodyPrefab, newPosition, tail.transform.rotation);
            newPart.following = tail;
            newPart.turnIntoTail();
            tail.turnIntoBodyPart();
            tail = newPart;

            parts.Add(newPart);
        }
    }

    void swipeDetection(SwipeControls.swipeDirection direction)
    {
        switch (direction)
        {
            case SwipeControls.swipeDirection.Up:
                moveUp();
                break;
            case SwipeControls.swipeDirection.Down:
                moveDown();
                break;
            case SwipeControls.swipeDirection.Left:
                moveLeft();
                break;
            case SwipeControls.swipeDirection.Right:
                moveRight();
                break;
        }
    }

    void moveUp()
    {
        movement = Vector2.up * GameController.instance.snakeSpeed;
    }

    void moveDown()
    {
        movement = Vector2.down * GameController.instance.snakeSpeed;
    }
    void moveRight()
    {
        movement = Vector2.right * GameController.instance.snakeSpeed;
    }
    void moveLeft()
    {
        movement = Vector2.left * GameController.instance.snakeSpeed;
    }

    public void resetSnake()
    {
        foreach (BodyPart part in parts)
        {
            Destroy(part.gameObject);
        }
        parts.Clear();

        tail = null;
        moveUp();

        gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        gameObject.transform.position = new Vector3(0, 0, -8);

        resetMemory();
        partsToAdd = 5;
        addTimer = TIMETOADDBODYPART;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Egg egg = collision.GetComponent<Egg>();
        if (egg)
        {
            Debug.Log("egg hit");
            eatEgg(egg);
            int rand = Random.Range(0, 3);
            gulpSounds[rand].Play();  
        }
        else
        {
            Debug.Log("obstacle detected!");
            GameController.instance.gameOver();
            dieSound.Play();
        }
    }

    private void eatEgg(Egg egg)
    {
        partsToAdd = 5;
        addTimer   = 0;

        GameController.instance.eggEaten(egg);
    }
}
