using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Atom3D : MonoBehaviour {
	public static Atom3D self;
	public string name;
	public int charge;
	public Vector3 totalForce = Vector3.zero;
	[HideInInspector]public SphereCollider cc;
	public int visitState;
	public enum DFSState{
		visited,
		unvisited,
		visiting
	};
	public List<Atom3D> neighbours;
	//for connect monsters game
	public Color normalColor;
	public Color pathColor;//the ring around the atom
	public Color highlightColor;//overall highlight color
	public float radius;
	public Vector3 lastRandWalkForce;
	//public GameObject pathHighlighter;
	// Use this for initialization
	public virtual void Awake(){
		self = this;
		visitState = (int)DFSState.unvisited;
		if(Application.loadedLevelName == "AnchorDemo"){
			neighbours = new List<Atom3D>();
		}

		//by default, path color is white
		pathColor = Color.white;
	}
	protected void SetUp(string name, int charge, float radius){
		this.name = name;
		this.charge = charge;
		this.radius = radius;
		cc = GetComponent<SphereCollider>();
		if(Application.loadedLevelName == "AnchorDemo"){
			GetComponent<MeshRenderer>().material.color = normalColor;
			//pathHighlighter = transform.Find("PathHighlighter").gameObject;
			float scaledRadius = radius / 1000 * 6;
			transform.localScale = new Vector3(scaledRadius, scaledRadius, scaledRadius);
		}
	}
	void Start () {
		if(Application.loadedLevelName == "AnchorDemo"){
			//pathHighlighter = transform.Find("PathHighlighter").gameObject;
		}
	}
	
	void OnCollisionEnter(Collision collision){
		if(Application.loadedLevelName != "AnchorDemo")return;
		if(collision.gameObject.tag != "Atom"
			&& collision.gameObject.tag != "MonsterAnchor")return;
		//Debug.Log("collision enter: " + gameObject.name);
		//Debug.Log(Time.time);
		if(!neighbours.Contains(collision.gameObject.GetComponent<Atom3D>()))
			neighbours.Add(collision.gameObject.GetComponent<Atom3D>());
	}
	void OnCollisionExit(Collision collision){
		if(Application.loadedLevelName != "AnchorDemo")return;
		if(collision.gameObject.tag != "Atom"
			&& collision.gameObject.tag != "MonsterAnchor")return;
		neighbours.Remove(collision.gameObject.GetComponent<Atom3D>());
	}
	//gives a random vel
	public void Kick(){
		float lo = -15.50f;
		float hi = 15.50f;
		float x = UnityEngine.Random.Range(lo, hi);
		float y = UnityEngine.Random.Range(lo, hi);
		float z = UnityEngine.Random.Range(lo, hi);
		GetComponent<Rigidbody>().velocity = new Vector3(x,y,z);
	}
	//used for destroying atoms beyond viewport
	//if it's the target atom, lose game
	/*
	public bool WithinViewport(){
		if(cc == null){
			//Debug.Log("cc is null");
			return true;
		}
		Vector2 rightMostPoint = transform.position + new Vector3(-cc.radius,0, 0);
		Vector2 leftMostPoint = transform.position + new Vector3(cc.radius,0, 0);
		Vector2 topPoint = transform.position + new Vector3(0, -cc.radius, 0);
		Vector2 bottomPoint = transform.position + new Vector3(0, cc.radius, 0);
	
		//right bound
		Vector2 viewportRightPoint = Camera.main.WorldToViewportPoint(leftMostPoint);
		//left bound
		Vector2 viewportLeftPoint = Camera.main.WorldToViewportPoint(rightMostPoint);
		//top bound
		Vector2 viewportTopPoint = Camera.main.WorldToViewportPoint(bottomPoint);
		//bottom boud
		Vector2 viewportBottomPoint = Camera.main.WorldToViewportPoint(topPoint);

		if(viewportRightPoint.x < -0.1f || viewportRightPoint.x > 1.1f){
//			Debug.Log(viewportRightPoint);
			return false;
		}
		if(viewportLeftPoint.x < -0.1f || viewportLeftPoint.x > 1.1f){
	//		Debug.Log(viewportLeftPoint);
			return false;
		}
		if(viewportTopPoint.y < -0.1f || viewportTopPoint.y > 1.1f){
			return false;
		}
		if(viewportBottomPoint.y < -0.1f || viewportBottomPoint.y > 1.1f){
			return false;
		}
		return true;
	}
	*/
}
