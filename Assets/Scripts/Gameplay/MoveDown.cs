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

    public Vector3 finalRollScale;
    public Vector3 finalRollPosition;
    public Vector3 finalSideScale;
    public Vector3 finalPiecePosition;

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
    private Material material;
    public float distance;

    private Transform roll;
    private Vector3 rollScale;
    private Vector3 initialRollScale;
    private Vector3 initialRollPosition;

    private Transform side;
    private Vector3 siceScale;
    private Vector3 initialSideScale;

    private Vector3 initialPiecePosition;

    private Transform hole;
    private Transform emptyRoll;

    private bool untillEnded;

   
    private Vector3 pos;
    private Vector3 posInter;

    private bool shakeUp;
    private float lastTime;

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

    AudioSource mAudioSource;

    RollAudioStart rollAudioStart;

    public float timeToChangeRoll = 5.0f;
    private float timerChangeRoll;

    enum EState
    {
        FirstPiece,
        Middle,
        EndPiece,
        ChangingRoll,
        ServingNewRoll
    }

    EState state = EState.FirstPiece;

	// Use this for initialization
	void Start () {

        GameObject.Find("GestureManager").GetComponent<GestureManager>().receivers.Add(this);

        distance = 0;

        mAudioSource = GetComponent<AudioSource>();

        rollAudioStart = transform.Find("AudioStart").GetComponent<RollAudioStart>();

        initPosition = transform.position;
        
        piece = transform.Find("Piece").gameObject;

        initialPiecePosition = piece.transform.localPosition;

        insCont = false;
        ended = false;
        untillEnded = false;

        roll = transform.Find("Roll");
        rollScale = new Vector3(1,1,1);
        initialRollScale = roll.localScale;
        initialRollPosition = roll.transform.localPosition;

        side = transform.Find("Side");
        initialSideScale = side.localScale;
        siceScale = initialSideScale;

        emptyRoll = transform.Find("EmptyRoll");
        hole = emptyRoll.Find("Hole");

        pos = initPosition;
        posInter = new Vector3();

        shakeUp = true;

        ReInitVariables();

        DataManager.LoadUpgrades();
        //GUIManager.UpdateToiletPaper();
	}

    void UpdateRollCounter()
    {
        goldenRollCounter += Time.deltaTime;
        if (goldenRollCounter > DataManager.goldenRollTime)
        {
            goldenRollCounter = 0f;
            nextGoldenRoll = false;
        }
    }

    void UpdateSpeed()
    {
        speed += acceleration;

        if (!ignoreFriction)
        {
            speed -= friction;
        }
        speed = Mathf.Clamp(speed, 0, maxSpeed);
        DataManager.userMPS = speed;
    }

    void UpdateScale()
    {
        // Calculamos el valor de la escala del Roll en función de la distancia que queda hasta que se gaste
        float remainingDistance = rollLongitude - distance;
        float scaleFactor = remainingDistance / rollLongitude;

        roll.localScale = Vector3.Lerp(finalRollScale, initialRollScale, scaleFactor);
        roll.transform.localPosition = Vector3.Lerp(finalRollPosition, initialRollPosition, scaleFactor);

        side.localScale = Vector3.Lerp(finalSideScale, initialSideScale, scaleFactor);

        Vector3 piecePosition = piece.transform.localPosition;
        piecePosition.x = Mathf.Lerp(finalPiecePosition.x, initialPiecePosition.x, scaleFactor);
        piece.transform.localPosition = piecePosition;
    }

    void UpdateRolling()
    {
        distance += Time.deltaTime * speed;

        Vector3 piecePosition = piece.transform.localPosition;
        piecePosition.y = piecePosition.y - speed;
        piece.transform.localPosition = piecePosition;
    }
	
	// Update is called once per frame
	void Update()
    {
        if (speed < 0.1f)
        {
            mAudioSource.Stop();
        }

        UpdateSpeed();
        UpdateRollCounter();

        switch(state)
        {
            case EState.FirstPiece:
                {
                    Shake();
                    UpdateRolling();
                    UpdateScale();

                    if (piece.transform.localPosition.y < -12.67)
                    {
                        ResetPieceOffset();

                        state = EState.Middle;
                        if (goldenRoll)
                        {
                            piece.GetComponent<SpriteRenderer>().sprite = goldenCont;
                        }
                        else
                        {
                            piece.GetComponent<SpriteRenderer>().sprite = cont;
                        }
                    }
                }
                break;
            case EState.Middle:
                {
                    Shake();
                    UpdateRolling();
                    UpdateScale();

                    if (piece.transform.localPosition.y < -11.1)
                    {
                        ResetPieceOffset();
                    }

                    if (distance > rollLongitude)
                    {
                        ResetPieceOffset();

                        state = EState.EndPiece;
                        if (goldenRoll)
                        {
                            piece.GetComponent<SpriteRenderer>().sprite = goldenEnd;
                        }
                        else
                        {
                            piece.GetComponent<SpriteRenderer>().sprite = end;
                        }

                        // Hide side
                        side.GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
                break;
            case EState.EndPiece:
                {
                    Shake();
                    UpdateRolling();
                    UpdateScale();

                    if (piece.transform.localPosition.y < -12.8)
                    {
                        state = EState.ChangingRoll;
                        ResetPieceOffset();

                        // Launch roll
                        GameObject go = ObjectPool.instance.GetObjectForType("RollJump", false);
                        RollJump rj = go.GetComponent<RollJump>();
                        rj.parent = this;
                        //rj.isGolden = Random.Range(0, 100) < DataManager.goldenRollChance;
                        rj.isGolden = false;
                        rj.Restart();
                        go.transform.position = initPosition;

                        SetInvisible();
                    }
                }
                break;
            case EState.ChangingRoll:
                {
                    timerChangeRoll += Time.deltaTime;
                    speed = 0;

                    if (timerChangeRoll > timeToChangeRoll)
                    {
                        timeToChangeRoll = 0.0f;

                        // Inicializamos todo
                        ResetPieceOffset();
                        ReInitVariables();
                        SetVisible();

                        // Recolocamos el rollo en un lateral
                        pos.x = initPosition.x + Screen.width * 1.5f;
                        transform.position = pos;

                        state = EState.ServingNewRoll;
                    }
                }
                break;
            case EState.ServingNewRoll:
                {
                    TranslateToInitPosition();
                    speed = 0;

                    if (transform.position.x - initPosition.x < 0.1f)
                    {
                        transform.position = initPosition;
                        state = EState.FirstPiece;
                    }
                }
                break;
        }

        acceleration = 0;
	}

    public void TranslateToInitPosition() {

        pos.x = Mathf.Lerp(transform.position.x, initPosition.x, 1/changeTime);
        transform.position = pos;
    }

    public void ReInitVariables()
    {
        timerChangeRoll = 0.0f;
        distance = 0;
        speed = 0;
        pos = initPosition;
        posInter = initPosition;
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
        side.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        emptyRoll.GetComponent<SpriteRenderer>().enabled = false;
        hole.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void SetVisible()
    {
        piece.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        roll.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        side.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        emptyRoll.GetComponent<SpriteRenderer>().enabled = true;
        hole.GetComponent<SpriteRenderer>().enabled = true;
    }

    private bool Retire()
    {
        // Calculamos las velocidades
        side.gameObject.GetComponent<SpriteRenderer>().enabled = false;    
        return false;
    }

    private void ResetPieceOffset()
    {
        Vector3 piecePosition = piece.transform.localPosition;
        piecePosition.y = initialPiecePosition.y;
        piece.transform.localPosition = piecePosition;
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
        else
        {
            pos = initPosition;
        }

        transform.position = pos;
    }

    bool ignoreFriction = false;

    public void OnGesture(Gesture gesture)
    {
        if (DataManager.InGame)
        {
            if (gesture.type == GestureType.Drag)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(gesture.position);
                if (GetComponent<BoxCollider2D>().OverlapPoint(mousePosition))
                {
                    if (gesture.delta.y < 0)
                    {
                        float distance = gesture.delta.y;
                        float gestureSpeed = distance / gesture.gestureTime;
                        float baseSpeed = Mathf.Abs(gestureSpeed * (goldenRoll ? DataManager.userSpeed * goldenRollMultiplier : DataManager.userSpeed));
                        float extraSpeed = (baseSpeed + DataManager.userMPSSumIncrement) * DataManager.userMPSMulIncrement;
                        speed += extraSpeed;

                        ignoreFriction = true;
                    }
                }                
            }
            if (gesture.type == GestureType.Swipe)
            {
                ignoreFriction = false;
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(gesture.position);
                if (GetComponent<BoxCollider2D>().OverlapPoint(mousePosition))
                {

                    float gDistance = gesture.delta.y;
                    float gSpeed = gDistance / gesture.gestureTime;

                    float baseSpeed = Mathf.Abs(gSpeed * (goldenRoll ? DataManager.userSpeed * goldenRollMultiplier : DataManager.userSpeed));
                    speed += (baseSpeed + DataManager.userMPSSumIncrement) * DataManager.userMPSMulIncrement;


                    if (speed >0.2f)
                        rollAudioStart.StartClip();

                    if (!mAudioSource.isPlaying)
                        mAudioSource.Play();
                }
            }
        }
    }

    public void StartGoldenRoll()
    {
        nextGoldenRoll = true;
    }
}