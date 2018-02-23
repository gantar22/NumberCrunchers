using System.Collections;
using UnityEngine;


public class PMove : MonoBehaviour {

	public enum PS {quick,power,passive} //state
	public enum TS {standing,walking,running} //travel state

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
    
    [SerializeField]
    [Range(0f,1f)]
    float timeToDash;

    [HideInInspector]
	public bool kicking;
	[HideInInspector]
	public float chargedTime;
	[HideInInspector]
	public bool stunned;
    public Sprite winSprite;
    [HideInInspector]
    public bool dodging;
    [HideInInspector]
    public bool highlighting;
    [HideInInspector]
    public int score;
    public AudioClip kick;
    public AudioClip pickup;
    public AudioClip[] wrong;
    public AudioClip correct;
    public AudioClip wooHoo;
    public AudioClip highlightSuccess;


    [SerializeField]
	bool turning;
	[SerializeField]
	bool standing;
	[SerializeField]
	bool letgo;
	[SerializeField]
	bool walking;
    
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
    private TS travel;
    private Vector3 runningFace;
    private int oldX;
    private float timeSinceStanding;
    private int powerUpCount = 0;
    private bool highlightHeld;
    private bool invincible;

    private GameObject gToOwn;
    private Square sqrToOwn;


    [SerializeField]
    Transform tileSpawn;
    [SerializeField]
    GameObject dragTilePrefab;
    [SerializeField]
    Sprite dragTileSprite;
    [SerializeField]
    Sprite backSprite;
    [SerializeField]
    Sprite highlightsprite1;
    [SerializeField]
    Sprite highlightsprite2;
    [SerializeField]
    GameObject menu;

    private int dragTileNum = 0;
    private GameObject dragTileGO;


    void Awake(){
    	StartCoroutine(PopUp());
    }

    IEnumerator PopUp(){
    	float tp = 0;
    	float dur = .3f;
    	while(tp < dur){
    		tp += Time.deltaTime;
    		transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1.3f,tp / dur);

    		yield return null;
    	}
    	tp = 0;
    	dur = .1f;
    	while(tp < dur){
    		tp += Time.deltaTime;
    		transform.localScale = Vector3.Lerp(Vector3.one * 1.3f, Vector3.one,tp / dur);
    		yield return null;
    	}

