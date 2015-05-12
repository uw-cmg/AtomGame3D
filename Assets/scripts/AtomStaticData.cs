using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtomStaticData : MonoBehaviour {
	public static Dictionary<string, AtomUIData> AtomDataMap;
	public string[] atomNames; 
	public int[] startStocks;
	public AtomUIData uiDataPrefab;

	public static int totalRemainingStock;
	public static void DecrementStock(string name){
		AtomDataMap[name].remainingStock -= 1;
		totalRemainingStock -= 1;
	}
	void Start(){
		AtomDataMap = new Dictionary<string, AtomUIData>();
		if(Application.loadedLevelName == "AnchorDemo"){
			atomNames = new string[]{"Cl","Cu","Na"}; 
			startStocks = new int[]{50,30,30};
			if(atomNames.Length != startStocks.Length){
				Debug.Log("atomNames and startStocks should have same lengths!");
				Application.Quit();
			}
			for(int i=0; i < atomNames.Length;i++){
				string atomName = atomNames[i];
				if(!AtomDataMap.ContainsKey(atomName)){
					AtomUIData atomUIData = Instantiate(uiDataPrefab) as AtomUIData;
					atomUIData.CreateSelf(atomName, startStocks[i]);
					totalRemainingStock += startStocks[i];
					AtomDataMap.Add(atomName, atomUIData);
				}else{
					Debug.Log(atomName + " already exists!");
				}
				
			}
		}else if(Application.loadedLevelName == "main"){
			atomNames = new string[]{"Cl","Cu","Na"}; 
			totalRemainingStock = int.MaxValue;
			for(int i=0; i < atomNames.Length;i++){
				string atomName = atomNames[i];
				if(!AtomDataMap.ContainsKey(atomName)){
					AtomUIData atomUIData = Instantiate(uiDataPrefab) as AtomUIData;
					atomUIData.CreateSelf(atomName, int.MaxValue);
					
					AtomDataMap.Add(atomName, atomUIData);
				}else{
					Debug.Log(atomName + " already exists!");
				}
				
			}
		}
	}
}
