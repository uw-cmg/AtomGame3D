using UnityEngine;
using System.Collections;

public class Na3D : Atom3D{
	public override void Awake(){
		base.Awake();
		SetUp("Na", 1, 102f);
	}
}