using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterAtomManager : MonoBehaviour {
	public static MonsterAtomManager self;
	public List<MonsterAtom3D> monsterAnchors;
	public List<MonsterAtomConnection> atomConnections;
	private Stack<Atom3D> atomPath;
	public int totalConnections;
	void Awake(){
		self = this;
		totalConnections = 0;
		atomConnections = new List<MonsterAtomConnection>();
		monsterAnchors = new List<MonsterAtom3D>();
		atomPath = new Stack<Atom3D>();
		Debug.Log("atom path init");
		GameObject[] monsterAnchorArr = GameObject.FindGameObjectsWithTag("MonsterAnchor");
		foreach(GameObject g in monsterAnchorArr){
			monsterAnchors.Add(g.GetComponent<MonsterAtom3D>());
		}
		GameObject[] connectionObjs = GameObject.FindGameObjectsWithTag("MonsterAtomConnection");
		foreach(GameObject g in connectionObjs){
			atomConnections.Add(g.GetComponent<MonsterAtomConnection>());
		}
	}
	IEnumerator CheckMonsterAtomConnections(){
		while(true){
			yield return new WaitForSeconds(0.04f);
			totalConnections = 0;
			foreach(MonsterAtomConnection mac in atomConnections){
				Atom3D maStart = mac.start;
				Atom3D maEnd = mac.end;
				//clear visited flags
				foreach(MonsterAtom3D ma3D in monsterAnchors){
					ma3D.visitState = (int)Atom3D.DFSState.unvisited;
				}
				foreach(GameObject atom in AtomPhysics3D.self.Ions){
					atom.GetComponent<Atom3D>().visitState = (int)Atom3D.DFSState.unvisited;
				}
				//DFS
				bool areConnected = BacktrackMonstersMonsters(ref maStart, ref maEnd);
				if(areConnected){
					/*
					Debug.DrawLine(maStart.transform.position, maEnd.transform.position,
						Color.white, 2.0f);

					Debug.Log(maStart.gameObject.name + " and " + maEnd.gameObject.name + " are connected!");
					*/
					if(!mac.HasPath()){
						//from not having path to having path, plus score
						GameControl3D.self.UpdateScoreBy(100.0f);
					}
					
					//clear old path rendering
					mac.ClearPath();
					bool pathBroken = false;
					//if has path, copy path from atom path to mac.path
					while(atomPath.Count > 0 && !pathBroken){
						Atom3D atomNode = atomPath.Pop();
						if(atomNode == null){
							//this happens when an atom moves out of viewport
							//and has been destroyed
							//which means the path is now broken
							pathBroken = true;
						}else{
							mac.path.Add(atomNode);
							atomNode.GetComponent<MeshRenderer>().material.color = Color.white;
							//atomNode.pathHighlighter.SetActive(true);
						}
						
					}
					if(pathBroken){
						mac.ClearPath();
						//mac.taskToggle.isOn = false;
					}else{
						//mac.taskToggle.isOn = true;
						totalConnections += 1;
					}
					
				}else{
					//if had path last frame but no longer has it
					if(mac.HasPath()){
						//from having path to not having path
						//minus score
						GameControl3D.self.UpdateScoreBy(-100.0f);
					}
					//no path, clear last path,restore normal atom colors
					mac.ClearPath();
					//mac.taskToggle.isOn = false;
					totalConnections -= 1;
				}
			}
			if(totalConnections >= atomConnections.Count){
				GameControl3D.self.gameState = (int)GameControl3D.GameState.Ended;
			}
		}
		
	}
	//dfs with backtracking wrapper
	bool BacktrackMonstersMonsters(ref Atom3D maStart, ref Atom3D maEnd){
		atomPath.Clear();
		atomPath.Push(maStart);
		return MonstersAreConnected(ref maStart, ref maEnd);
	}
	//Awwww sweet DFS with backtracking, for real
	bool MonstersAreConnected(ref Atom3D maStart, ref Atom3D maEnd){
		if(maStart == maEnd){
			return true;
		}
		maStart.visitState = (int)Atom3D.DFSState.visiting;

		foreach(Atom3D neighbour in maStart.neighbours){
			Atom3D neighbourRef = neighbour;

			if(neighbourRef.visitState == (int)Atom3D.DFSState.unvisited){
				atomPath.Push(neighbourRef);
				if(MonstersAreConnected(ref neighbourRef, ref maEnd)){
					return true;
				}
			}
		}
		maStart.visitState = (int)Atom3D.DFSState.visited;
		Atom3D curr;
		while(atomPath.Count > 0){
			curr = atomPath.Pop();
			if(curr == maStart){
				break;
			}
		}
		
		return false;

	}
	public void EndAllCoroutines(){
		StopCoroutine(CheckMonsterAtomConnections());
	}
	// Use this for initialization
	void Start () {
		StartCoroutine(CheckMonsterAtomConnections());
	}
	
}
