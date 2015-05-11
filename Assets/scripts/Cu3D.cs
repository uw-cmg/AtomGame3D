using UnityEngine;
using System.Collections;

public class Cu3D : Atom3D {
	public override void Awake(){
		base.Awake();
		//data: http://en.wikipedia.org/wiki/Ionic_radius
		SetUp("Cu",2,73f);
	}
}