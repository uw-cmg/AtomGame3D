using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MonsterAtomConnection : MonoBehaviour {
	public MonsterAtom3D start;
	public MonsterAtom3D end;
	public List<Atom3D> path;
	public Toggle taskToggle; //assigned in inspector

	public bool HasPath(){
		return path.Count > 0;
	}
	public void ShowPath(){
		Debug.Log("showing path");
		start.GetComponent<MeshRenderer>().material.color = start.highlightColor;
		end.GetComponent<MeshRenderer>().material.color = end.highlightColor;
		foreach(Atom3D atomNode in path){

			atomNode.GetComponent<MeshRenderer>().material.color = atomNode.highlightColor;
			
			
		}
	}
	public void HidePath(){
		
		start.GetComponent<MeshRenderer>().material.color = start.normalColor;
		end.GetComponent<MeshRenderer>().material.color = end.normalColor;
		foreach(Atom3D atomNode in path){
			if(atomNode.GetComponent<MonsterAtom3D>() != null){
				if(atomNode.GetComponent<MonsterAtom3D>().enabled){
					atomNode.GetComponent<MeshRenderer>().material.color 
						= atomNode.GetComponent<MonsterAtom3D>().normalColor;
				}
			}else{
				atomNode.GetComponent<MeshRenderer>().material.color = atomNode.normalColor;
			}
			
		}
		
	}
	public void ClearPath(){
		while(path.Count > 0){
			Atom3D node = path[0];
			if(node != null){
				//node.pathHighlighter.SetActive(false);
				//node.GetComponent<MeshRenderer>().material.color = node.normalColor;
			}
			path.RemoveAt(0);
		}
	}
	// Use this for initialization
	void Start () {
		path = new List<Atom3D>();
	}
	// Update is called once per frame
	void Update () {
	
	}
}

