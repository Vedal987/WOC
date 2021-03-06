﻿using System.Collections;
using System.Xml.Serialization;
using System.IO;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;	


public class Main : MonoBehaviour {

	public const string path = "Creature";

	public GameObject TextTest;

	public bool canMove = true;
	public bool dialogue = false;
	public bool start = true;
	public GameObject creature;
	public GameObject creatureAgainst;
	public GameObject BattleUI;
	public GameObject BagUI;
	public GameObject BagText;
	public GameObject model;
	public GameObject DialogueBox;
	public GameObject DialogueText;
	public GameObject Flash;
	public Animator modelAnimator;
	float playerSpeed = 4f;

	public GameObject lastInteract;

	public GameObject mapVillage1;
	public GameObject mapTitan;
	public GameObject mapTitan1;
	public GameObject mapGrass;


	public GameObject Battle;
	public GameObject BattleMenu;
	public GameObject BattleFight;
	public GameObject BattleBag;
	public GameObject BattleTalk;

	public GameObject ClickNo;
	public GameObject EnemyDie;

	public GameObject Question;
	public GameObject Asker;
	public GameObject Options;

	//TitanHQ
	public GameObject TitanManager;
	public GameObject demonAttackCamera;
	public GameObject camera;
	private Animator daAnimator;
	public GameObject demons;
	public GameObject Ariel2;
	public GameObject Ariel3;
	public GameObject guard;
	public GameObject Hell;
	public AudioClip HowCouldYou;
	public AudioClip FlightOfTheDemon;

	//GrassArea
	public GameObject SeaDemon;
	public bool SeaBattle;
	public GameObject GwenFake;
	public GameObject Gwen;

	public GameObject BattleText;
	public GameObject BattleTooltip;

	public List<string> Bag;
	public bool isBag;

	public bool inBattle;
	public bool BattleSkip;
	public bool exitingBattle;

	public string LastKeyPress;
	public string direction;
	Vector2 dir;

	private string str;

	public bool canSkip = true;
	public bool canBack = false;
	public bool slowText = false;

	public AudioClip Grass;

	public bool canEncounter;

	public Texture2D KaiImage;
	public Texture2D SeaDemonImage;
	public Texture2D GwenImage;

	public GameObject SeaDemonPrompt;

	public GameObject TitleScreen;
	public AudioClip TitleScreenMusic;

	public GameObject Rain;



	//SAVE POINTS
	public int SeaDemonPoint;
	public int SeaDemonDonePoint;

	//InBattle

	public List<BattleMove> Moves = new List<BattleMove>();
	public List<BattleMove> EnemyMoves = new List<BattleMove>();
	public bool isTurn;


	void Start () {
		Rain.GetComponent<RainCameraController> ().Stop();
		modelAnimator = model.GetComponent<Animator> ();
		//StartCoroutine ("StartBattle");
		if (PlayerPrefs.GetInt ("Flash") == 1) {
			Flash.SetActive (true);
			DialogueBox.SetActive (false);
			Flash.GetComponent<Animation> ().Play ();
			canMove = true;
			dialogue = false;
			start = false;
			canSkip = true;
		} else {
			Flash.SetActive (true);
			mapGrass.SetActive (false);
			mapTitan.SetActive (false);
			mapVillage1.SetActive (true);
		}
		daAnimator = demonAttackCamera.GetComponent<Animator> ();
	}

	void Awake()
	{
		Vector3 newPos = new Vector3(PlayerPrefs.GetFloat ("PlayerXPos"), PlayerPrefs.GetFloat ("PlayerYPos"), 0);
		Vector3 newCamPos = new Vector3(PlayerPrefs.GetFloat ("PlayerXPos"), PlayerPrefs.GetFloat ("PlayerYPos"), -10);
		this.gameObject.transform.position = newPos;
		camera.gameObject.transform.position = newCamPos;
		LoadCreature (PlayerPrefs.GetString ("Creature"));
		if (PlayerPrefs.GetInt ("SeaDemon") == 1) {
			SeaDemonPoint = 1;
			SeaDemon.SetActive (true);
			SeaDemonPrompt.SetActive (false);
			Bag.Remove ("Potion Of Healing");
		}
		if (PlayerPrefs.GetInt ("SeaDemonDone") == 1) {
			SeaDemonDonePoint = 1;
			Gwen.SetActive (true);
			SeaDemon.SetActive (false);
			Bag.Remove ("Potion Of Healing");
		}
		if (PlayerPrefs.GetString ("SaveArea") == "GRASS") {
			GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource>().clip = Grass;
			GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().Play ();
			Bag.Add ("Potion Of Healing");
			mapGrass.SetActive (true);
			mapTitan.SetActive (false);
			mapVillage1.SetActive (false);

		}
		GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().currentM = PlayerPrefs.GetInt("Music");
	}

