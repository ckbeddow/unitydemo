﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSolitaireGame : MonoBehaviour {

	private Deck deck;
	[SerializeField] private Sprite[] glyphImages;
	[SerializeField] private GameObject cardPrefab;

	private float _XLEFT = -5.0f;
	private float _XOFFSET = 1.85f;
	private float _YTOP = 3.5f;
	private float _YOFFSET = 2.2f;

	private GameObject[] cardObjects;
	private Card[] cardScripts;

	private HashSet<int> selectedCards; // the cards,(TODO: by value or by index?) clicked on


	// Use this for initialization
	void Start () {
		deck = new Deck ();
		deck.shuffle ();

		selectedCards = new HashSet<int> ();

		int cardValue;
		int color; // 0-2
		int shape; // 0-2
		int fill; // 0-2
		int count; // 0-2

		int arrayIndexOfCard;

		cardObjects = new GameObject[15];
		cardScripts = new Card[15];

		// testing code
		for(int i=0; i<15; i++){
			cardObjects [i] = Instantiate (cardPrefab) as GameObject;
			cardObjects [i].transform.position = new Vector3 (_XLEFT + _XOFFSET * (i/3), _YTOP - _YOFFSET * (i%3), 0);
			cardScripts[i] = cardObjects[i].GetComponent<Card> ();

			cardScripts [i].setIndex (i); // set the index of each card so we know later when its clicked
			cardScripts[i].parent = this; // so card can invoke members in here!

			// make the card the next one dealt from the deck
			cardValue = deck.getCard();
			cardScripts[i].setCardValue( cardValue );

			color = (cardValue & 3) -1; // 0,1,2
			shape = (cardValue & 48) / 16 -1; // 0,1,2
			fill = (cardValue & 192) / 64 -1; // 0,1,2
			count = (cardValue & 12) / 4 ; // not -1 so 1, 2, or 3

			arrayIndexOfCard = color * 9 + shape * 3 + fill;

			Sprite image = glyphImages [arrayIndexOfCard];
			cardScripts [i].glyph = image;

			cardScripts [i].setCount (count);



		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// do these three cards (by value) make a set?
	private bool isASet(int [] cards){
		//Debug.Log (" [1] " + cards [0] + "     [2] " + cards [1] + "     [3] " + cards [2]); // good

		return false;
	}



	private void checkForThreeCardsSelected(){
		int [] cardArray = new int[3];
		bool result;

		if (selectedCards.Count == 3) {
			//Debug.Log ("Selected Three Cards!");
			// temporarily set all as bad:
			//

			//foreach (int item in selectedCards){
			//	Debug.Log(item);
			//}
			HashSet<int>.Enumerator e = selectedCards.GetEnumerator ();
			//Debug.Log (e.Current);
			e.MoveNext ();
			cardArray [0] = e.Current;
			e.MoveNext ();
			cardArray [1] = e.Current;
			e.MoveNext ();
			cardArray [2] = e.Current;
	

			result = isASet (cardArray);
			if (!result) {
				cardScripts [cardArray [0]].setAsBadCard ();
				cardScripts [cardArray [1]].setAsBadCard ();
				cardScripts [cardArray [2]].setAsBadCard ();

			}
		}
	}

	public void clickedOn(int index){
		// player just clicked on card with index index
		// now figure out what to do
		//Debug.Log(index);
		selectedCards.Add (index);
		checkForThreeCardsSelected();
	}

	public void clickedOff(int index){
		// player just clicked on card with index index
		// now figure out what to do
		//Debug.Log(index);
		selectedCards.Remove (index);
	}

}
