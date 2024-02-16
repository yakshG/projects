using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidbody;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0f) ;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFace();
    }

    void FlipEnemyFace()
    {
        transform.localScale = new Vector3(-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }
}