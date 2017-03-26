using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

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

	public GameObject Battle;
	public GameObject BattleMenu;
	public GameObject BattleFight;

	public GameObject demonAttackCamera;
	public GameObject camera;
	private Animator daAnimator;
	public GameObject demons;
	public GameObject Ariel2;

	public List<string> Bag;
	public bool isBag;

	public bool inBattle;

	public string LastKeyPress;
	public string direction;
	Vector2 dir;

	private string str;

	public bool canSkip = true;



	//InBattle

	List<BattleMove> Moves = new List<BattleMove>();

	void Start () {
		modelAnimator = model.GetComponent<Animator> ();
		//StartCoroutine ("StartBattle");
		Flash.SetActive(true);
		daAnimator = demonAttackCamera.GetComponent<Animator> ();
	}
		
	void Update()
	{
		if (inBattle) {
			if (Input.GetKeyDown (KeyCode.Q)) {
				BattleMenu.SetActive (true);
				BattleFight.SetActive (false);
				this.GetComponent <AudioSource> ().Play ();
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
				if (direction == "W") {
					dir = Vector2.up;
				}
				if (direction == "A") {
					dir = Vector2.left;
				}
				if (direction == "S") {
					dir = Vector2.down;
				}
				if (direction == "D") {
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
						if (direction == "W") {
							dir = Vector2.up;
						}
						if (direction == "A") {
							dir = Vector2.left;
						}
						if (direction == "S") {
							dir = -Vector2.up;
						}
						if (direction == "D") {
							dir = -Vector2.left;
						}
						RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, 1.8f);
						if (hit.collider != null) {
							if (hit.collider.gameObject.GetComponent<InteractObject> () || hit.collider.gameObject.GetComponent<Ariel> ()) {
								hit.collider.gameObject.SendMessage ("Interact");
							}
						}
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

	public void Dialogue(string d)
	{
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
			if (d == "*Ariel mutturs random words*") {
				StartCoroutine ("FlashCamera");
			}
			DialogueBox.SetActive (true);
			canMove = false;
			dialogue = true;
			StartCoroutine (AnimateText (d));
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
			yield return new WaitForSeconds(0.05F);
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

	IEnumerator StartBattle()
	{
		canMove = false;
		GameObject.FindGameObjectWithTag ("Music").GetComponent<Music> ().ChangeMusic ();
		BattleUI.GetComponent<Animation> ().Play ("BattleUI_Enter");
		yield return new WaitForSeconds (1);
		inBattle = true;
		BattleUI.GetComponent<RectTransform> ().localScale.Set (1, 1, 1);
		Battle.SetActive (true);
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name1, creature.GetComponent<Creature>().damage1, creature.GetComponent<Creature>().heal1, creature.GetComponent<Creature>().details1));
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name2, creature.GetComponent<Creature>().damage2, creature.GetComponent<Creature>().heal2, creature.GetComponent<Creature>().details2));
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name3, creature.GetComponent<Creature>().damage3, creature.GetComponent<Creature>().heal3, creature.GetComponent<Creature>().details3));
		Moves.Add( new BattleMove(creature.GetComponent<Creature>().name4, creature.GetComponent<Creature>().damage4, creature.GetComponent<Creature>().heal4, creature.GetComponent<Creature>().details4));
		BattleFight.transform.GetChild (0).transform.GetChild (0).GetComponent<Text> ().text = Moves [0].name;
		BattleFight.transform.GetChild (1).transform.GetChild (0).GetComponent<Text> ().text = Moves [1].name;
		BattleFight.transform.GetChild (2).transform.GetChild (0).GetComponent<Text> ().text = Moves [2].name;
		BattleFight.transform.GetChild (3).transform.GetChild (0).GetComponent<Text> ().text = Moves [3].name;
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
		
	}
	public void Move2()
	{

	}
	public void Move3()
	{

	}
	public void Move4()
	{

	}
}



