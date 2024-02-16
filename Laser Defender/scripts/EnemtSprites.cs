using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemtSprites : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;

    void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }
     
}
