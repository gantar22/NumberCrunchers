using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjT : MonoBehaviour {
	public int id;
	public enum obj {pc,pwr,num,bgr}
	[SerializeField]
	public obj typ;
}
