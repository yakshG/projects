using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour {
    public Player targetPlayer = null;
    public Card   sourceCard   = null;
    public Image  effectImage  = null;

    public AudioSource iceAudio          = null;
    public AudioSource fireballAudio     = null;
    public AudioSource destructballAudio = null;

    public void EndTrigger(){
        bool bounce = false;
        if (targetPlayer.hasMirror()){
            bounce = true;
            targetPlayer.setMirror(false);
            targetPlayer.playSmashSound();

            if (targetPlayer.isPlayer){
                GameController.instance.castAttackEffect(sourceCard, GameController.instance.enemy);
            }
            else{
                GameController.instance.castAttackEffect(sourceCard, GameController.instance.player);
            }
        }
        else{
            int damage = sourceCard.cardData.damage;
            if (!targetPlayer.isPlayer){
                if (sourceCard.cardData.damageType == CardData.DamageType.Fire && targetPlayer.isFire)
                    damage = damage / 2;
                if (sourceCard.cardData.damageType == CardData.DamageType.Ice && !targetPlayer.isFire)
                    damage = damage / 2;
            }
            targetPlayer.health -= damage;
            targetPlayer.playHitAnim();

            GameController.instance.updateHealths();

            if (targetPlayer.health <= 0) {
                targetPlayer.health = 0;
                if (targetPlayer.isPlayer){
                    GameController.instance.playPlayerDieSound();
                }
                else{
                    GameController.instance.playEnemyDieSound();
                }
            }

            if (!bounce)
                GameController.instance.nextPlayersTurn();

            GameController.instance.isPlayable = true;
        }
        Destroy(gameObject);
    }

    internal void playIceSound(){
        iceAudio.Play();
    }

    internal void playFireballSound(){
        fireballAudio.Play();
    }

    internal void playDestructSound(){
        destructballAudio.Play();
    }
}
