using UnityEngine;
using System.Collections;

public class GameControl3D : MonoBehaviour {
	public static GameControl3D self;
	public GameObject atomToAdd;
	public float timeAllowed;
	public float timeRemaining;
	public int gameState;
	public float score;
	public int totalAtomsUsed;

	public enum GameState{
		Running,
		AddingAtom,
		Ended
	};
	void Awake(){
		self = this;
		if(Application.loadedLevelName == "main"){
			timeAllowed = 60.0f;
		}else if(Application.loadedLevelName == "AnchorDemo"){
			timeAllowed = 180.0f;//3 minutes
		}else{
			timeAllowed = 900.0f;//5 minutes
		}
		
	}
	// Use this for initialization
	void Start () {
		score = 0.0f;
		totalAtomsUsed = 0;
		timeRemaining = timeAllowed;
		gameState = (int)GameState.Running;
	}
	
	void Update(){
		timeRemaining -= Time.deltaTime;
		UIControl3D.self.UpdateTimer(timeRemaining);
		if(timeRemaining <= 0){
			gameState = (int)GameState.Ended;
		}

		if(gameState == (int)GameState.Running){
			
		}else if(gameState == (int)GameState.AddingAtom){
			if(Input.GetButtonDown("Cancel")){//escape
				Destroy(atomToAdd);
				gameState = (int)GameState.Running;
				UIControl3D.self.EnableAtomBtns();
				return;
				
			}
			UpdateAtomPositionWithMouse();

			atomToAdd.GetComponent<Rigidbody>().velocity = Vector3.zero;
			if(!Application.isMobilePlatform){
				if(Input.GetMouseButtonDown(0)){
					FinishAddingAtom();
				}
			}
		//this state is only for connect monster atoms	
		}else if (gameState == (int)GameState.Ended){
			
			if(Time.timeScale == 0){
				return;
			}
			if(Application.loadedLevelName == "main"){
				UIControl3D.self.EndGame(false);
			}else if(Application.loadedLevelName == "AnchorDemo"){
				OnGameEnded();
				UIControl3D.self.OnGameEnded();
			}
			
			Time.timeScale = 0;
			
		}
		
		
		
	}
	public void UpdateScoreBy(float offset){
		score += offset;
		UIControl3D.self.UpdateScore();
	}
	//score calculation, etc
	void OnGameEnded(){
		//check total number of atoms used
		MonsterAtomManager.self.EndAllCoroutines();
		score += 100.0f;
		score -= 5.0f * totalAtomsUsed;
		UIControl3D.self.UpdateScore();
		
	}
	void UpdateAtomPositionWithMouse(){
		//new atom position this frame
		Vector2 mousePosInViewport =  Camera.main.ScreenToViewportPoint(Input.mousePosition);
		if(!Application.isMobilePlatform){
			mousePosInViewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		}else{
			/*
			if(Input.touchCount == 1){
				mousePosInViewport = Camera.main.ScreenToViewportPoint(Input.GetTouch(0));
			}else{
				return;
			}
			*/
			
		}
			
		float viewportX = Mathf.Clamp(mousePosInViewport.x, 0.0f,1.0f);
		float viewportY = Mathf.Clamp(mousePosInViewport.y, 0.0f,1.0f);
		float deltaDepth = Input.GetAxis("Mouse ScrollWheel") * 5f; // (+-20)
		float depth = Mathf.Clamp(atomToAdd.transform.position.z + deltaDepth, -10f,10f);
		//Debug.Log(depth);

		Vector3 atomPos = Camera.main.ViewportToWorldPoint(
			new Vector3(viewportX, viewportY, -Camera.main.transform.position.z + depth)
			);
		//check if hits another atom
		Collider[] touchingOtherAtoms = Physics.OverlapSphere(
			atomPos, 
			atomToAdd.GetComponent<SphereCollider>().radius * atomToAdd.transform.localScale.x
		);
		if(touchingOtherAtoms.Length > 1){
			return;
		}else if(touchingOtherAtoms.Length > 0){
			if(touchingOtherAtoms[0] != atomToAdd.GetComponent<SphereCollider>())
				return;
		}
		atomToAdd.transform.position = atomPos;
	}
	//register atom and stuff
	void FinishAddingAtom(){
		atomToAdd.name = "Atom" + atomToAdd.GetInstanceID().ToString();
		//add to NaCl list
		Atom3D atom = atomToAdd.GetComponent<Atom3D>();
		atom.Kick();
		//for diff prototypes
		if(AtomPhysics3D.self != null){
			AtomPhysics3D.self.Ions.Add(atomToAdd);
		}

		if(Application.loadedLevelName == "AnchorDemo"){
			//Atom2D.remainingStock -= 1;
			AtomStaticData.DecrementStock(atom.name);
			
			//atom.GetComponent<AudioSource>().Play();
		}
		if(Application.loadedLevelName == "AnchorDemo"){
			if(AtomStaticData.totalRemainingStock <= 0){
				//end of game
				gameState = (int)GameState.Ended;
			}else{
				gameState = (int)GameState.Running;
			}
		}else{
			gameState = (int)GameState.Running;
		}
		totalAtomsUsed += 1;


		UIControl3D.self.EnableAtomBtns();
		UIControl3D.self.UpdateAtomBtnWithStock(atom);
		
	}
	public void CreateAtom(GameObject prefab){
		//Debug.Log("creating atom");
		Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		spawnPos.z = 0;
		Quaternion curRotation = Quaternion.Euler(0, 0, 0);
		GameObject atom = Instantiate(prefab, spawnPos, curRotation) as GameObject;

		atom.GetComponent<Rigidbody>().velocity = Vector2.zero;
		atom.GetComponent<Rigidbody>().isKinematic = false;
		SetGameStateAddingAtom(atom);
		UIControl3D.self.EnableAtomBtns(false);

	}
	public void SetGameStateAddingAtom(GameObject atom){
		atomToAdd = atom;
		gameState = (int)GameState.AddingAtom;
	}
}
