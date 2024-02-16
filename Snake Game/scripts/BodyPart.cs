using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    Vector2 dPosition;

    public BodyPart following = null;
    
    private SpriteRenderer spriteRenderer = null;

    const int PARTSREMEMBERED = 10;
    public Vector3[] previousPostions = new Vector3[PARTSREMEMBERED];

    public int setIndex = 0;
    public int getIndex = -(PARTSREMEMBERED-1);

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }

    virtual public void Update()
    {
        if (!GameController.instance.alive) return;

        Vector3 followPosition;
        if (following != null)
        {
            if (following.getIndex > -1)
                followPosition = following.previousPostions[following.getIndex];
            else
                followPosition = following.transform.position;
        }
        else
            followPosition = gameObject.transform.position;

        previousPostions[setIndex].x = gameObject.transform.position.x;
        previousPostions[setIndex].y = gameObject.transform.position.y;
        previousPostions[setIndex].z = gameObject.transform.position.z;

        setIndex++;
        if(setIndex >= PARTSREMEMBERED) setIndex = 0;
        getIndex++;
        if (getIndex >= PARTSREMEMBERED) getIndex = 0;

        if(following != null)
        {
            Vector3 newPosition;
            if(following.getIndex > -1)
            {
                newPosition = followPosition;
            }
            else
            {
                newPosition = following.transform.position;
            }

            newPosition.z = newPosition.z + 0.001f;

            setMovement(newPosition - gameObject.transform.position);
            updateDirection();
            updatePosition();
        }
    }

    public void resetMemory()
    {
        setIndex = 0;
        getIndex = -(PARTSREMEMBERED - 1);
    }

    public void setMovement(Vector2 movement)
    {
        dPosition = movement;
    }

    public void updatePosition()
    {
        gameObject.transform.position += (Vector3)dPosition;
    }

    public void updateDirection()
    {
        if (dPosition.y > 0)
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        else if (dPosition.y < 0)
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
        else if (dPosition.x < 0)
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
        else if (dPosition.x > 0)
            gameObject.transform.localEulerAngles = new Vector3(0, 0, -90);   

    }

    public void turnIntoTail()
    {
        spriteRenderer.sprite = GameController.instance.tailSprite;
    }

    public void turnIntoBodyPart()
    {
        spriteRenderer.sprite = GameController.instance.bodySprite;
    }
}
