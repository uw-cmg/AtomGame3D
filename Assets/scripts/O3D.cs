using UnityEngine;
using System.Collections;

public class O3D : Atom3D{
	public override void Awake(){
		base.Awake();
		SetUp("O",-2,140f);
	}
}