	IEnumerator Death()
	{
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene ("Menu");
	}
		
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.F8)) {
			DialogueBox.SetActive (false);
			canMove = true;
			dialogue = false;
			canSkip = true;
		}
		if (GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().currentM == 9) {
			Rain.GetComponent<RainCameraController> ().Stop ();
		}
		if (TitanManager.GetComponent<ManagerTitanHQ> ().FinFight) {
			guard.GetComponent<InteractObject> ().Option = 2;
		}
		if (inBattle) {
			if (creatureAgainst.GetComponent<Creature> ().health < 1 && !exitingBattle) {
				StartCoroutine (ExitBattle ());
				exitingBattle = true;
			}
			if (creature.GetComponent<Creature> ().health < 1 && !exitingBattle) {
				creature.GetComponent<Creature> ().health = 0;
				if (SeaBattle) {
					StartCoroutine (SeaBattleAfter());
					exitingBattle = true;
					return;
				}
				StartCoroutine (Death ());
			}
			if (isTurn) {
				if (Input.GetKeyDown (KeyCode.Q)) {
					BattleMenu.SetActive (true);
					BattleFight.SetActive (false);
					BattleBag.SetActive (false);
					BattleTalk.SetActive (false);
					this.GetComponent <AudioSource> ().Play ();
				}
			} else {
				BattleMenu.SetActive (false);
				BattleFight.SetActive (false);
			}
		}
		if (start) {
			if (Input.GetKeyDown(KeyCode.E)) {
				Flash.SendMessage ("Interact");
			}
			return;
		}
		if (!dialogue) {
			if (Input.GetKeyDown(KeyCode.Q) && !inBattle) {
				isBag = !isBag;
				this.GetComponent <AudioSource> ().Play ();
			}
			if (isBag) {
				GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
				modelAnimator.enabled = false;
				canMove = false;
				BagUI.SetActive (true);
				string Items = "";
				foreach (string item in Bag) {
					Items = Items + item + "\n";
				}
				BagText.GetComponent<Text> ().text = Items;
			} else {
				modelAnimator.enabled = true;
				canMove = true;
				BagUI.SetActive (false);
			}
		}
		if (canMove) {
			if (GetComponent<Rigidbody2D> ().velocity.x == 0 && GetComponent<Rigidbody2D> ().velocity.y == 0) {
				modelAnimator.enabled = false;
			} else {
				modelAnimator.enabled = true;
			}
			float hor = Input.GetAxisRaw ("Horizontal");
			float ver = Input.GetAxisRaw ("Vertical");
			modelAnimator.SetFloat ("Horizontal", hor);
			modelAnimator.SetFloat ("Vertical", ver);
			if (Input.GetKeyUp (KeyCode.W)) {
				LastKeyPress = "W";
			}
			if (Input.GetKeyUp (KeyCode.A)) {
				LastKeyPress = "A";
			}
			if (Input.GetKeyUp (KeyCode.S)) {
				LastKeyPress = "S";
			}
			if (Input.GetKeyUp (KeyCode.D)) {
				LastKeyPress = "D";
			}

			if (Input.GetKeyDown (KeyCode.W)) {
				direction = "W";
			} else if (Input.GetKeyDown (KeyCode.A)) {
				direction = "A";
			} else if (Input.GetKeyDown (KeyCode.S)) {
				direction = "S";
			} else if (Input.GetKeyDown (KeyCode.D)) {
				direction = "D";
			}
//			if (!Input.GetKey (KeyCode.W)) {
//				if (!Input.GetKey (KeyCode.A)) {
//					if (!Input.GetKey (KeyCode.S)) {
//						if (!Input.GetKey (KeyCode.D)) {
//							modelAnimator.SetTrigger ("Reset");
//						}
//					}
//				}
//			}
			if (Input.GetKeyDown (KeyCode.E)) {
				if (LastKeyPress == "W") {
					dir = Vector2.up;
				}
				if (LastKeyPress == "A") {
					dir = Vector2.left;
				}
				if (LastKeyPress == "S") {
					dir = Vector2.down;
				}
				if (LastKeyPress == "D") {
					dir = Vector2.right;
				}
				RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, 1.8f);
				if (hit.collider != null) {
					if (hit.collider.gameObject.GetComponent<InteractObject> () || hit.collider.gameObject.GetComponent<Ariel> ()) {
						hit.collider.gameObject.SendMessage ("Interact");
					}
				}

			}
		} else {
			if (dialogue) {
				if (canSkip) {
					GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
					modelAnimator.enabled = false;
					if (Input.GetKeyDown (KeyCode.E)) {
						if (LastKeyPress == "W") {
							dir = Vector2.up;
						}
						if (LastKeyPress == "A") {
							dir = Vector2.left;
						}
						if (LastKeyPress == "S") {
							dir = -Vector2.up;
						}
						if (LastKeyPress == "D") {
							dir = -Vector2.left;
						}
						RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, 1.8f);
						if (hit.collider != null) {
							if (hit.collider.gameObject.GetComponent<InteractObject> () || hit.collider.gameObject.GetComponent<Ariel> ()) {
								hit.collider.gameObject.SendMessage ("Interact");
							}
						}
					}
					if (Input.GetKeyDown (KeyCode.Q) && canBack == true) {
						lastInteract.GetComponent<InteractObject> ().d = 0;
						DialogueBox.SetActive (false);
						canMove = true;
						dialogue = false;
						canSkip = true;
					}
				}
				
			} else {
				modelAnimator.enabled = true;
			}
		}
		if (!Input.anyKey) {
			if (LastKeyPress == "W") {
				modelAnimator.SetBool ("IdleUp", true);
				modelAnimator.SetBool ("IdleRight", false);
				modelAnimator.SetBool ("IdleDown", false);
				modelAnimator.SetBool ("IdleLeft", false);
			}
			if (LastKeyPress == "A") {
				modelAnimator.SetBool ("IdleLeft", true);
				modelAnimator.SetBool ("IdleRight", false);
				modelAnimator.SetBool ("IdleDown", false);
				modelAnimator.SetBool ("IdleUp", false);
			}
			if (LastKeyPress == "S") {
				modelAnimator.SetBool ("IdleDown", true);
				modelAnimator.SetBool ("IdleRight", false);
				modelAnimator.SetBool ("IdleLeft", false);
				modelAnimator.SetBool ("IdleUp", false);
			}
			if (LastKeyPress == "D") {
				modelAnimator.SetBool ("IdleRight", true);
				modelAnimator.SetBool ("IdleDown", false);
				modelAnimator.SetBool ("IdleLeft", false);
				modelAnimator.SetBool ("IdleUp", false);
			}
		} 

	}

	IEnumerator SeaBattleAfter()
	{
		yield return new WaitForSeconds (2f);
		inBattle = false;
		SeaBattle = false;
		Vector3 scale = BattleUI.GetComponent<RectTransform> ().localScale;
		scale.x = 0;
		scale.y = 0;
		BattleUI.GetComponent<RectTransform> ().localScale = scale;
		Battle.SetActive (false);
		canMove = false;
		dialogue = false;
		GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
		creatureAgainst.GetComponent<Creature> ().health = creatureAgainst.GetComponent<Creature> ().MaxHealth;
		exitingBattle = false;
		SeaDemon.GetComponent<Animator> ().SetTrigger ("Spawn");
		yield return new WaitForSeconds (7f);
		GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
		canMove = true;
		GwenFake.SetActive (false);
		Gwen.SetActive (true);
		creature.GetComponent<Creature> ().health = creature.GetComponent<Creature> ().MaxHealth;
	}

	public void QuestionYes()
	{
		GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
		TitleScreen.SetActive (true);
	}

	public void QuestionNo()
	{
		GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
		TitleScreen.SetActive (true);
	}

	IEnumerator QuestionActivate()
	{
		GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
		Question.SetActive (true);
		yield return new WaitForSeconds (2f);
		Asker.SetActive (true);
		yield return new WaitForSeconds (2f);
		Asker.GetComponent<InteractObject> ().Interact ();
		yield return new WaitForSeconds (5f);
		Options.SetActive (true);
	}

	IEnumerator TitleAfterTime()
	{
		yield return new WaitForSeconds (9f);
		GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().clip = TitleScreenMusic;
		GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().Play ();
		TitleScreen.SetActive (true);
		Hell.SetActive (false);
	}

	IEnumerator ReloadAfterTime()
	{
		yield return new WaitForSeconds (0.5f);
		SceneManager.LoadScene ("Main");
	}

	public void Dialogue(string d, GameObject interact)
	{
		slowText = false;
		string a;
		lastInteract = interact;
		if(d.Contains("[BACK]"))
		{
			canBack = true;
			a = d.Replace ("[BACK]", "");
			d = a;
		} else {
			canBack = false;
		}
		if (canSkip) {
			canSkip = false;
			if (d.Contains ("x7Item")) {
				d = "You found a" + d.Replace ("x7Item", "");
				string Item = d.Remove (0, 12);
				if (Bag.Count == 8) {
					d = d + "\nBut your bag is full.";
				} else {
					Bag.Add (Item);
				}

			}
			if (d == "x7Finish") {
				DialogueBox.SetActive (false);
				canMove = true;
				dialogue = false;
				canSkip = true;
				return;
			}
			if (d == "x7Start") {
				DialogueBox.SetActive (false);
				Flash.GetComponent<Animation> ().Play ();
				canMove = true;
				dialogue = false;
				start = false;
				canSkip = true;
				return;
			}
			if (d.Contains("*Saving Game*")) {
				PlayerPrefs.SetFloat ("PlayerXPos", this.gameObject.transform.position.x);
				PlayerPrefs.SetFloat ("PlayerYPos", this.gameObject.transform.position.y);
				PlayerPrefs.SetInt ("Flash", 1);
				PlayerPrefs.SetInt ("Music", GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().currentM);
				PlayerPrefs.SetInt ("SeaDemon", SeaDemonPoint);
				PlayerPrefs.SetInt ("SeaDemonDone", SeaDemonDonePoint);
				StartCoroutine (ReloadAfterTime ());
			}
			if (d.Contains ("[GRASS]")) {
				PlayerPrefs.SetString ("SaveArea", "GRASS");
				a = d.Replace ("[GRASS]", "");
				d = a;
			}
			if (d.Contains ("[SLOW]")) {
				a = d.Replace ("[SLOW]", "");
				d = a;
				slowText = true;
			}
			if (d.Contains ("[QUESTION]")) {
				StartCoroutine (QuestionActivate ());
				DialogueBox.SetActive (false);
				canMove = false;
				dialogue = false;
				canSkip = true;
				return;
			}
			if (d == "ARAGGHHH") {
				daAnimator.SetTrigger ("2");
				demons.GetComponent<Animator> ().SetTrigger ("Jump");
				StartCoroutine("DemonJumpWait");
			}
			if (d.Contains ("[NAME]")) {
				string name = PlayerPrefs.GetString ("Name");
				d = d.Replace ("[NAME]", name);
			}
			if (d == "[FIGHT]" || d == "[FIGHT2]") {
				StartCoroutine ("StartBattle");
				canMove = false;
				dialogue = false;
				canSkip = true;
				return;
			}
			if (d == "[HELL]") {
				Ariel3.GetComponent<Animator> ().SetTrigger ("Suicide");
				Destroy (Ariel3, 6f);
				StartCoroutine (TitleAfterTime ());
				DialogueBox.SetActive (false);
				canMove = true;
				dialogue = false;
				canSkip = true;
				return;
			}
			if (d == "[SEADEMON]") {
				Bag.Remove ("Potion Of Healing");
				creatureAgainst.GetComponent<RawImage> ().texture = SeaDemonImage;
				interact.SetActive (false);
				DialogueBox.SetActive (false);
				canMove = true;
				dialogue = false;
				canSkip = true;
				StartCoroutine (AnimateSeaDemon ());
				return;
			}
			if (d == "*Ariel mutters random words*") {
				StartCoroutine ("FlashCamera");
				mapTitan.SetActive (true);
				mapVillage1.SetActive (false);
			}
			if (d.Contains( "*You received Kai*")) {
				LoadCreature ("Kai");
				mapGrass.SetActive (true);
				mapTitan1.SetActive (false);
			}
			DialogueBox.SetActive (true);
			canMove = false;
			dialogue = true;
			StartCoroutine (AnimateText (d));
		}
	}

	public IEnumerator AnimateSeaDemon()
	{
		SeaDemon.SetActive (true);
		SeaBattle = true;
		yield return new WaitForSeconds (3f);
		GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
		LoadEnemyCreature("SeaDemon");
		SeaDemon.GetComponent<InteractObject> ().enabled = true;
	}

	public void LoadEnemyCreature(string cname)
	{
		CreatureContainer cc = CreatureContainer.Load (path);
		foreach (CreatureTemplate creaturev in cc.creatures) {
			if (creaturev.Name == cname) {
				creatureAgainst.GetComponent<Creature> ().CreatureName = cname;

				creatureAgainst.GetComponent<Creature> ().name1 = creaturev.name1;
				creatureAgainst.GetComponent<Creature> ().damage1 = creaturev.damage1;
				creatureAgainst.GetComponent<Creature> ().heal1 = creaturev.heal1;
				creatureAgainst.GetComponent<Creature> ().details1 = creaturev.details1;
				creatureAgainst.GetComponent<Creature> ().tooltip1 = creaturev.tooltip1;

				creatureAgainst.GetComponent<Creature> ().name2 = creaturev.name2;
				creatureAgainst.GetComponent<Creature> ().damage2 = creaturev.damage2;
				creatureAgainst.GetComponent<Creature> ().heal2 = creaturev.heal2;
				creatureAgainst.GetComponent<Creature> ().details2 = creaturev.details2;
				creatureAgainst.GetComponent<Creature> ().tooltip2 = creaturev.tooltip2;

				creatureAgainst.GetComponent<Creature> ().name3 = creaturev.name3;
				creatureAgainst.GetComponent<Creature> ().damage3 = creaturev.damage3;
				creatureAgainst.GetComponent<Creature> ().heal3 = creaturev.heal3;
				creatureAgainst.GetComponent<Creature> ().details3 = creaturev.details3;
				creatureAgainst.GetComponent<Creature> ().tooltip3 = creaturev.tooltip3;

				creatureAgainst.GetComponent<Creature> ().name4 = creaturev.name4;
				creatureAgainst.GetComponent<Creature> ().damage4 = creaturev.damage4;
				creatureAgainst.GetComponent<Creature> ().heal4 = creaturev.heal4;
				creatureAgainst.GetComponent<Creature> ().details4 = creaturev.details4;
				creatureAgainst.GetComponent<Creature> ().tooltip4 = creaturev.tooltip4;

				creatureAgainst.GetComponent<Creature> ().health = creaturev.health;
				creatureAgainst.GetComponent<Creature> ().MaxHealth = creaturev.MaxHealth;
				creatureAgainst.GetComponent<Creature> ().Level = creaturev.Level;

				ChangeEnemyMoves();

			}
		}
	}

	public void LoadCreature(string cname)
	{
		CreatureContainer cc = CreatureContainer.Load (path);
		foreach (CreatureTemplate creaturev in cc.creatures) {
			if (creaturev.Name == cname) {
				creature.GetComponent<Creature> ().CreatureName = cname;

				creature.GetComponent<Creature> ().name1 = creaturev.name1;
				creature.GetComponent<Creature> ().damage1 = creaturev.damage1;
				creature.GetComponent<Creature> ().heal1 = creaturev.heal1;
				creature.GetComponent<Creature> ().details1 = creaturev.details1;
				creature.GetComponent<Creature> ().tooltip1 = creaturev.tooltip1;

				creature.GetComponent<Creature> ().name2 = creaturev.name2;
				creature.GetComponent<Creature> ().damage2 = creaturev.damage2;
				creature.GetComponent<Creature> ().heal2 = creaturev.heal2;
				creature.GetComponent<Creature> ().details2 = creaturev.details2;
				creature.GetComponent<Creature> ().tooltip2 = creaturev.tooltip2;

				creature.GetComponent<Creature> ().name3 = creaturev.name3;
				creature.GetComponent<Creature> ().damage3 = creaturev.damage3;
				creature.GetComponent<Creature> ().heal3 = creaturev.heal3;
				creature.GetComponent<Creature> ().details3 = creaturev.details3;
				creature.GetComponent<Creature> ().tooltip3 = creaturev.tooltip3;

				creature.GetComponent<Creature> ().name4 = creaturev.name4;
				creature.GetComponent<Creature> ().damage4 = creaturev.damage4;
				creature.GetComponent<Creature> ().heal4 = creaturev.heal4;
				creature.GetComponent<Creature> ().details4 = creaturev.details4;
				creature.GetComponent<Creature> ().tooltip4 = creaturev.tooltip4;

				creature.GetComponent<Creature> ().health = creaturev.health;
				creature.GetComponent<Creature> ().MaxHealth = creaturev.MaxHealth;
				creature.GetComponent<Creature> ().Level = creaturev.Level;

				ChangeMoves ();
				PlayerPrefs.SetString ("Creature", cname);
				if (cname == "Kai") {
					creature.GetComponent<RawImage> ().texture = KaiImage;
				}
			}
		}
	}

	IEnumerator DemonJumpWait()
	{
		yield return new WaitForSeconds (3);
		demonAttackCamera.SetActive (false);
		camera.SetActive (true);
		Rain.GetComponent<RainCameraController> ().Play ();
		DialogueBox.SetActive (false);
		canMove = true;
		dialogue = false;
		canSkip = true;
		Ariel2.SendMessage ("StartDemonHelp");
	}

	IEnumerator FlashCamera()
	{
		Flash.GetComponent<SpriteRenderer> ().color = Color.black;
		yield return new WaitForSeconds (5f);
		Flash.GetComponent<Animation> ().Play ();
	}

	List<string> WrapText(string text, double pixels)
	{
		string[] originalLines = text.Split(' ');

		List<string> wrappedLines = new List<string>();

		StringBuilder actualLine = new StringBuilder();
		double actualWidth = 0;
		string temp;

		foreach (var item in originalLines)
		{
			temp = actualLine.ToString();
			actualLine.Append(item + " ");
			TextTest.GetComponent<Text> ().text = actualLine.ToString ();
			Canvas.ForceUpdateCanvases ();
			actualWidth = TextTest.GetComponent<RectTransform> ().rect.width;

			if (actualWidth > pixels)
			{
				//Too big - undo last add!
				wrappedLines.Add(temp);
				actualLine.Remove(0, actualLine.Length);
				actualWidth = 0;
				actualLine.Append (item + " ");
			} 
		}

		if(actualLine.Length > 0)
			wrappedLines.Add(actualLine.ToString());

		return wrappedLines;
	}
		

	IEnumerator AnimateText(string strComplete){
		/*
		int i = 0;
		str = "";
		while( i < strComplete.Length ){
			str += strComplete[i++];
			DialogueText.GetComponent<Text> ().text = str;
			if (slowText) {
				if (strComplete [i].ToString() == "." || strComplete [i].ToString() == ",") {
					yield return new WaitForSeconds (0.2F);
				}
				yield return new WaitForSeconds (0.09F);
			} else {
				if (strComplete [i].ToString() == "." || strComplete [i].ToString() == ",") {
					yield return new WaitForSeconds (0.2F);
				}
				yield return new WaitForSeconds (0.03F);
			}
			if (Input.GetKey (KeyCode.E) && i > 4) {
				canSkip = true;
				DialogueText.GetComponent<Text> ().text = strComplete;
				yield break;
			}
		}
		canSkip = true;
		int i = 0;
		str = "";
		bool waitingForTag = false;
		while (i < strComplete.Length) {
			
			str = strComplete.Insert (i, "<color=#00000000>");
			str = str.Insert (str.Length, "</color>");
			DialogueText.GetComponent<Text> ().text = str;
			if (slowText) {
				yield return new WaitForSeconds (0.09F);
			} else {
				yield return new WaitForSeconds (0.03F);
			}
			if (Input.GetKey (KeyCode.E) && i > 4) {
				canSkip = true;
				DialogueText.GetComponent<Text> ().text = strComplete;
				yield break;
			}
			i++;
		}
		DialogueText.GetComponent<Text> ().text = strComplete;
		canSkip = true;
		*/

		DialogueText.GetComponent<Text> ().text = "";

		int i = 0;

		bool bold = false;
		bool red = false;
		bool orange = false;
		bool italics = false;

		bool ignore = false;
		bool skipping = false;
		string wrappedStrComplete = "";

		List<string> lines = WrapText(strComplete, 448.5f);

		foreach (var item in lines) {

			wrappedStrComplete += item + "\n";

		}



		foreach(char nextletter in wrappedStrComplete.ToCharArray())
		{
			switch (nextletter) {
				case '@':
					ignore = true;
					red = !red;
				break;
				case '`':
					ignore = true;
					orange = !orange;
				break;
				case '¬':
					ignore = true;
					bold = !bold;
					break;
			case '*':
				ignore = true;
				italics = !italics;
				break;
			}

			string letter = nextletter.ToString ();

			if (!ignore) {
				
				if (bold){
					letter = "<b>"+letter+"</b>";
				}
				if (italics){
					letter = "<i>"+letter+"</i>";
				}
				if (red){
					letter = "<color=#ff0000>"+letter+"</color>";
				}
				if (orange){
					letter = "<color=#ffa500ff>"+letter+"</color>";
				}
				DialogueText.GetComponent<Text> ().text += letter;
			}
			ignore = false;
			if (Input.GetKey (KeyCode.E) && i > 4 && !skipping) {
				//DialogueText.GetComponent<Text> ().text = strComplete;
				skipping = true;
			}
			if (!skipping) {
				if (slowText) {
					if (letter == "." || letter == "," || letter == "!" || letter == "?") {
						yield return new WaitForSeconds (0.2F);
					}
					yield return new WaitForSeconds (0.09F);
				} else {
					if (letter == "." || letter == "," || letter == "!" || letter == "?") {
						yield return new WaitForSeconds (0.2F);
					}
					yield return new WaitForSeconds (0.03F);
				}
			} else {
				yield return new WaitForSeconds (0.0001F);
			}
			i++;
		}
		canSkip = true;
		skipping = false;
	}

	void FixedUpdate () {
		if (canMove) {
			Vector2 targetVelocity = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
			GetComponent<Rigidbody2D> ().velocity = targetVelocity * playerSpeed;
		}
	}
	IEnumerator ExitBattle()
	{
		creatureAgainst.GetComponent<Creature> ().health = 0;
		GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().Pause ();
		Instantiate (EnemyDie);
		yield return new WaitForSeconds (2f);
		inBattle = false;
		SeaBattle = false;
		Vector3 scale = BattleUI.GetComponent<RectTransform> ().localScale;
		scale.x = 0;
		scale.y = 0;
		BattleUI.GetComponent<RectTransform> ().localScale = scale;
		Battle.SetActive (false);
		canMove = true;
		dialogue = false;
		demons.GetComponent<InteractObject> ().Option = 2;
		demons.GetComponent<InteractObject> ().d = 0;	
		Ariel3.SetActive (true);
		if (PlayerPrefs.GetString ("SaveArea") == "GRASS") {
			GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().clip = Grass;
			GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().Play ();
		} else if (Ariel3.GetComponent<InteractObject> ().d == Ariel3.GetComponent<InteractObject> ().Dialogue.Length) {
			GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().clip = Grass;
			GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().Play ();
		} else {
			GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
			GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().Play ();
		}

		if (LastKeyPress == "W") {
			dir = Vector2.up;
		}
		if (LastKeyPress == "A") {
			dir = Vector2.left;
		}
		if (LastKeyPress == "S") {
			dir = -Vector2.up;
		}
		if (LastKeyPress == "D") {
			dir = -Vector2.left;
		}
		creatureAgainst.GetComponent<Creature> ().health = creatureAgainst.GetComponent<Creature> ().MaxHealth;
		exitingBattle = false;
		RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, 1.8f);
		if (hit.collider != null) {
			if (hit.collider.gameObject.GetComponent<InteractObject> () || hit.collider.gameObject.GetComponent<Ariel> ()) {
				hit.collider.gameObject.SendMessage ("Interact");
			}
		}
	}

	IEnumerator FleeBattle()
	{
		GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().Pause ();
		BattleText.GetComponent<Text> ().text = "You fled the battle.";
		BattleMenu.SetActive (false);
		yield return new WaitForSeconds (2f);
		GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().clip = HowCouldYou;
		GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().Play ();
		Hell.SetActive (true);
		inBattle = false;
		Vector3 scale = BattleUI.GetComponent<RectTransform> ().localScale;
		scale.x = 0;
		scale.y = 0;
		BattleUI.GetComponent<RectTransform> ().localScale = scale;
		Battle.SetActive (false);
		canMove = true;
		dialogue = false;
		demons.GetComponent<InteractObject> ().Option = 2;
		demons.GetComponent<InteractObject> ().d = 0;
		Ariel3.SetActive (true);
		creatureAgainst.GetComponent<Creature> ().health = creatureAgainst.GetComponent<Creature> ().MaxHealth;
		exitingBattle = false;
	}

	public void ChangeMoves()
	{
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name1, creature.GetComponent<Creature>().damage1, creature.GetComponent<Creature>().heal1, creature.GetComponent<Creature>().details1));
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name2, creature.GetComponent<Creature>().damage2, creature.GetComponent<Creature>().heal2, creature.GetComponent<Creature>().details2));
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name3, creature.GetComponent<Creature>().damage3, creature.GetComponent<Creature>().heal3, creature.GetComponent<Creature>().details3));
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name4, creature.GetComponent<Creature>().damage4, creature.GetComponent<Creature>().heal4, creature.GetComponent<Creature>().details4));
	}

	public void ChangeEnemyMoves()
	{
		EnemyMoves.Add( new BattleMove(creatureAgainst.GetComponent<Creature>().name1, creatureAgainst.GetComponent<Creature>().damage1, creatureAgainst.GetComponent<Creature>().heal1, creatureAgainst.GetComponent<Creature>().details1));
		EnemyMoves.Add( new BattleMove(creatureAgainst.GetComponent<Creature>().name2, creatureAgainst.GetComponent<Creature>().damage2, creatureAgainst.GetComponent<Creature>().heal2, creatureAgainst.GetComponent<Creature>().details2));
		EnemyMoves.Add( new BattleMove(creatureAgainst.GetComponent<Creature>().name3, creatureAgainst.GetComponent<Creature>().damage3, creatureAgainst.GetComponent<Creature>().heal3, creatureAgainst.GetComponent<Creature>().details3));
		EnemyMoves.Add( new BattleMove(creatureAgainst.GetComponent<Creature>().name4, creatureAgainst.GetComponent<Creature>().damage4, creatureAgainst.GetComponent<Creature>().heal4, creatureAgainst.GetComponent<Creature>().details4));
	}

	IEnumerator StartBattle()
	{
		DialogueBox.SetActive (false);
		canMove = false;
		GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
		BattleUI.GetComponent<Animation> ().Play ("BattleUI_Enter");
		yield return new WaitForSeconds (1);
		inBattle = true;
		BattleUI.GetComponent<RectTransform> ().localScale.Set (1, 1, 1);
		Battle.SetActive (true);
		BattleText.GetComponent<Text> ().text = creatureAgainst.GetComponent<Creature> ().CreatureName + " appears.";
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name1, creature.GetComponent<Creature>().damage1, creature.GetComponent<Creature>().heal1, creature.GetComponent<Creature>().details1));
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name2, creature.GetComponent<Creature>().damage2, creature.GetComponent<Creature>().heal2, creature.GetComponent<Creature>().details2));
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name3, creature.GetComponent<Creature>().damage3, creature.GetComponent<Creature>().heal3, creature.GetComponent<Creature>().details3));
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name4, creature.GetComponent<Creature>().damage4, creature.GetComponent<Creature>().heal4, creature.GetComponent<Creature>().details4));

		BattleFight.transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = Moves [0].name;
		if (BattleFight.transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text == "") {
			BattleFight.transform.GetChild (0).GetComponent<Button> ().interactable = false;
		}
		BattleFight.transform.GetChild (1).transform.GetChild (0).GetComponent<Text> ().text = Moves [1].name;
		if (BattleFight.transform.GetChild (1).transform.GetChild (0).GetComponent<Text> ().text == "") {
			BattleFight.transform.GetChild (1).GetComponent<Button> ().interactable = false;
		}
		BattleFight.transform.GetChild (2).transform.GetChild (0).GetComponent<Text> ().text = Moves [2].name;
		if (BattleFight.transform.GetChild (2).transform.GetChild (0).GetComponent<Text> ().text == "") {
			BattleFight.transform.GetChild (2).GetComponent<Button> ().interactable = false;
		}
		BattleFight.transform.GetChild (3).transform.GetChild (0).GetComponent<Text> ().text = Moves [3].name;
		if (BattleFight.transform.GetChild (3).transform.GetChild (0).GetComponent<Text> ().text == "") {
			BattleFight.transform.GetChild (3).GetComponent<Button> ().interactable = false;
		}
		EnemyMoves.Add( new BattleMove(creatureAgainst.GetComponent<Creature>().name1, creatureAgainst.GetComponent<Creature>().damage1, creatureAgainst.GetComponent<Creature>().heal1, creatureAgainst.GetComponent<Creature>().details1));
		EnemyMoves.Add( new BattleMove(creatureAgainst.GetComponent<Creature>().name2, creatureAgainst.GetComponent<Creature>().damage2, creatureAgainst.GetComponent<Creature>().heal2, creatureAgainst.GetComponent<Creature>().details2));
		EnemyMoves.Add( new BattleMove(creatureAgainst.GetComponent<Creature>().name3, creatureAgainst.GetComponent<Creature>().damage3, creatureAgainst.GetComponent<Creature>().heal3, creatureAgainst.GetComponent<Creature>().details3));
		EnemyMoves.Add( new BattleMove(creatureAgainst.GetComponent<Creature>().name4, creatureAgainst.GetComponent<Creature>().damage4, creatureAgainst.GetComponent<Creature>().heal4, creatureAgainst.GetComponent<Creature>().details4));
	}

	public void FightButton()
	{
		BattleMenu.SetActive (false);
		BattleFight.SetActive (true);
		this.GetComponent <AudioSource> ().Play ();
	}

	public void BagButton()
	{
		BattleMenu.SetActive (false);
		BattleBag.SetActive (true);
		this.GetComponent <AudioSource> ().Play ();
	}

	public void TalkButton()
	{
		BattleMenu.SetActive (false);
		BattleTalk.SetActive (true);
		this.GetComponent <AudioSource> ().Play ();
	}

	public void FleeButton()
	{
		if (creatureAgainst.GetComponent<Creature> ().CreatureName == "SeaDemon") {
			BattleText.GetComponent<Text> ().text = "You cannot flee right now";
			Instantiate (ClickNo);
		} else {
			this.GetComponent <AudioSource> ().Play ();
			if (creatureAgainst.GetComponent<Creature> ().CreatureName == "Kronos") {
				Ariel3.GetComponent<InteractObject> ().Option = 2;
				StartCoroutine (FleeBattle());
			}
		}
	}

	public void Move1()
	{
		creatureAgainst.GetComponent<Creature> ().health -= Moves [0].damage;
		creature.GetComponent<Creature> ().health += Moves [0].heal;
		BattleText.GetComponent<Text> ().text = creature.GetComponent<Creature> ().CreatureName + " used " + Moves [0].name;
		StartCoroutine(EnemyTurn (Moves [0].details));
		this.GetComponent <AudioSource> ().Play ();
	}
	public void Move2()
	{
		creatureAgainst.GetComponent<Creature> ().health -= Moves [1].damage;
		creature.GetComponent<Creature> ().health += Moves [1].heal;
		BattleText.GetComponent<Text> ().text = creature.GetComponent<Creature> ().CreatureName + " used " + Moves [1].name;
		StartCoroutine(EnemyTurn (Moves [1].details));
		this.GetComponent <AudioSource> ().Play ();
	}
	public void Move3()
	{
		creatureAgainst.GetComponent<Creature> ().health -= Moves [2].damage;
		creature.GetComponent<Creature> ().health += Moves [2].heal;
		BattleText.GetComponent<Text> ().text = creature.GetComponent<Creature> ().CreatureName + " used " + Moves [2].name;
		StartCoroutine(EnemyTurn (Moves [2].details));
		this.GetComponent <AudioSource> ().Play ();
	}
	public void Move4()
	{
		creatureAgainst.GetComponent<Creature> ().health -= Moves [3].damage;
		creature.GetComponent<Creature> ().health += Moves [3].heal;
		BattleText.GetComponent<Text> ().text = creature.GetComponent<Creature> ().CreatureName + " used " + Moves [3].name;
		StartCoroutine(EnemyTurn (Moves [3].details));
		this.GetComponent <AudioSource> ().Play ();
	}
		

	IEnumerator EnemyTurn(string details)
	{
		TooltipExit ();
		isTurn = false;
		if (details == "Peace") {
			creature.GetComponent<Animator> ().SetTrigger ("Hit");
			yield return new WaitForSeconds (2f);
		} else if (details == "NoMove") {
			creature.GetComponent<Animator> ().SetTrigger ("Hit");
			creatureAgainst.GetComponent<Animator> ().SetTrigger ("Hit");
			yield return new WaitForSeconds (2f);
		} else if (details == "Blank") {
			creature.GetComponent<Animator> ().SetTrigger ("Hit");
			yield return new WaitForSeconds (2f);
		} else if (details == "SkipSelf") {
			creature.GetComponent<Animator> ().SetTrigger ("Attack");
			yield return new WaitForSeconds (0.15f);
			creatureAgainst.GetComponent<Animator> ().SetTrigger ("Hit");
			yield return new WaitForSeconds (1.85f);
			BattleSkip = true;
		} else {
			creature.GetComponent<Animator> ().SetTrigger ("Attack");
			yield return new WaitForSeconds (0.15f);
			creatureAgainst.GetComponent<Animator> ().SetTrigger ("Hit");
			yield return new WaitForSeconds (1.85f);
		}
		int RandomMove = Random.Range (0, 3);
		creature.GetComponent<Creature> ().health -= EnemyMoves [RandomMove].damage;
		creatureAgainst.GetComponent<Creature> ().health += EnemyMoves [RandomMove].heal;
		BattleText.GetComponent<Text> ().text = creatureAgainst.GetComponent<Creature> ().CreatureName + " used " + EnemyMoves [RandomMove].name;
		string EnemyDetails = EnemyMoves [RandomMove].details;

		if (EnemyDetails == "Peace") {
			creatureAgainst.GetComponent<Animator> ().SetTrigger ("Hit");
			yield return new WaitForSeconds (2f);
		} else if (EnemyDetails == "NoMove") {
			creatureAgainst.GetComponent<Animator> ().SetTrigger ("Hit");
			creature.GetComponent<Animator> ().SetTrigger ("Hit");
			yield return new WaitForSeconds (2f);
		} else {
			creatureAgainst.GetComponent<Animator> ().SetTrigger ("Attack");
			yield return new WaitForSeconds (0.2f);
			creature.GetComponent<Animator> ().SetTrigger ("Hit");
			yield return new WaitForSeconds (1.8f);
		}
		if (BattleSkip) {
			BattleSkip = false;
			BattleText.GetComponent<Text> ().text = "Ariel is recovering...";
			StartCoroutine (EnemyTurn ("Blank"));
		} else {
			isTurn = true;
			BattleMenu.SetActive (true);
			BattleFight.SetActive (false);
		}
		if (LastKeyPress == "W") {
			dir = Vector2.up;
		}
		if (LastKeyPress == "A") {
			dir = Vector2.left;
		}
		if (LastKeyPress == "S") {
			dir = -Vector2.up;
		}
		if (LastKeyPress == "D") {
			dir = -Vector2.left;
		}
		RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, 1.8f);
		if (hit.collider != null) {
			if (hit.collider.gameObject.GetComponent<InteractObject> () || hit.collider.gameObject.GetComponent<Ariel> ()) {
				hit.collider.gameObject.SendMessage ("Interact");
			}
		}
	}


































	//Battle Tooltips
	public void Attack1Enter()
	{
		BattleTooltip.GetComponent<Text> ().text = creature.GetComponent<Creature> ().tooltip1;
	}
	public void Attack2Enter()
	{
		BattleTooltip.GetComponent<Text> ().text = creature.GetComponent<Creature> ().tooltip2;
	}
	public void Defend1Enter()
	{
		BattleTooltip.GetComponent<Text> ().text = creature.GetComponent<Creature> ().tooltip3;
	}
	public void Defend2Enter()
	{
		BattleTooltip.GetComponent<Text> ().text = creature.GetComponent<Creature> ().tooltip4;
	}
	public void TooltipExit()
	{
		BattleTooltip.GetComponent<Text> ().text = "";
	}
}



