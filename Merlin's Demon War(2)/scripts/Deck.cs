using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck {
    public List<CardData> cardDatas = new List<CardData>();

    public void create(){
        List<CardData> cardDataInOrder = new List<CardData>();
        foreach(CardData cardData in GameController.instance.cards){
            for(int i = 0; i<cardData.numberInDeck; i++)
                cardDataInOrder.Add(cardData);
        }

        while(cardDataInOrder.Count>0){
            int randomIndex = Random.Range(0,cardDataInOrder.Count);
            cardDatas.Add(cardDataInOrder[randomIndex]);
            cardDataInOrder.RemoveAt(randomIndex);
        }
    }

    private CardData randomCard(){
        CardData result = null;

        if (cardDatas.Count == 0)
            create();

        result = cardDatas[0];
        cardDatas.RemoveAt(0);

        return result;
    }

    private Card createNewCard(Vector3 position, string animName){
        GameObject newCard = GameObject.Instantiate(GameController.instance.cardPrefab,
                                                    GameController.instance.canvas.gameObject.transform);
        newCard.transform.position = position;
        Card card = newCard.GetComponent<Card>();
        if (card){
            card.cardData = randomCard();
            card.Initialise();

            Animator animator = newCard.GetComponentInChildren<Animator>();
            if (animator){
                animator.CrossFade(animName, 0);
            }
            else{
                Debug.LogError("no animator found!");
            }

            return card;
        }
        else{
            Debug.LogError("no card component found!");
            return null;
        }
    }

    internal void dealCard(Hand hand){
        for (int i = 0; i < 3; i++){
            if (hand.cards[i]==null){
                if (hand.isPlayers){
                    GameController.instance.player.playDealSound();
                }
                else{
                    GameController.instance.enemy.playDealSound();
                }
                hand.cards[i] = createNewCard(hand.positions[i].position, hand.animNames[i]);
                return;
            }
        }
    }

}
