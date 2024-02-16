using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDropHandler {
    public Image playerImage = null;
    public Image mirrorImage = null;
    public Image healthNumberImage = null;
    public Image glowImage = null;

    public int maxHealth = 5;
    public int health = 5;
    public int mana = 1;

    public bool isPlayer;
    public bool isFire;

    public GameObject[] manaBalls = new GameObject[5];

    private Animator animator = null;

    public AudioSource dealAudio        = null;
    public AudioSource healAudio        = null;
    public AudioSource mirrorAudio      = null;
    public AudioSource smashAudio       = null;

    void Start(){       
        animator = GetComponent<Animator>();
        updateHealth();
        updateManaBalls();
    }

    internal void playHitAnim(){
        if (animator != null)
            animator.SetTrigger("Hit");
    }

    public void OnDrop(PointerEventData eventData){
        if (!GameController.instance.isPlayable)
            return;

        GameObject obj = eventData.pointerDrag;
        if (obj!=null){
            Card card = obj.GetComponent<Card>();
            if (card != null){
                GameController.instance.useCard(card, this, GameController.instance.playersHand);
            }
        }
    }

    internal void updateHealth(){
        if (health >= 0 && health < GameController.instance.healthNumbers.Length){
            healthNumberImage.sprite = GameController.instance.healthNumbers[health];
        }
        else{
            Debug.LogWarning("Health is not a valid number," +health.ToString());
        }
    }

    internal void setMirror(bool on){
        mirrorImage.gameObject.SetActive(on);
    }

    internal bool hasMirror(){
        return mirrorImage.gameObject.activeInHierarchy;
    }

    internal void updateManaBalls(){
        for(int i=0; i<5; i++){
            if (mana > i)
                manaBalls[i].SetActive(true);
            else
                manaBalls[i].SetActive(false);
        }
    }

    internal void playMirrorSound(){
        mirrorAudio.Play();
    }

    internal void playSmashSound(){
        smashAudio.Play();
    }

    internal void playHealSound(){
        healAudio.Play();
    }

    internal void playDealSound(){
        dealAudio.Play();
    }

}
