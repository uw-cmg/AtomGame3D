using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtomPhysics3D : MonoBehaviour {
	public static AtomPhysics3D self;
	public List<GameObject> Ions;
	public GameObject[] monsterAnchorAtoms;
	public float temperature;
	void Awake(){
		self = this;
		Application.targetFrameRate = 150;
		GameObject[] loadedAtoms = GameObject.FindGameObjectsWithTag("Atom");
		monsterAnchorAtoms = GameObject.FindGameObjectsWithTag("MonsterAnchor");
		foreach(GameObject g in loadedAtoms){
			Ions.Add(g);
		}
		foreach(GameObject g in monsterAnchorAtoms){
			Ions.Add(g);
		}
	}
	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
	}
	public Vector3 RandDirection(Vector3 combinedForce){
		Vector3 combinedForceDir = combinedForce;
		combinedForceDir.Normalize();
		float randDegree = Random.Range(-60f, 60f);

		Vector3 dir = Quaternion.Euler(0,0,randDegree) * combinedForceDir;
		dir.Normalize();
		return dir;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		for(int i=0; i < Ions.Count;i++){
			if(Ions[i] == null){
				Ions.Remove(Ions[i]);
				continue;
			}
			Atom3D atom = Ions[i].GetComponent<Atom3D>();
			atom.totalForce = Vector3.zero;
			atom.Kick(); 
			//check if atoms out of viewport, if so, destroy
			/*
			bool withinViewport = atom.WithinViewport();
			if(!withinViewport){
				//Destroy(atom.gameObject);
				Ions.Remove(atom.gameObject);
				
				Destroy(atom.gameObject);
			}
			*/
		}
		Atom3D curr;
		Atom3D other;
		for(int i=0; i < Ions.Count;i++){
			Rigidbody rb = Ions[i].GetComponent<Rigidbody>();
			if(rb.velocity.magnitude < 1f){
				rb.gameObject.GetComponent<Atom3D>().Kick();
			}
		}
		for(int i=0; i < Ions.Count;i++){
			curr = Ions[i].GetComponent<Atom3D>();
			Rigidbody currRb = Ions[i].GetComponent<Rigidbody>();

			for(int j=i+1; j < Ions.Count;j++){
				other = Ions[j].GetComponent<Atom3D>();

				float distance = Vector3.Distance(curr.gameObject.transform.position, 
				other.gameObject.transform.position);
				//repel
				//current to other
				Vector3 forceDireciton = curr.gameObject.transform.position - other.gameObject.transform.position;
				//attract
				if(curr.charge * other.charge < 0){
					forceDireciton *= -1;
				}
				float otherToCurr = 9 * Mathf.Pow(10, 9) * 1.602f *1.602f 
					* Mathf.Abs(other.charge) * Mathf.Abs(curr.charge) * Mathf.Pow(10,-8);
				float currToOther = otherToCurr;
				//Vector3 force = (currRb.mass * currRb.velocity - otherRb.mass * otherRb.velocity)/Time.deltaTime;
				forceDireciton.Normalize();
				//ft = m * deltaV
				//deltaV = sqrt(2C/m)
				//ft = sqrt(2cm)

				curr.totalForce += forceDireciton * otherToCurr / distance / distance;

				//Debug.Log(otherRb.velocity);
				other.totalForce += -forceDireciton * currToOther / distance / distance; 
			}
			/*
			if(temperature > 0.0f ){
				if(!curr.pathHighlighter.activeSelf){
					//curr atom is not in a path
					curr.lastRandWalkForce = RandDirection(curr.totalForce) 
						* Mathf.Sqrt(2f*(temperature/1000f) *currRb.mass)/Time.fixedDeltaTime;
					curr.totalForce += curr.lastRandWalkForce;
				}
				
				
			}
			*/
			currRb.velocity = Vector3.zero;
			if(currRb.gameObject.tag == "AnchorAtom" || currRb.gameObject.tag == "MonsterAnchor"){
				
			}else{

				currRb.AddForce(curr.totalForce);
			}
				
		}
	}
}
