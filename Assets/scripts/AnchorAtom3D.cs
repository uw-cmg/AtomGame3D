using UnityEngine;
using System.Collections;

//[RequireComponent (typeof (Atom2D))]

public class AnchorAtom3D : MonoBehaviour {
	public float lastClickTime;
	public bool clickIsForAddingAtom;
	public Atom3D atomComp;
	public MonsterAtom3D monsterAtomComp;

	// Use this for initialization
	void Start () {
		clickIsForAddingAtom = true;
		lastClickTime = -1.0f;
		atomComp = GetComponent<Atom3D>();
		monsterAtomComp = gameObject.AddComponent<MonsterAtom3D>() as MonsterAtom3D;
		if(atomComp.charge > 0){
			monsterAtomComp.charge = 6;
		}else{
			monsterAtomComp.charge = -6;
		}
		monsterAtomComp.InitColorsByCharge();
		monsterAtomComp.enabled = false;
		atomComp.enabled = true;

	}
	
	
	void OnMouseDown(){
		if(clickIsForAddingAtom){
			clickIsForAddingAtom = false;
			lastClickTime = -1.0f;
		}else if(lastClickTime <= 0){
			lastClickTime = Time.time;
		}else{ 
			if(Time.time - lastClickTime < 0.3f){
				//Debug.Log("double click");
				ToggleAnchor();
				lastClickTime = -1.0f;
			}else{
				lastClickTime = Time.time;
			}
			
		}
		
	}
	void ToggleAnchor(){
		if(gameObject.tag == "Atom"){
			gameObject.tag = "MonsterAnchor";
			//AtomPhysics2D.self.Ions.Remove(gameObject);
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().isKinematic = true;
			
			monsterAtomComp.enabled = true;
			atomComp.enabled = false;
			GetComponent<MeshRenderer>().material.color = monsterAtomComp.normalColor;

		}else if(gameObject.tag == "MonsterAnchor"){
			gameObject.tag = "Atom";
			//AtomPhysics2D.self.Ions.Add(gameObject);
			GetComponent<Rigidbody>().isKinematic = false;
			
			monsterAtomComp.enabled = false;
			atomComp.enabled = true;

			GetComponent<MeshRenderer>().material.color = atomComp.normalColor;
		}	
	}
}

