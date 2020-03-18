using UnityEngine;
using System.Collections;

public class MoveDown : MonoBehaviour {


    public float speed;
    public float maxSpeed = 15;

    public Sprite rollSprite;
    public Sprite sideSprite;

    public Sprite cont;
    public Sprite end;
    public Sprite init;

    public Sprite goldenRollSprite;
    public Sprite goldenSideSprite;

    public Sprite goldenCont;
    public Sprite goldenEnd;
    public Sprite goldenInit;

    public float initOffset;
    public float finalOffset;
    public float finalRollScale = 0.43f;
    public float rollLongitude = 15f;
    public float roleDropForce;
    public float roleFlipForce;
    public float maxShake;
    public float shakeTime;
    public float minShakeSpeed;
    public float shakeFactor;
    public float changeTime;
    

    public Vector3 initPosition;
    
    private bool insCont;
    private bool ended;

    private GameObject piece;
    private Vector2 myOffset;
    private Material material;
    private float distance;

    private Transform roll;
    private Vector3 rollScale;

    private Transform side;
    private Vector3 siceScale;
    private Vector3 initialSideScale;

    private Transform hole;
    private Transform emptyRoll;    

    private bool untillEnded;

   
    private Vector3 pos;
    private Vector3 posInter;

    private bool shakeUp;
    private float lastTime;

    private bool isReady;

    private bool rollLaunched = false;

    private float varAdvans = 0.8f;

    float acceleration = 0f;
    public float swipeAcceleration = 0.2f;
    public float friction = 0.04f;

    float lastSpeed;

    public float goldenRollMultiplier = 6f;
    bool nextGoldenRoll = false;
    bool goldenRoll = false;
    float goldenRollCounter = 0f;

    RollAudioStart rollAudioStart;

	// Use this for initialization
	void Start () {

        distance = 0;

        rollAudioStart = transform.Find("AudioStart").GetComponent<RollAudioStart>();

        initPosition = transform.position;
        
        piece = transform.Find("Piece").gameObject;
        myOffset = piece.GetComponent<Renderer>().material.mainTextureOffset;
        myOffset.y = initOffset;       

        insCont = false;
        ended = false;
        untillEnded = false;

        roll = transform.Find("Roll");
        rollScale = new Vector3(1,1,1);

        side = transform.Find("Side");
        initialSideScale = side.localScale;
        siceScale = initialSideScale;

        emptyRoll = transform.Find("Empty_roll");
        hole = emptyRoll.Find("Hole");

        pos = initPosition;
        posInter = new Vector3();

        shakeUp = true;

        isReady = true;

        ReInitVariables();

        DataManager.LoadUpgrades();
        //GUIManager.UpdateToiletPaper();
	}
	
