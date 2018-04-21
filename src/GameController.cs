using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {

	public  Sprite[]       spriteList;
	public  GameObject     player1;
	public  GameObject     player2;
	public  Text           score;
	private GameController m_Instance;
	public  GameController Instance { get { return m_Instance; } }
	private int            winner;    // -1: no winner, 0: p1, 1:p2
	private bool           gameOver;  // F: Game in progress, T: Game over
	private Queue<int>     p1Deck;
	private Queue<int>     p2Deck;

	void Awake() {
		m_Instance = this;
	}

	void OnDestroy() {
		m_Instance = null;
	}

	void Update() {
		score.text = "Score : " + p1Deck.Count.ToString ();
		if (gameOver) {
			Debug.Log ("game over");
		}
		// global game update logic goes here
	}

	void OnGui() {
		// common GUI code goes here
	}

	void Start() {
		// Set up all the crapola here
		winner = -1;
		gameOver = false;
		p2Deck = new Queue<int>();
		p1Deck = new Queue<int>();

		// generate a full deck
		int[] deck = new int[52];
		for(int i = 0; i < 52; i++) {
			deck[i] = i;
		}

        // Shuffle the deck
		for (int i = 0; i < deck.Length; i++) {
			int temp = deck [i];
			int rand = Random.Range (i, deck.Length);
			deck [i] = deck [rand];
			deck [rand] = temp;
		}
			
		// Split the deck into two player decks
		for(int i = 0; i < 52; i++) {
			if(i < 25) {
				p1Deck.Enqueue(deck[i]);
			} else {
				p2Deck.Enqueue(deck[i]);
			}
		}
	}

	void OnMouseDown() {
		Debug.Log ("Executing Turn");
		executeTurn();
	}

	private int calcCardValue(int card) {
		if(card > 12) {
			card = card % 12;
		}

		return card;
	}

	public int[] executeTurn() {
		int p1Card = p1Deck.Dequeue();
		int p2Card = p2Deck.Dequeue();

		player1.GetComponent<SpriteRenderer> ().sprite = spriteList [p1Card];
		player2.GetComponent<SpriteRenderer> ().sprite = spriteList [p2Card];

		// P1 has a better card
		if(calcCardValue(p1Card) > calcCardValue(p2Card)) {
			p1Deck.Enqueue(p1Card);
			p1Deck.Enqueue(p2Card);
			gameOver = checkForGameOver();
			return new[] {p1Card, p2Card, 0};

			// P2 has a better card
		} else if(calcCardValue(p1Card) < calcCardValue(p2Card)) {
			p2Deck.Enqueue(p1Card);
			p2Deck.Enqueue(p2Card);
			gameOver = checkForGameOver();
			return new[] {p1Card, p2Card, 1};

			// It's a tie
		} else {
			// P1 has the bigger deck
			if(p1Deck.Count > p2Deck.Count) {
				p1Deck.Enqueue(p1Card);
				p1Deck.Enqueue(p2Card);
				gameOver = checkForGameOver();
				return new[] {p1Card, p2Card, 0};

				// P2 has a bigger deck
			} else {
				p2Deck.Enqueue(p1Card);
				p2Deck.Enqueue(p2Card);
				gameOver= checkForGameOver();
				return new[] {p1Card, p2Card, 1};
			}

			// In any other case, just let P1 win
		} 
	}

	public bool checkForGameOver() {
		if (p1Deck.Count == 0) {
			winner = 0;
			return true;
		} else if (p2Deck.Count == 0) {
			winner = 1;
			return true;
		} else {
			return false;
		}
	}

	public int getWinner() {
		return winner;
	}

    public Queue<int> getP1Deck() {
        return this.p1Deck;
    }

    public Queue<int> getP2Deck() {
        return this.p2Deck;
    }
}
