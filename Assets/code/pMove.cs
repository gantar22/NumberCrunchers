
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMove : MonoBehaviour {

	public enum PS {quick,power,passive} //state

    [SerializeField]
    GameObject spriteHolder;
    [SerializeField]
	float mSpeed = 1;
	[SerializeField]
	float rampFactor = 1;
	[SerializeField]
	float dragFactor;
	[SerializeField]
	float maxSpeed = 1;
	[SerializeField]
	Sprite chargeSprite;
	[SerializeField]
	Sprite kickSprite;
	[SerializeField]
	Sprite slideSprite;
	[SerializeField]
	Sprite standingSprite;
	[SerializeField]
	Sprite oofSprite;
    [SerializeField]
    Sprite dragSprite;
    [SerializeField]
	Sprite[] walkSprites;
    [SerializeField]
    GameObject failTileExplosion;

    [HideInInspector]
	public bool kicking;
	[HideInInspector]
	public float chargedTime;
	[HideInInspector]
	public bool stunned;
	[SerializeField]
	bool turning;
	[SerializeField]
	bool standing;
	[SerializeField]
	bool letgo;
    
    private Vector3 velo;
	private Vector3 tarVelo;
	private int id;
    private string idStr;
	private Vector3 face = Vector3.one + Vector3.down;
	private bool charging;
	private bool sliding;
	private GameController gc;
	private bool hit;
    private int playersHit;

    [SerializeField]
    Transform tileSpawn;
    [SerializeField]
    GameObject dragTilePrefab;
    [SerializeField]
    Sprite dragTileSprite;

    private int dragTileNum = 0;
    private GameObject dragTileGO;

    void Start () {
        id = GetComponent<ObjT>().id;
        idStr = id.ToString();
	}

	void Update () {
		//GetComponent<Animator>().SetBool("walking",false);
		if(gc == null) gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        if (dragTileNum != 0)                                  spriteHolder.GetComponent<SpriteRenderer>().sprite = dragSprite;
        else if (!(kicking || stunned || sliding || charging)) spriteHolder.GetComponent<SpriteRenderer>().sprite = standingSprite;

		if((!kicking && !stunned) || sliding) Move(); else velo = Vector3.zero; 
		
		//print(id + (kicking ? " kicking " : "") + (stunned ? " stunned " : "") + (sliding ? " sliding" : ""));
		if(stunned) print(idStr);

		Timers();

        if (dragTileNum != 0 && Input.GetButton("pd"+idStr)) HandleTileDrop();
        if ((Input.GetKeyDown(keys.b(idStr)) || Input.GetButtonDown("b"+idStr)) && !kicking && !stunned) Kick();
		if ((Input.GetKeyUp  (keys.b(idStr)) || Input.GetButtonUp  ("b"+idStr)) && !stunned) KickRelease(); //this may get called while in standing kick multiple times

        Orient();

        if (kicking){
			for(int i = 0; i < gc.players.Count; i++){
				if(gameObject != gc.players[i] && gc.players[i].GetComponent<BoxCollider>().bounds.Intersects(GetComponent<BoxCollider>().bounds)){
							if(playersHit >> gc.players[i].GetComponent<ObjT>().id == 0)
							{gc.players[i].GetComponent<PMove>().GotKicked(chargedTime);
							playersHit += ((playersHit >> gc.players[i].GetComponent<ObjT>().id) + 1) << gc.players[i].GetComponent<ObjT>().id;
							GetComponent<AudioSource>().volume = .5f + (chargedTime);
							GetComponent<AudioSource>().Play();
							} 
				}
			}
		}
	}

	void Orient(){	
		spriteHolder.GetComponent<SpriteRenderer>().flipX = sliding && (face.x < 0 || (face.x == 0 && face.z > 0));
        spriteHolder.GetComponent<SpriteRenderer>().flipY = sliding && (face.x < 0 || (face.x == 0 && face.z > 0));
	

		if(Mathf.Abs(velo.x) < .1 && !charging) transform.eulerAngles = new Vector3(transform.eulerAngles.x,Mathf.LerpAngle(transform.eulerAngles.y,transform.eulerAngles.y > 90 ? 180 : 0,Time.deltaTime * 5),transform.eulerAngles.z);

		if(Mathf.Abs(velo.x) < .1 && !charging) return;
		if(face.x >  0) transform.eulerAngles = new Vector3(transform.eulerAngles.x,Mathf.LerpAngle(transform.eulerAngles.y,0  ,Time.deltaTime * 5),transform.eulerAngles.z);
		if(face.x <  0) transform.eulerAngles = new Vector3(transform.eulerAngles.x,Mathf.LerpAngle(transform.eulerAngles.y,180,Time.deltaTime * 5),transform.eulerAngles.z);
	}

	void Timers(){
		if(charging) chargedTime = Mathf.Clamp(chargedTime + Time.deltaTime,0,3);
	}

	void Kick(){
		//GetComponent<Animator>().SetBool("walking",true);
		if(Mathf.Pow(Sqr(velo.x)+Sqr(velo.z),.5f) > .1f){
			kicking = true;
			sliding = true;
            spriteHolder.GetComponent<SpriteRenderer>().sprite = slideSprite;
			transform.Rotate(Vector3.forward * 90 * face.x);
		} else {
			charging = true;
            spriteHolder.GetComponent<SpriteRenderer>().sprite = chargeSprite;
		}
	}

	void KickRelease(){
		playersHit = 0;
		if(sliding){
            spriteHolder.GetComponent<SpriteRenderer>().sprite = standingSprite;
			kicking = false;
			chargedTime = 0;
			sliding = false;
			transform.Rotate(Vector3.back * 90 * face.x);
			stunned = true;
			Invoke("Unoof",.1f);
		} else {
            spriteHolder.GetComponent<SpriteRenderer>().sprite = kickSprite;
			charging = false;
			kicking = true;
			if(!hit) Invoke("Unkick",.5f);
			else hit = false;
		}
	}

	void Unkick(){
		CancelInvoke("Unkick");//safety reasons
		if((Random.value > .25f || chargedTime < .3f)) {LandKick(); return;}
		kicking = false;
        spriteHolder.GetComponent<SpriteRenderer>().sprite = oofSprite;
		stunned = true;
		Invoke("Unoof",chargedTime);

		chargedTime = 0;
	}

	void Unoof(){
		stunned = false;
        spriteHolder.GetComponent<SpriteRenderer>().sprite = standingSprite;
	}

	public void LandKick(){
		CancelInvoke("Unkick");
		kicking = false;
        spriteHolder.GetComponent<SpriteRenderer>().sprite = standingSprite;

		chargedTime = 0;
	}

	public void GotKicked(float time){
		if(stunned) return;
		stunned = true;
        spriteHolder.GetComponent<SpriteRenderer>().sprite = oofSprite;
		Invoke("Unoof",time + 1f);

	}

    private void HandleTileDrop()
    {
        if (dragTileGO == null) return;

        Square sqr  = gc.sBoard.TryClaimSquare(dragTileNum, id, dragTileGO.transform);

        if (sqr == null) return;
        else if (sqr.solNum != sqr.fillNum || sqr.ownedBy != id)
        {
            dragTileNum = 0;
            Destroy(dragTileGO);
            GameObject explosion = Instantiate(failTileExplosion, tileSpawn);
            explosion.transform.parent = null;
            explosion.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "playground";
            explosion.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
            Destroy(explosion, 1.0f);
        }
        else
        {
            dragTileNum = 0;
            Destroy(dragTileGO);
            Instantiate(dragTilePrefab, new Vector3(sqr.xMinLim+gc.sBoard.xRes/2, -0.6f, sqr.zMinLim + gc.sBoard.zRes / 2), new Quaternion()).transform.parent = null;
        }
    }



    void Move(){
		//naive
		//transform.position += new Vector3(Input.GetAxis("HorizontalJ") * mSpeed,0f,Input.GetAxis("VerticalJ") * mSpeed);


		Vector3 joy = new Vector3(Input.GetAxis("HorizontalJ" + idStr),
			                    0,Input.GetAxis("VerticalJ"  + idStr));

		if(sliding){
			velo = Vector3.Lerp(velo,Vector3.zero,Time.deltaTime / velo.magnitude);
			transform.position += velo * mSpeed;
			return;
		} 

		face = new Vector3(joy.x > 0 ? 1 : (joy.x == 0 ? 0 : -1),0,joy.z > 0 ? 1 : (joy.z == 0 ? 0 : -1));

		if(charging) return;

		//if(joy.magnitude > .01f) GetComponent<Animator>().SetBool("walking",true);


		if(Mathf.Abs(joy.x) < .9f && Mathf.Abs(joy.z) < .9f){
			transform.position += joy * mSpeed;
			velo = Vector3.zero;
			tarVelo = Vector3.zero;
			return;
		}

        spriteHolder.GetComponent<SpriteRenderer>().sprite = walkSprites[(int)((Time.time * 4) % walkSprites.Length)];
		//print(Time.time.ToString() + " : " + ((Time.time * 4) % walkSprites.Length).ToString() + " > " + ((int)(( 4 * Time.time) % walkSprites.Length)).ToString());

		tarVelo = new Vector3(joy.x,0,joy.z);
		/*if(turning){
			if(velo.x * tarVelo.x < 0){
				velo = new Vector3(0,0,velo.z);
			}
			if(velo.z * tarVelo.z < 0){
				velo = new Vector3(velo.x,0,0);
			} //turning doesn't keep momentum
		}*/
		if(turning){
			if(velo.z * tarVelo.z <= 0 && velo.x * tarVelo.x <= 0){
				velo *= Time.deltaTime/2;
			}
		}

		if(standing) tarVelo += new Vector3(velo.x == 0 && Sqr(joy.x) > .9f ? 500 * tarVelo.x * Time.deltaTime : 0,0, velo.z == 0 && Sqr(joy.z) > .9f ? 500 * tarVelo.z * Time.deltaTime : 0); //slam
		
		float dragH = 1;
		float dragV = 1;

		if(letgo){
			dragH = (joy.x == 0 ? (joy.z == 0 ? Sqr(dragFactor * velo.x) + dragFactor : rampFactor) : rampFactor);
			dragV = (joy.z == 0 ? (joy.x == 0 ? Sqr(dragFactor * velo.z) + dragFactor : rampFactor) : rampFactor);
		}

		float deltaH = Mathf.Lerp(velo.x,tarVelo.x,Time.deltaTime * dragH);
		float deltaV = Mathf.Lerp(velo.z,tarVelo.z,Time.deltaTime * dragV);

		velo = new Vector3(deltaH,0,deltaV);


		float diagonalC = Mathf.Clamp(Mathf.Pow(Sqr(velo.x)+Sqr(velo.z),.5f) - 1,0,2);

		transform.position += velo * (mSpeed / (diagonalC + 1));


		if(velo.x > 0) face.x =  1;
		if(velo.y < 0) face.x = -1;
		if(velo.z > 0) face.z =  1;
		if(velo.z < 0) face.z = -1;

	}

	float Sqr(float x){
		return Mathf.Pow(x,2);
	}

	// Parents newChild to the object to which this script is attached. In this case, the player
	void SetChild(GameObject newChild) {
		newChild.transform.parent = this.transform;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "scissors") {
			SetChild (other.gameObject);
			other.gameObject.tag = "weapon";
		}
    }

    private void OnTriggerStay(Collider other)
    {
        if (dragTileNum == 0 && other.CompareTag("PickupTile") && Input.GetButton("pd"+idStr))
        {
            dragTileNum = other.gameObject.GetComponent<ObjT>().id;
            dragTileGO = Instantiate(dragTilePrefab, tileSpawn);
            dragTileGO.GetComponent<BoxCollider>().enabled = false;
            dragTileGO.GetComponentInChildren<SpriteRenderer>().sprite = dragTileSprite;
            dragTileGO.transform.GetChild(0).transform.localScale = new Vector3(0.15f, 0.14f, 0);
        }
    }
}
