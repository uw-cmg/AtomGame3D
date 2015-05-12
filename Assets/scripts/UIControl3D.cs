using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIControl3D : MonoBehaviour {
	public static UIControl3D self;
	public GameObject endGamePanel;
	public GameObject tryAgainBtn;
	public GameObject nextLevelBtn;
	public GameObject atomMenuPanel;
	public Scrollbar temperatureScroll;

	public Text scoreText;
	public Text scoreTextInStat;// score in the end of game stat
	public Text numberOfConnections;//in end of game stat
	public Text numberOfUsedAtomsText;//in end of game stat

	public Text endGameText;
	public Text timerText;
	void Awake(){
		self = this;
		endGamePanel.SetActive(false);
	}
	// Use this for initialization
	void Start () {
		if(Application.loadedLevelName == "AnchorDemo"){
			foreach(AtomUIData atomUIData in AtomStaticData.AtomDataMap.Values){
				atomUIData.btnText.text = atomUIData.atomName + " (" 
					+ atomUIData.remainingStock + ")";
			}
		}
	}
	//for connect monsters
	public void OnGameEnded(){
		//update game stats
		
		scoreTextInStat.text = GameControl3D.self.score.ToString("0.0");
		
		numberOfConnections.text 
			= MonsterAtomManager.self.totalConnections + "/" 
			+ MonsterAtomManager.self.atomConnections.Count; 
			
		numberOfUsedAtomsText.text = GameControl3D.self.totalAtomsUsed.ToString("0");
		
		endGamePanel.SetActive(true);
	}
	
	public void OnHoverConnectionEntry(MonsterAtomConnection mac){
		mac.ShowPath();
	}
	public void OnLeaveConnectionEntry(MonsterAtomConnection mac){
		mac.HidePath();
	}
	
	public void UpdateTimer(float timeRemaining){
		timerText.text = Mathf.Max(0.0f, timeRemaining).ToString("0.0");
	}

	public void OnAddAtom(GameObject prefab){
		//fade that atom button
		GameControl3D.self.CreateAtom(prefab);

	}
	public void EnableAtomBtns(bool enable = true){
		if(Application.loadedLevelName != "AnchorDemo"){
			
			foreach(AtomUIData atomUIData in AtomStaticData.AtomDataMap.Values){
				atomUIData.btn.interactable = enable;
			}
			
		}else{
			if(GameControl3D.self.gameState == (int)GameControl3D.GameState.AddingAtom){
				
				foreach(AtomUIData atomUIData in AtomStaticData.AtomDataMap.Values){
					atomUIData.btn.interactable = false;
				}
				return;
			}
			foreach(AtomUIData atomUIData in AtomStaticData.AtomDataMap.Values){
				atomUIData.btn.interactable = (atomUIData.remainingStock > 0);
			}
		}
		
	}
	public void OnScrollbarTemperatureChanged(){
		AtomPhysics3D.self.temperature = temperatureScroll.value * 2000f;
	}
	public void UpdateScore(){
		scoreText.text = GameControl3D.self.score.ToString("0.0");
	}
	public void UpdateAtomBtnWithStock(Atom3D newlyAddedAtom){
		if(Application.loadedLevelName == "AnchorDemo"){
			AtomUIData atomUIData = AtomStaticData.AtomDataMap[newlyAddedAtom.name];
			atomUIData.btnText.text = newlyAddedAtom.name + " (" + 
				atomUIData.remainingStock + ")";
			if(atomUIData.remainingStock <= 0){
				atomUIData.btn.interactable = false;
			}

		}
	}
	public void OnClickAtomBtn(){
		EnableAtomBtns(false);
	}
	//for gooey
	public void EndGame(bool win){
		if(win){
			endGameText.text = "You Won nyo >w<!";
			tryAgainBtn.SetActive(false);
			nextLevelBtn.SetActive(true);
		}else{
			endGameText.text = "You Lost nio QvQ!";
			tryAgainBtn.SetActive(true);
			nextLevelBtn.SetActive(false);
		}
		
		endGamePanel.SetActive(true);
	}
	public void OnClickQuit(){
		Application.Quit();
	}
	public void OnClickTryAgain(){
		Application.LoadLevel("main");
	}

}