    	transform.localScale = Vector3.one;
    
    }


    void Start () {
        id = GetComponent<ObjT>().id;
        idStr = id.ToString();
    }

    void Update () {

		if(Input.GetKeyDown(keys.start(idStr))) {gc.gameObject.SetActive(false); menu.SetActive(true);}


		//GetComponent<Animator>().SetBool("walking",false);
		if(gc == null) gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();

		transform.position = new Vector3(Mathf.Clamp(transform.position.x,-4.5f,4.5f),0,Mathf.Clamp(transform.position.z,-4,4));

		if(highlighting && (Input.GetKeyDown(keys.a(idStr)) || Input.GetButtonUp("h" + idStr))) HandleHighLight();
		if(!highlighting || Input.GetKeyUp(keys.a(idStr))) highlightHeld = false;

		
	    if (dragTileNum != 0)                                  spriteHolder.GetComponent<SpriteRenderer>().sprite = dragSprite;
        else if (!(kicking || stunned || sliding || charging)) spriteHolder.GetComponent<SpriteRenderer>().sprite = standingSprite;


        if ((!kicking && !stunned) || sliding) Move(); else velo = Vector3.zero;
        if (highlighting) spriteHolder.GetComponent<SpriteRenderer>().sprite = highlightsprite1;
		if(highlightHeld && ((Time.time * 8) % 2 > .5f)) spriteHolder.GetComponent<SpriteRenderer>().sprite = highlightsprite2;
		if(!(kicking || stunned || sliding || charging) && Input.GetKeyDown(keys.x(idStr))) dodge(); 

		//print(id + (kicking ? " kicking " : "") + (stunned ? " stunned " : "") + (sliding ? " sliding" : ""));
		//if(stunned) print(idStr);

		Timers();

        if (dragTileNum != 0 && !highlighting && (Input.GetButtonUp("pd"+idStr) || Input.GetKeyUp(keys.a(idStr)))) HandleTileDrop();
        if ((Input.GetKeyDown(keys.b(idStr)) || Input.GetButtonDown("b"+idStr)) && !kicking && !stunned) Kick();
		if ((Input.GetKeyUp  (keys.b(idStr)) || Input.GetButtonUp  ("b"+idStr)) && !stunned) KickRelease(); //this may get called while in standing kick multiple times

        if(dragTileNum == 0 && !dodging) Orient();
        else spriteHolder.transform.Rotate(new Vector3(0,Mathf.LerpAngle(spriteHolder.transform.localEulerAngles.y,spriteHolder.transform.localEulerAngles.y > 90 ? 180 : 0,Time.deltaTime * 4) - spriteHolder.transform.localEulerAngles.y,0));

        if (kicking){
			for(int i = 0; i < gc.players.Count; i++){
				if(gameObject != gc.players[i] && gc.players[i].GetComponent<BoxCollider>().bounds.Intersects(GetComponent<BoxCollider>().bounds)){
							if(playersHit >> gc.players[i].GetComponent<ObjT>().id == 0 && !(gc.players[i].GetComponent<PMove>().dodging || gc.players[i].GetComponent<PMove>().invincible))
							{
								gc.players[i].GetComponent<PMove>().GotKicked(chargedTime);
								playersHit += ((playersHit >> gc.players[i].GetComponent<ObjT>().id) + 1) << gc.players[i].GetComponent<ObjT>().id;
								GetComponent<AudioSource>().volume = .5f + (chargedTime);
								GetComponent<AudioSource>().PlayOneShot(kick);
								
							} 
				}
			}
		}
	}





	void dodge(){
		stunned = true;
		spriteHolder.GetComponent<SpriteRenderer>().sprite = backSprite;
		spriteHolder.GetComponent<SpriteRenderer>().color  = new Color(.8f,.8f,.8f,.9f);
		dodging = true;
		Invoke("undodge",1f);
	}

	void undodge(){
		dodging = false;
		stunned = false;
		spriteHolder.GetComponent<SpriteRenderer>().color  = new Color(1,1,1,1);
	}


	void Orient(){
		Transform t = spriteHolder.transform;
		spriteHolder.GetComponent<SpriteRenderer>().flipX = sliding && (face.x < 0 || (face.x == 0 && face.z > 0));
        spriteHolder.GetComponent<SpriteRenderer>().flipY = sliding && (face.x < 0 || (face.x == 0 && face.z > 0));
	

		if(Mathf.Abs(velo.x) < .1 && charging) t.Rotate(new Vector3(0,Mathf.LerpAngle(t.localEulerAngles.y,t.localEulerAngles.y > 90 ? 180 : 0,Time.deltaTime * 4) - t.localEulerAngles.y,0));//t.localEulerAngles = new Vector3(t.localEulerAngles.x,Mathf.LerpAngle(t.localEulerAngles.y,t.localEulerAngles.y > 90 ? 180 : 0,Time.deltaTime * 5),t.localEulerAngles.z);

		if(Mathf.Abs(velo.x) < .1 && charging) return;
		if(face.x >  0) t.Rotate(new Vector3(0,Mathf.LerpAngle(t.localEulerAngles.y,0  ,Time.deltaTime * 4) - t.localEulerAngles.y,0));//t.localEulerAngles = new Vector3(t.localEulerAngles.x,Mathf.LerpAngle(t.localEulerAngles.y,0  ,Time.deltaTime * 6),t.localEulerAngles.z);
		if(face.x <  0) t.Rotate(new Vector3(0,Mathf.LerpAngle(t.localEulerAngles.y,180,Time.deltaTime * 4) - t.localEulerAngles.y,0));//t.localEulerAngles = new Vector3(t.localEulerAngles.x,Mathf.LerpAngle(t.localEulerAngles.y,180,Time.deltaTime * 6),t.localEulerAngles.z);
		if(face.x == 0) {
			if(oldX == 1)
				t.Rotate(new Vector3(0,Mathf.LerpAngle(t.localEulerAngles.y,  0,Time.deltaTime * 4) - t.localEulerAngles.y,0));//t.localEulerAngles = new Vector3(t.localEulerAngles.x,Mathf.LerpAngle(t.eulerAngles.y,180,Time.deltaTime * 5),t.localEulerAngles.z);
			if(oldX == -1)
				t.Rotate(new Vector3(0,Mathf.LerpAngle(t.localEulerAngles.y,180,Time.deltaTime * 4) - t.localEulerAngles.y,0));//t.localEulerAngles = new Vector3(t.localEulerAngles.x,Mathf.LerpAngle(t.eulerAngles.y,0  ,Time.deltaTime * 5),t.localEulerAngles.z);
		}
		if(face.x > 0) oldX =  1;
		if(face.x < 0) oldX = -1; 
		t.localEulerAngles = new Vector3(0,t.localEulerAngles.y,t.localEulerAngles.z);
	}

	void Timers(){
		if(charging) chargedTime = Mathf.Clamp(chargedTime + Time.deltaTime,0,3);
	}

	void Kick(){
		//GetComponent<Animator>().SetBool("walking",true);
		if(Mathf.Pow(Sqr(velo.x)+Sqr(velo.z),.5f) > .1f){
			charging = true;
			spriteHolder.GetComponent<SpriteRenderer>().sprite = chargeSprite;
			/*kicking = true;
			sliding = true;
            spriteHolder.GetComponent<SpriteRenderer>().sprite = slideSprite;
			spriteHolder.transform.Rotate(Vector3.forward * 90 * face.x); */
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
			spriteHolder.transform.Rotate(Vector3.back * 90 * face.x);
			stunned = true;
			Invoke("Unoof",.6f);
		} else {
            spriteHolder.GetComponent<SpriteRenderer>().sprite = kickSprite;
			charging = false;
			kicking = true;
			if(!hit) Invoke("Unkick",.35f);
			else hit = false;
		}
	}

	void Unkick(){
		CancelInvoke("Unkick");//safety reasons
		if((Random.value > .1f || chargedTime < .3f)) {LandKick(); return;}
		kicking = false;
        spriteHolder.GetComponent<SpriteRenderer>().sprite = oofSprite;
		stunned = true;
		Invoke("Unoof",.5f);

		chargedTime = 0;
	}

	void Unoof(){
		stunned = false;
        spriteHolder.GetComponent<SpriteRenderer>().sprite = standingSprite;

	}
	void UnoofI(){
		invincible = true;
		stunned = false;
        spriteHolder.GetComponent<SpriteRenderer>().sprite = standingSprite;
   		spriteHolder.GetComponent<SpriteRenderer>().color  = new Color(.8f,.8f,.8f,.9f);

        Invoke("UnInvincible",.45f);
	}

	void UnInvincible(){
		spriteHolder.GetComponent<SpriteRenderer>().color  = new Color(1,1,1,1);
		invincible = false;
	}

	public void LandKick(){
		CancelInvoke("Unkick");
		kicking = false;
        spriteHolder.GetComponent<SpriteRenderer>().sprite = standingSprite;

		chargedTime = 0;
	}

	public void GotKicked(float time){
        Unhighlight();
        //GamePad.SetVibration((PlayerIndex)(id - 1), .2f,.2f);

        GetComponent<CameraShakeScript>().activate(.1f,.1f);

		if(stunned) return;
		stunned = true;
        spriteHolder.GetComponent<SpriteRenderer>().sprite = oofSprite;
		Invoke("UnoofI",time + 1f);
		Invoke("unshake",.1f);

	}

	void unshake(){

	}

    private void HandleTileDrop()
    {
        if (dragTileGO == null) return;

        Square sqr  = gc.sBoard.TransformToSquare(dragTileGO.transform);

        if (sqr == null) return;
        else if (sqr.solNum != dragTileNum)
        {
            dragTileNum = 0;
            Destroy(dragTileGO);
            GetComponent<AudioSource>().PlayOneShot(wrong[Random.Range(0,2)]);
            GameObject explosion = Instantiate(failTileExplosion, tileSpawn);
            explosion.transform.parent = null;
            explosion.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "playground";
            explosion.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
            Destroy(explosion, 1.0f);
        }
        else if (sqr.ownedBy == -1)//0 || sqr.ownedBy == -1 || sqr.ownedBy == -2)
        {
        	if(sqr.ownedBy == -1) {
        		gc.autoFillGotFilled = true;
                if (Random.value >= ((gc.sBoard.tilesForPlayer(id) + 1) / (gc.sBoard.totalTiles() + 1)))
                {
                    stunned = true;
                    spriteHolder.GetComponent<SpriteRenderer>().sprite = winSprite;
                    Invoke("beginHigh", .4f);
                }
            }
            /* if (sqr.fillNum == -2)
            {
                powerUpCount++;
                if (powerUpCount >= 3)
                {
                    powerUpCount = 0;
                    stunned = true;
                    spriteHolder.GetComponent<SpriteRenderer>().sprite = winSprite;
                    Invoke("beginHigh", .4f);
                }
            } */
            sqr.fillNum = dragTileNum;
            sqr.ownedBy = id;
            gc.sBoard.SetSquare(sqr);
            GetComponent<AudioSource>().PlayOneShot(correct);

            dragTileNum = 0;
            dragTileGO.transform.parent = null;
            dragTileGO.transform.position = new Vector3(sqr.xMinLim + gc.sBoard.xRes / 2, -0.6f, sqr.zMinLim + gc.sBoard.zRes / 2);
            foreach(GameObject g in GameObject.FindGameObjectsWithTag("PickupTile")){
	        	if(gc.sBoard.TransformToSquare(g.transform) == sqr && g.GetComponent<ObjT>() != null && g.GetComponent<ObjT>().id == -1f){

	        		Destroy(g,.1f);

	        	}
       		}
            
        }

    }

    void beginHigh(){
    	stunned = false;
        highlighting = true;
    	Invoke("Unhighlight",6);
        GetComponent<AudioSource>().PlayOneShot(wooHoo);
    }

    void Unhighlight(){
    	highlighting = false;
    }


    void HandleHighLight(){

        Square sqr  = gc.sBoard.TransformToSquare(transform);

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("PickupTile")){
        	if(gc.sBoard.TransformToSquare(g.transform) == sqr && sqr != null){

        		highlightHeld = true;
        		gToOwn = g;
        		sqrToOwn = sqr;
                GetComponent<AudioSource>().PlayOneShot(highlightSuccess);
                Invoke("Own",.15f);

        	}
        }

	}


	void Own(){
		if(!highlightHeld) return;
     	gToOwn.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = dragTileSprite;
		sqrToOwn.ownedBy = id;
		gc.sBoard.SetSquare(sqrToOwn); 
	}

    void setMoving(){
    	CancelInvoke("setMoving");
    	if(travel == TS.standing) travel = TS.walking; 
    }


    void Move(){

        //naive
        //transform.position += new Vector3(Input.GetAxis("HorizontalJ") * mSpeed,0f,Input.GetAxis("VerticalJ") * mSpeed);

		Vector3 joy = new Vector3(Input.GetAxisRaw("HorizontalJ" + idStr),
			                    0,Input.GetAxisRaw("VerticalJ"  + idStr));
		if(joy.magnitude == 0) timeSinceStanding = Time.time;
		if(joy.magnitude == 1 && timeSinceStanding != 0) {
			if(Time.time - timeSinceStanding < .5f) travel = TS.running;
			timeSinceStanding = 0;
		}

		if(joy.magnitude < 1 && joy.magnitude != 0) {
			travel = TS.walking;
		}
		if(travel == TS.running && face.x * joy.x < 0){
			travel = TS.walking;
		}

		if(sliding){
			velo = Vector3.Lerp(velo,Vector3.zero,Time.deltaTime  * (Sqr(1.5f + velo.magnitude)));
			transform.position += velo * mSpeed;
			return;
		} 

		face = new Vector3(joy.x > 0 ? 1 : (joy.x == 0 ? 0 : -1),0,joy.z > 0 ? 1 : (joy.z == 0 ? 0 : -1));


		if(charging) return;

        //if(joy.magnitude > .01f) GetComponent<Animator>().SetBool("walking",true);

        /*
		if(joy.magnitude < .1f) {
			travel = TS.standing;

		}else if(joy.magnitude < .9f && walking){
			transform.position += joy * mSpeed;
			velo = Vector3.zero;
			tarVelo = Vector3.zero;
			if(travel == TS.standing) Invoke("setMoving",timeToDash);
			else travel = TS.walking;
			return;
		}a

		if(face != runningFace && travel == TS.running){
			velo = Vector3.zero;
			tarVelo = Vector3.zero;
			travel = TS.walking;
			Invoke("setMoving",timeToDash);
		}



		if((joy.magnitude > .95f) && travel == TS.standing){
			travel = TS.running;
			runningFace = face;
			print("dash");
		}
		if((joy.magnitude < .95f) && travel == TS.running){
			travel = TS.standing;
			if(travel == TS.standing) Invoke("setMoving",2 * timeToDash);
		}*/



        if (tarVelo.magnitude > .3f && dragTileNum == 0)
		{
	        spriteHolder.GetComponent<SpriteRenderer>().sprite =
	        		 walkSprites[(int)((Time.time * (travel == TS.running ? 6 : 4)) % walkSprites.Length)];
		}
		//print(Time.time.ToString() + " : " + ((Time.time * 4) % walkSprites.Length).ToString() + " > " + ((int)(( 4 * Time.time) % walkSprites.Length)).ToString());

		tarVelo = new Vector3(joy.x,0,joy.z);

		if(turning){
			/*if(velo.x * tarVelo.x < 0){
				velo = new Vector3(0,0,velo.z);
			}
			if(velo.z * tarVelo.z < 0){
				velo = new Vector3(velo.x,0,0);
			}*/ //turning doesn't keep momentum

			if((velo.z * tarVelo.z < 0 && velo.x * tarVelo.x <= 0)
				|| (velo.z * tarVelo.z <= 0 && velo.x * tarVelo.x < 0)){
				stunned = true;
				Invoke("Unoof",.1f);
			}
		}
		if(turning){
			if(velo.z * tarVelo.z <= 0 && velo.x * tarVelo.x <= 0){
				velo *= Time.deltaTime/2;
			}
		}


		//if(standing) tarVelo += new Vector3(velo.x == 0 && Sqr(joy.x) > .9f ? 500 * tarVelo.x * Time.deltaTime : 0,0, velo.z == 0 && Sqr(joy.z) > .9f ? 500 * tarVelo.z * Time.deltaTime : 0); //slam
		

		float dragH = 1;
		float dragV = 1;

		if(letgo){
			dragH = (joy.x == 0 ? (joy.z == 0 ? Sqr(dragFactor * velo.x) : rampFactor) : rampFactor);
			dragV = (joy.z == 0 ? (joy.x == 0 ? Sqr(dragFactor * velo.z) : rampFactor) : rampFactor);
		}

		float deltaH = Mathf.Lerp(velo.x,tarVelo.x,Time.deltaTime * dragH);
		float deltaV = Mathf.Lerp(velo.z,tarVelo.z,Time.deltaTime * dragV);

		velo = new Vector3(deltaH,0,deltaV);


		float diagonalC = Mathf.Clamp(Mathf.Pow(Sqr(velo.x)+Sqr(velo.z),.5f) - 1,0,2);

		/*if(travel == TS.running){
		//	transform.position += face * (mSpeed * 1.1f / (diagonalC + 1));
		} else {
			transform.position += velo * (mSpeed / (diagonalC + 1));
		}*/
		if(dragTileNum == 0) transform.position += velo * (mSpeed / (diagonalC + 1));
        else transform.position += velo * (mSpeed * .5f / (diagonalC + 1));



        if (velo.x > 0) face.x =  1;
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
        if (dragTileNum == 0 && !highlighting && other.CompareTag("PickupTile") && (Input.GetButtonDown("pd"+idStr) || Input.GetKeyDown(keys.a(idStr))) && face.x == 1)
        {
            dragTileNum = other.gameObject.GetComponent<ObjT>().id;
            dragTileGO = Instantiate(dragTilePrefab, tileSpawn);
            dragTileGO.GetComponentInChildren<SpriteRenderer>().sprite = dragTileSprite;
            dragTileGO.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = other.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
            dragTileGO.transform.GetChild(0).transform.localScale = new Vector3(0.15f, 0.14f, 0);
            GetComponent<AudioSource>().PlayOneShot(pickup);
        }
    }
}
