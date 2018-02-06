using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pMove : MonoBehaviour {

	[SerializeField]
	float mSpeed = 1;
	[SerializeField]
	float rampFactor = 1;
	[SerializeField]
	float dragFactor;
	[SerializeField]
	float maxSpeed = 1;

	private Vector3 velo;
	private Vector3 tarVelo;
	private string id;


	// Use this for initialization
	void Start () {
		id = GetComponent<objT>().id.ToString();
		
	}


	
	// Update is called once per frame
	void Update () {
		move();
		if(Input.GetButton("Joystick"+id+"Button1") && kick()) kickFX();
	}

	bool kick(){ //return true if you hit
		bool r = false;
	
		return r;
	}

	void kickFX(){

	}

	void move(){
		//naive
		//transform.position += new Vector3(Input.GetAxis("HorizontalJ") * mSpeed,0f,Input.GetAxis("VerticalJ") * mSpeed);

		Vector3 joy = new Vector3(Input.GetAxis("HorizontalJ" + id),
			                    0,Input.GetAxis("VerticalJ"  + id));

		tarVelo = new Vector3(joy.x * mSpeed,0,joy.z * mSpeed);
		
		if(velo.x * tarVelo.x < 0){
			velo = new Vector3(0,0,velo.z);
		}
		if(velo.z * tarVelo.z < 0){
			velo = new Vector3(velo.x,0,0);
		}

		tarVelo += new Vector3(velo.x == 0 ? 1000 * tarVelo.x * Time.deltaTime : 0,0, velo.z == 0 ? 1000 * tarVelo.z * Time.deltaTime : 0); //slam
		
		//velo = Vector3.Lerp(velo,tarVelo,Time.deltaTime * rampFactor); no drag
		float dragH = (joy.x == 0 ? (joy.z == 0 ? sqr(dragFactor * velo.x) + dragFactor : 4 * rampFactor) : rampFactor);
		float dragV = (joy.z == 0 ? (joy.x == 0 ? sqr(dragFactor * velo.z) + dragFactor : 4 * rampFactor) : rampFactor);
		


		float deltaH = Mathf.Lerp(velo.x,tarVelo.x,Time.deltaTime * dragH);
		float deltaV = Mathf.Lerp(velo.z,tarVelo.z,Time.deltaTime * dragV);

		velo = new Vector3(deltaH,0,deltaV);

		transform.position += velo;

	}

	float sqr(float x){
		return Mathf.Pow(x,2);
	}



}
