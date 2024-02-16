using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour {
    static public GameController instance = null;

    public Deck playerDeck = new Deck();
    public Deck enemyDeck  = new Deck();

    public Hand playersHand = new Hand();
    public Hand enemysHand  = new Hand();

    public Player player = null;
    public Player enemy  = null;

    public List<CardData> cards = new List<CardData>();

    public Sprite[] healthNumbers = new Sprite[10];
    public Sprite[] damageNumbers = new Sprite[10];

    public GameObject cardPrefab = null;
    public Canvas     canvas     = null;

    public bool isPlayable = false;

    public GameObject effectFromLeftPrefab  = null;
    public GameObject effectFromRightPrefab = null;

    public Sprite fireBallImage       = null;
    public Sprite iceBallImage        = null;
    public Sprite multiFireBallImage  = null;
    public Sprite multiIceBallImage   = null;
    public Sprite fireAndIceBallImage = null;
    public Sprite destructBallImage = null;

    public bool playersTurn = true;

    public TextMeshProUGUI turnText  = null;
    public TextMeshProUGUI scoreText = null;

    public int playerScore = 0;
    public int playerKills = 0;

    public Image enemySkipTurn = null;

    public Sprite fireDemon = null;
    public Sprite iceDemon = null;

    public AudioSource playerDieAudio  = null;
    public AudioSource enemyDieAudio   = null;

    private void Awake(){
        instance = this;

        setUpEnemy();

        playerDeck.create();
        enemyDeck.create();

        StartCoroutine(dealHands());
    }

    public void Quit(){
        SceneManager.LoadScene(0);
    }

    public void SkipTurn(){
        if (playersTurn && isPlayable)
            nextPlayersTurn();
    }

    internal IEnumerator dealHands(){
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 3; i++){
            playerDeck.dealCard(playersHand);
            enemyDeck.dealCard(enemysHand);
            yield return new WaitForSeconds(1);
        }
        isPlayable = true;
    }

    internal bool useCard(Card card, Player usingOnPlayer, Hand fromHand){
        if (!cardValid(card, usingOnPlayer, fromHand))
            return false;

        isPlayable = false;

        castCard(card, usingOnPlayer, fromHand);

        player.glowImage.gameObject.SetActive(false);
        enemy.glowImage.gameObject.SetActive(false);

        fromHand.removeCard(card);

        return false;
    }

    internal bool cardValid(Card cardBeingPlayed, Player usingOnPlayer, Hand fromHand){
        bool valid = false;

        if (cardBeingPlayed == null)
            return false;

        if (fromHand.isPlayers){
            if (cardBeingPlayed.cardData.cost <= player.mana){
                if (usingOnPlayer.isPlayer && cardBeingPlayed.cardData.isDefenseCard)
                    valid = true;
                if (!usingOnPlayer.isPlayer && !cardBeingPlayed.cardData.isDefenseCard)
                    valid = true;
            }
        }
        else {
            if (cardBeingPlayed.cardData.cost <= enemy.mana){
                if (!usingOnPlayer.isPlayer && cardBeingPlayed.cardData.isDefenseCard)
                    valid = true;
                if (usingOnPlayer.isPlayer && !cardBeingPlayed.cardData.isDefenseCard)
                    valid = true;
            }
        }
        return valid;
    }

    internal void castCard(Card card, Player usingOnPlayer, Hand fromHand){
        if (card.cardData.isMirrorCard){
            usingOnPlayer.setMirror(true);
            usingOnPlayer.playMirrorSound();
            nextPlayersTurn();
            isPlayable = true;
        }
        else {
            if (card.cardData.isDefenseCard){
                usingOnPlayer.health += card.cardData.damage;
                usingOnPlayer.playHealSound();

                if (usingOnPlayer.health > usingOnPlayer.maxHealth)
                    usingOnPlayer.health = usingOnPlayer.maxHealth;

                updateHealths();

                StartCoroutine(castHealEffect(usingOnPlayer));
            }
            else{
                castAttackEffect(card,usingOnPlayer);
            }
            
            if (fromHand.isPlayers)
                playerScore += card.cardData.damage;
            
             updateScore();
        }
        
        if (fromHand.isPlayers){
            GameController.instance.player.mana -= card.cardData.cost;
            GameController.instance.player.updateManaBalls();
        }
        else{
            GameController.instance.enemy.mana -= card.cardData.cost;
            GameController.instance.enemy.updateManaBalls();
        }
    }

    private IEnumerator castHealEffect(Player usingOnPlayer){
        yield return new WaitForSeconds(0.5f);
        nextPlayersTurn();
        isPlayable = true;
    }

    internal void castAttackEffect(Card card, Player usingOnPlayer){
        GameObject effectGO = null;
        if (usingOnPlayer.isPlayer)
            effectGO = Instantiate(effectFromRightPrefab, canvas.gameObject.transform);
        else
            effectGO = Instantiate(effectFromLeftPrefab, canvas.gameObject.transform);

        Effect effect = effectGO.GetComponent<Effect>();
        if (effect){
            effect.targetPlayer = usingOnPlayer;
            effect.sourceCard   = card;

            switch(card.cardData.damageType){
                case CardData.DamageType.Fire:
                    if (card.cardData.isMulti)
                        effect.effectImage.sprite = multiFireBallImage;
                    else
                        effect.effectImage.sprite = fireBallImage;
                    effect.playFireballSound();
                break;
                case CardData.DamageType.Ice:
                    if (card.cardData.isMulti)
                        effect.effectImage.sprite = multiIceBallImage;
                    else
                        effect.effectImage.sprite = iceBallImage;
                    effect.playIceSound();
                break;
                case CardData.DamageType.Both:
                    effect.effectImage.sprite = fireAndIceBallImage;
                    effect.playFireballSound();
                    effect.playIceSound();
                break;
                case CardData.DamageType.Destruct:
                    if (card.cardData.isDestruct)
                        effect.effectImage.sprite = destructBallImage;
                    effect.playDestructSound();
                    break;
            }
        }
    }

    internal void updateHealths(){
        player.updateHealth();
        enemy.updateHealth();

        if (player.health <= 0){
            StartCoroutine(gameOver());
        }
        if (enemy.health <= 0){
            playerKills++;
            playerScore += 100;
            updateScore();
            StartCoroutine(newEnemy());
        }
    }

    private IEnumerator newEnemy(){
        enemy.gameObject.SetActive(false);
        enemysHand.clearHand();
        yield return new WaitForSeconds(0.75f);
        setUpEnemy();
        enemy.gameObject.SetActive(true);
        StartCoroutine(dealHands());
    }

    private void setUpEnemy(){
        enemy.mana = 0;
        enemy.health = 5;
        enemy.updateHealth();
        enemy.isFire = true;
        if (UnityEngine.Random.Range(0,2) == 1)
            enemy.isFire = false;
        if (enemy.isFire)
            enemy.playerImage.sprite = fireDemon;
        else
            enemy.playerImage.sprite = iceDemon;
    }

    private IEnumerator gameOver(){
        yield return new WaitForSeconds(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    internal void nextPlayersTurn(){
        playersTurn = !playersTurn;
        bool enemyIsDead = false;
        if (playersTurn)
        {
            if (player.mana < 5)
                player.mana++;
        }
        else 
        {
            if (enemy.health > 0)
            {
                if (enemy.mana < 5)
                    enemy.mana++;
            }
            else
                enemyIsDead = true;
         }
        if (enemyIsDead)
        {
            playersTurn = !playersTurn;
            if (player.mana < 5)
                player.mana++;
        }
        else
        {
            setTurnText();
            if (!playersTurn)
                monstersTurn();
        }

        player.updateManaBalls();
        enemy.updateManaBalls();
    }

    internal void setTurnText(){
        if (playersTurn){
            turnText.text = "Merlin's Turn";
        }
        else{
            turnText.text = "Enemys Turn";
        }
    }

    private void monstersTurn(){
        Card card = AIChooseCard();
        StartCoroutine(monsterCastCard(card));
    }

    private Card AIChooseCard(){
        List<Card> available = new List<Card>();
        for(int i=0; i<3; i++){
            if (cardValid(enemysHand.cards[i], enemy, enemysHand))
                available.Add(enemysHand.cards[i]);
            else if (cardValid(enemysHand.cards[i], player, enemysHand))
                available.Add(enemysHand.cards[i]);
        }

        if (available.Count ==0) {
            nextPlayersTurn();
            return null;
        }
        int choice = UnityEngine.Random.Range(0, available.Count);
        return available[choice];
    }

    private IEnumerator monsterCastCard(Card card){
        yield return new WaitForSeconds(0.5f);

        if (card){
            turnCard(card);

            yield return new WaitForSeconds(2);

            if (card.cardData.isDefenseCard)
                useCard(card, enemy, enemysHand);
            else
                useCard(card, player, enemysHand);

            yield return new WaitForSeconds(1);

            enemyDeck.dealCard(enemysHand);            

            yield return new WaitForSeconds(1);
        }
        else{
            enemySkipTurn.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            enemySkipTurn.gameObject.SetActive(false);
        }
    }

    internal void turnCard(Card card){
        Animator animator = card.GetComponentInChildren<Animator>();
        if (animator){
            animator.SetTrigger("Flip");
        }
        else
            Debug.LogError("no Animator found.");
    }

    private void updateScore(){
        scoreText.text = "Demons killed: " +playerKills.ToString()+ ".  Score: " +playerScore.ToString();
    }

    internal void playPlayerDieSound(){
        playerDieAudio.Play();
    }

    internal void playEnemyDieSound(){
        enemyDieAudio.Play();
    }
}