	// Update is called once per frame
	void Update() {

        // apagar el sonido con poca velocidad
        if (speed < 0.1f)
            GetComponent<AudioSource>().Stop();

        // Conteo del rollo dorado
        goldenRollCounter += Time.deltaTime;
        if (goldenRollCounter > DataManager.goldenRollTime)
        {
            goldenRollCounter = 0f;
            nextGoldenRoll = false;
        }

        speed += acceleration;
        if (!ignoreFriction)
            speed -= friction;
        speed = Mathf.Clamp(speed, 0, maxSpeed);

        DataManager.userMPS = speed;

        if (isReady)
        {
            Shake();
            if (!ended)
                distance += Time.deltaTime * speed;
          
            if (distance + initOffset < 0)
            {
                myOffset.y = (distance + initOffset) * varAdvans;
            }
            else if (distance < rollLongitude)
            {
                myOffset.y += (Time.deltaTime * speed) * varAdvans;
                if (!insCont)
                {
                    insCont = true;
                    if (goldenRoll)
                        piece.GetComponent<SpriteRenderer>().sprite = goldenCont;
                    else
                        piece.GetComponent<SpriteRenderer>().sprite = cont;
                    myOffset.y = -0.5f;
                }
                if (myOffset.y >= 0)
                {
                    float oldOffset = myOffset.y;
                    oldOffset = oldOffset % 1f;
                    myOffset.y = -0.5f + oldOffset;
                }
            }
            else
            {
                myOffset.y += (Time.deltaTime * speed) * varAdvans;
                /*if (myOffset.y >= 0)
                {
                    myOffset.y = 0;                    
                }*/
                if (!ended)
                {
                    ended = true;
                    untillEnded = true;
                    if (goldenRoll)
                        piece.GetComponent<SpriteRenderer>().sprite = goldenEnd;
                    else
                        piece.GetComponent<SpriteRenderer>().sprite = end;
                    side.GetComponent<SpriteRenderer>().enabled = false;
                    //roll.GetComponent<SpriteRenderer>().enabled = false;
                    myOffset.y = 0f;
                    lastSpeed = Mathf.Max(0.8f, speed);
                }
                if (untillEnded && myOffset.y >= finalOffset - 0.5f)
                {
                    myOffset.y += (Time.deltaTime * lastSpeed) * varAdvans;
                    roll.GetComponent<SpriteRenderer>().enabled = false;

                    if (!rollLaunched)
                    {
                        emptyRoll.GetComponent<SpriteRenderer>().enabled = false;
                        hole.GetComponent<SpriteRenderer>().enabled = false;
                        GameObject go = ObjectPool.instance.GetObjectForType("JumpRoll", false);
                        RollJump rj = go.GetComponent<RollJump>();
                        rj.parent = this;
                        //rj.isGolden = Random.Range(0, 100) < DataManager.goldenRollChance;
                        rj.isGolden = false;
                        rj.Restart();
                        go.transform.position = initPosition;
                        go.transform.localScale = transform.localScale * 2;
                        rollLaunched = true;
                    }
                }
                if (untillEnded && myOffset.y >= finalOffset)
                {
                    untillEnded = false;
                    speed = 5;
                    roll.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    Retire();

                    isReady = false;

                    // Inicializamos todo
                    ReInitVariables();
                    SetVisible();

                    // Recolocamos el rollo en un lateral
                    pos.x = initPosition.x + Screen.width * 1.5f;
                    transform.position = pos;
                }

            }

            piece.GetComponent<Renderer>().material.mainTextureOffset = myOffset;

            // Calculamos el valor de la escala del Rollen función de la distancia que queda hasta que se gaste
            float scale = (1 - finalRollScale) * (rollLongitude - distance) / rollLongitude + finalRollScale;

            if (scale <= finalRollScale) scale = finalRollScale;


            // Final scale de roll en x: 0.9
            // Final position de roll en x: -0.2
            rollScale.y = scale;
            rollScale.x = 1f - ((1 - scale) * 0.11f) / (1 - finalRollScale);
            roll.localScale = rollScale;

            Vector3 rollPosition = roll.localPosition;
            rollPosition.x = 0f - ((1 - scale) * 0.22f) / (1 - finalRollScale);
            roll.localPosition = rollPosition;

            Vector3 piecePosition = piece.transform.localPosition;
            piecePosition.x = 0f - ((1 - scale) * 0.489f) / (1 - finalRollScale) + 0.419f;
            piece.transform.localPosition = piecePosition;

            siceScale.x = scale;
            siceScale.y = scale;
            side.localScale = siceScale;
            
        }
        else {
            TranslateToInitPosition();
        }

        acceleration = 0;
	}

    public void TranslateToInitPosition() {

        pos.x = Mathf.Lerp(transform.position.x, initPosition.x, 1/changeTime);
        
        transform.position = pos;

        if (transform.position.x - initPosition.x < 0.1f) {
            transform.position = initPosition;
            isReady = true;
            speed = 0;
            //return true;
        }
        
        //return false;
    }

