using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    int enemyPoints = 50;
    float xSpeed;
    Rigidbody2D rb;
    PlayerMovement player;
    GameSession gameSession;
    void Awake()
    {
        
    }

    void Start()
    {
        rb     = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        gameSession = FindObjectOfType<GameSession>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        rb.velocity = new Vector2 (xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            gameSession.score += enemyPoints;
            gameSession.scoreText.text = gameSession.score.ToString();
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
