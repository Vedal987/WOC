using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	public const string path = "Creature";

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

	public GameObject Battle;
	public GameObject BattleMenu;
	public GameObject BattleFight;


	//TitanHQ
	public GameObject TitanManager;
	public GameObject demonAttackCamera;
	public GameObject camera;
	private Animator daAnimator;
	public GameObject demons;
	public GameObject Ariel2;
	public GameObject Ariel3;
	public GameObject guard;

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

	public AudioClip Grass;

	//InBattle

	public List<BattleMove> Moves = new List<BattleMove>();
	public List<BattleMove> EnemyMoves = new List<BattleMove>();
	public bool isTurn;


	void Start () {
		
		modelAnimator = model.GetComponent<Animator> ();
		//StartCoroutine ("StartBattle");
		Flash.SetActive (true);
		daAnimator = demonAttackCamera.GetComponent<Animator> ();
	}

	void Awake()
	{
		Vector3 newPos = new Vector3(PlayerPrefs.GetFloat ("PlayerXPos"), PlayerPrefs.GetFloat ("PlayerYPos"), 0);
		Vector3 newCamPos = new Vector3(PlayerPrefs.GetFloat ("PlayerXPos"), PlayerPrefs.GetFloat ("PlayerYPos"), -10);
		this.gameObject.transform.position = newPos;
		camera.gameObject.transform.position = newCamPos;
		if (PlayerPrefs.GetString ("SaveArea") == "GRASS") {
			GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource>().clip = Grass;
			GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().Play ();
		}
	}
		
	void Update()
	{
		if (TitanManager.GetComponent<ManagerTitanHQ> ().FinFight) {
			guard.GetComponent<InteractObject> ().Option = 2;
		}
		if (inBattle) {
			if (creatureAgainst.GetComponent<Creature> ().health < 1 && !exitingBattle) {
				StartCoroutine (ExitBattle ());
				exitingBattle = true;
			}
			if (creature.GetComponent<Creature> ().health < 1) {
				SceneManager.LoadScene ("Main");
			}
			if (isTurn) {
				if (Input.GetKeyDown (KeyCode.Q)) {
					BattleMenu.SetActive (true);
					BattleFight.SetActive (false);
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

	public void Dialogue(string d, GameObject interact)
	{
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
			if (d.Contains("*Game Saved*")) {
				PlayerPrefs.SetFloat ("PlayerXPos", this.gameObject.transform.position.x);
				PlayerPrefs.SetFloat ("PlayerYPos", this.gameObject.transform.position.y);
				PlayerPrefs.SetInt ("Flash", 1);
			}
			if (d.Contains ("[GRASS]")) {
				PlayerPrefs.SetString ("SaveArea", "GRASS");
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
			if (d == "[FIGHT]") {
				StartCoroutine ("StartBattle");
				canMove = false;
				dialogue = false;
				canSkip = true;
				return;
			}
			if (d == "*Ariel mutters random words*") {
				StartCoroutine ("FlashCamera");
			}
			if (d == "*You recieved Kai*") {
				LoadCreature ("Kai");
			}
			DialogueBox.SetActive (true);
			canMove = false;
			dialogue = true;
			StartCoroutine (AnimateText (d));
		}
	}

	public void LoadCreature(string cname)
	{
		CreatureContainer cc = CreatureContainer.Load (path);
		foreach (CreatureTemplate creaturev in cc.creatures) {
			if (creaturev.Name == cname) {
				creature.GetComponent<Creature> ().Name = cname;

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
			}
		}
	}

	IEnumerator DemonJumpWait()
	{
		yield return new WaitForSeconds (3);
		demonAttackCamera.SetActive (false);
		camera.SetActive (true);
		DialogueBox.SetActive (false);
		canMove = true;
		dialogue = false;
		canSkip = true;
		Ariel2.SendMessage ("StartDemonHelp");
	}

	IEnumerator FlashCamera()
	{
		Flash.GetComponent<SpriteRenderer> ().color = Color.white;
		yield return new WaitForSeconds (5f);
		Flash.GetComponent<Animation> ().Play ();
	}

	IEnumerator AnimateText(string strComplete){
		int i = 0;
		str = "";
		while( i < strComplete.Length ){
			str += strComplete[i++];
			DialogueText.GetComponent<Text> ().text = str;
			yield return new WaitForSeconds(0.03F);
			if (Input.GetKey (KeyCode.E) && i > 4) {
				canSkip = true;
				DialogueText.GetComponent<Text> ().text = strComplete;
				yield break;
			}
		}
		canSkip = true;
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
		yield return new WaitForSeconds (2f);
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
		GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
		GameObject.FindGameObjectWithTag ("Music").GetComponent<AudioSource> ().Play ();
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

	public void ChangeMoves()
	{
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name1, creature.GetComponent<Creature>().damage1, creature.GetComponent<Creature>().heal1, creature.GetComponent<Creature>().details1));
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name2, creature.GetComponent<Creature>().damage2, creature.GetComponent<Creature>().heal2, creature.GetComponent<Creature>().details2));
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name3, creature.GetComponent<Creature>().damage3, creature.GetComponent<Creature>().heal3, creature.GetComponent<Creature>().details3));
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name4, creature.GetComponent<Creature>().damage4, creature.GetComponent<Creature>().heal4, creature.GetComponent<Creature>().details4));
	}
	IEnumerator StartBattle()
	{
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
		if (Moves [0].name == "") {
			BattleFight.transform.GetChild (0).GetComponent<Button> ().interactable = false;
		}
		BattleFight.transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = Moves [0].name;
		if (Moves [1].name == "") {
			BattleFight.transform.GetChild (0).GetComponent<Button> ().interactable = false;
		}
		BattleFight.transform.GetChild (1).transform.GetChild (0).GetComponent<Text> ().text = Moves [1].name;
		if (Moves [2].name == "") {
			BattleFight.transform.GetChild (0).GetComponent<Button> ().interactable = false;
		}
		BattleFight.transform.GetChild (2).transform.GetChild (0).GetComponent<Text> ().text = Moves [2].name;
		if (Moves [3].name == "") {
			BattleFight.transform.GetChild (0).GetComponent<Button> ().interactable = false;
		}
		BattleFight.transform.GetChild (3).transform.GetChild (0).GetComponent<Text> ().text = Moves [3].name;
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
		this.GetComponent <AudioSource> ().Play ();
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