    public void ReInitVariables() {
        distance = 0;
        speed = 0;
        pos = initPosition;
        posInter = initPosition;
        myOffset.y = initOffset;
        insCont = false;
        ended = false;
        rollScale = new Vector3(1, 1, 1);
        siceScale = initialSideScale;
        side.GetComponent<SpriteRenderer>().enabled = true;
        roll.GetComponent<SpriteRenderer>().enabled = true;
        emptyRoll.GetComponent<SpriteRenderer>().enabled = true;
        hole.GetComponent<SpriteRenderer>().enabled = true;

        emptyRoll.localPosition = new Vector3(-0.45f, 0, 0);
        //emptyRoll.position = Vector3.zero;
        emptyRoll.rotation = Quaternion.identity;
        rollLaunched = false;

        //Instantiate(jumpRoll, transform.position, Quaternion.identity);

        // Rollo dorado
        if (nextGoldenRoll)
        {
            piece.GetComponent<SpriteRenderer>().sprite = goldenInit;
            roll.GetComponent<SpriteRenderer>().sprite = goldenRollSprite;
            side.GetComponent<SpriteRenderer>().sprite = goldenSideSprite;
            goldenRoll = true;
            changeTime = 2f;
        }
        else
        {
            piece.GetComponent<SpriteRenderer>().sprite = init;
            roll.GetComponent<SpriteRenderer>().sprite = rollSprite;
            side.GetComponent<SpriteRenderer>().sprite = sideSprite;
            goldenRollCounter = 0f;
            goldenRoll = false;
            changeTime = 4f;
        }
    }

    
    private void SetInvisible()
    {
        piece.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        roll.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void SetVisible()
    {
        piece.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        roll.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        side.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    private bool Retire()
    {
        // Calculamos las velocidades
        side.gameObject.GetComponent<SpriteRenderer>().enabled = false;    
        return false;
    }

    private void Shake() {

        if (speed > minShakeSpeed)
        {
            if (Time.fixedTime - lastTime > shakeTime)
            {
                //Debug.Log("Delta time: " + Time.fixedTime + ", Last time: " + lastTime);
                lastTime = Time.fixedTime;
                if (shakeUp)
                {
                    //Debug.Log("ShakeUp");
                    shakeUp = false;
                    pos.y = initPosition.y + Mathf.Min(maxShake, speed / (1 / shakeFactor));
                    pos.x = initPosition.x + Mathf.Min(maxShake, speed / (1 / shakeFactor));
                }
                else
                {
                    shakeUp = true;
                    pos.y = initPosition.y;
                    pos.x = initPosition.x;
                }
            }
        }
        else {
            pos = initPosition;
        }
        transform.position = pos;
    }

    bool ignoreFriction = false;

    public void OnGesture(Gesture gesture)
    {
        //if (tabPanel.Position.y > 0)
        {
            if (gesture.type == GestureType.Drag)
            {
                
                Vector3 wp = Camera.main.ScreenToWorldPoint(gesture.position);
                wp.z = 0f;
                if (GetComponent<Collider>().bounds.Contains(wp))
                {
                    if (gesture.delta.y < 0)
                    {
                        float gDistance = gesture.delta.y;
                        // Velocidad = espacio / tiempo
                        float gSpeed = gDistance / gesture.gestureTime;

                        float baseSpeed = Mathf.Abs(gSpeed * (goldenRoll ? DataManager.userSpeed * goldenRollMultiplier : DataManager.userSpeed));
                        speed += (baseSpeed + DataManager.userMPSSumIncrement) * DataManager.userMPSMulIncrement;

                        ignoreFriction = true;
                    }
                }                
            }
            if (gesture.type == GestureType.Swipe)
            {
                ignoreFriction = false;
                Vector3 wp = Camera.main.ScreenToWorldPoint(gesture.position);
                wp.z = 0f;
                if (GetComponent<Collider>().bounds.Contains(wp))
                {

                    //if (gesture.delta.y < 0)
                    
                    float gDistance = gesture.delta.y;
                    // Velocidad = espacio / tiempo
                    float gSpeed = gDistance / gesture.gestureTime;

                    float baseSpeed = Mathf.Abs(gSpeed * (goldenRoll ? DataManager.userSpeed * goldenRollMultiplier : DataManager.userSpeed));
                    speed += (baseSpeed + DataManager.userMPSSumIncrement) * DataManager.userMPSMulIncrement;


                    if (speed >0.2f)
                        rollAudioStart.StartClip();

                    if (!GetComponent<AudioSource>().isPlaying)
                        GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    public void StartGoldenRoll()
    {
        nextGoldenRoll = true;
    }
}