using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum GestureType
{
    Swipe = 1,
    DoubleSwipe = 2,
    Pinch = 3,
    Tap = 4,
    Drag = 5,
    DoubleDrag = 6,
    VerticalDrag = 7,
    HoriztonalDrag = 8,
    RightMouseDrag = 9,
    LeftMouseDrag = 10,
    MiddleMouseDrag = 11,
    MouseWheel = 12,
    RightMouseTap = 13,
    LeftMouseTap = 14,
    MiddleMouseTap = 15,
}

public struct Gesture
{
    public GestureType type;
    public Vector2 position, position2, delta, delta2;
    public Collider[] colliders;
    public float gestureTime;

    public Gesture(GestureType type, Vector2 position, float gestureTime, Collider[] colliders)
    {
        this.type = type;
        this.position = position;
        this.position2 = Vector2.zero;
        this.delta = Vector2.zero;
        this.delta2 = Vector2.zero;
        this.colliders = colliders;
        this.gestureTime = gestureTime;
    }

    public Gesture(GestureType type, Vector2 position, Vector2 delta, float gestureTime, Collider[] colliders)
    {
        this.type = type;
        this.position = position;
        this.position2 = Vector2.zero;
        this.delta = delta;
        this.delta2 = Vector2.zero;
        this.colliders = colliders;
        this.gestureTime = gestureTime;
    }

    public Gesture(GestureType type, Vector2 position, Vector2 delta, Vector2 position2, Vector2 delta2, float gestureTime, Collider[] colliders)
    {
        this.type = type;
        this.position = position;
        this.position2 = position2;
        this.delta = delta;
        this.delta2 = delta2;
        this.colliders = colliders;
        this.gestureTime = gestureTime;
    }
}

public class GestureManager : MonoBehaviour {

    class FingerTouch
    {
        public int fingerID;
        public float startTime;
        public Vector2 initialPosition;
        public Vector2 position;
        public Vector2 delta;
        public bool ended;
        public Collider[] colliders;

        public FingerTouch(int fingerID, Vector2 position)
        {
            this.fingerID = fingerID;
            this.initialPosition = position;
            this.position = position;
            this.delta = Vector2.zero;
            this.startTime = Time.time;
            this.ended = false;

            // Find colliders in the touch position
            RaycastHit[] hits;
            hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(initialPosition)).OrderBy(h => h.distance).ToArray(); ;
            colliders = new Collider[hits.Length];
            for (int i=0; i<hits.Length; i++)
            {
                colliders[i] = hits[i].collider;
            }
        }

        public void MoveFinger(Vector2 newPosition)
        {
            this.delta = newPosition - this.position;
            this.position = newPosition;
        }

        public Vector2 Travel
        {
            get
            {
                return position - initialPosition;
            }
        }

        public Vector2 NormalizedTravel
        {
            get
            {
                Vector2 travel = position - initialPosition;
                return travel == Vector2.zero ? Vector2.zero : new Vector2(travel.x / Screen.width, travel.y / Screen.height);
            }
        }

        public Vector2 NormalizedDelta
        {
            get
            {
                return delta == Vector2.zero ? Vector2.zero : new Vector2(delta.x / Screen.width, delta.y / Screen.height);
            }
        }
    }

    public float    maxDuration = 0.25f,
                    minDragScreenTravel = 0.001f,
                    minSwipeScreenTravel = 0.2f, 
                    minPinchScreenTravel = 0.1f, 
                    maxTapScreenTravel = 0.05f, 
                    maxTapDuration = 0.5f;

    float           maxParallelDotDeviation = 0.2f, 
                    maxOpposedDotDeviant = 0.5f;

    Dictionary<int, FingerTouch> fingerTouches = new Dictionary<int, FingerTouch>();
    public List<MonoBehaviour> receivers = new List<MonoBehaviour>();

    public Text log;

    Collider[] collidersTouched;

    bool SendGesture(Gesture gesture)
    {
        foreach (MonoBehaviour receiver in receivers)
        {
            receiver.SendMessage("OnGesture", gesture);
        }

        //fingerTouches.Clear();
        return true;
    }

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        RecordPatterns();
        List<FingerTouch> drags = GetValidDrags();
        DetectTap();
        DetectDrag(drags);
        DetectDoubleDrag(drags);
        DetectSwipe(drags);
        DetectPinch(drags);
        if (log != null)
        {
            LogTouches();
        }
	}

    void LogTouches()
    {
        string log = "Log: (" + fingerTouches.Values.Count + " touches)\n";
        foreach (FingerTouch touch in fingerTouches.Values)
        {
            log += "ID: " + touch.fingerID + "\n";
            log += "Position: " + touch.position + "\n";
            log += "Delta: " + touch.NormalizedDelta + "\n";
            log += "Ended: " + touch.ended + "\n";
        }
        this.log.text = log;
    }

    void DetectSwipe(List<FingerTouch> drags)
    {
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
            if (drags.Count == 1)
            {
                // El toque debe estar terminado
                if (drags[0].ended)
                {
                    SendGesture(new Gesture(GestureType.Swipe, drags[0].position, drags[0].NormalizedDelta, Time.time - drags[0].startTime, drags[0].colliders));
                }
            }
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
            foreach (FingerTouch drag in drags)
            {
                if (drag.fingerID == 0)
                    SendGesture(new Gesture(GestureType.LeftMouseDrag, drags[0].position, drags[0].NormalizedDelta, Time.time - drags[0].startTime,drag.colliders));
                if (drag.fingerID == 1)
                    SendGesture(new Gesture(GestureType.RightMouseDrag, drags[0].position, drags[0].NormalizedDelta, Time.time - drags[0].startTime,drag.colliders));
                if (drag.fingerID == 2)
                    SendGesture(new Gesture(GestureType.MiddleMouseDrag, drags[0].position, drags[0].NormalizedDelta, Time.time - drags[0].startTime,drag.colliders));
            }
#endif
    }

    void DetectTap()
    {
        foreach (FingerTouch touch in fingerTouches.Values)
        {
            // El toque se detecta al terminarse
            if (touch.ended)
            {
                // Condiciones de toque: en un mismo lugar y en un periodo corto de tiempo
                if (Time.time - touch.startTime < maxTapDuration && touch.NormalizedTravel.magnitude < maxTapScreenTravel)
                {
                    #if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
                    SendGesture(new Gesture(GestureType.Tap, touch.position, Time.time - touch.startTime, touch.colliders));
                    #else
                    if (touch.fingerID == 0)
                        SendGesture(new Gesture(GestureType.LeftMouseTap, touch.position, touch.colliders));
                    if (touch.fingerID == 1)
                        SendGesture(new Gesture(GestureType.RightMouseTap, touch.position, touch.colliders));
                    if (touch.fingerID == 2)
                        SendGesture(new Gesture(GestureType.MiddleMouseTap, touch.position, touch.colliders));
                    #endif
                }
            }
        }
    }

    List<FingerTouch> GetValidDrags()
    {
        List<FingerTouch> drags = new List<FingerTouch>();
        foreach (FingerTouch touch in fingerTouches.Values)
        {
            // El toque no tiene por qué estar terminado, el drag puede darse en cualquier momento
            // El toque debe superar el mínimo de distancia recorrida para ser reconocido como Drag
            if (touch.NormalizedTravel.magnitude > minDragScreenTravel)
            {
                // Drag válido
                drags.Add(touch);
            }
        }

        return drags;
    }

    void DetectDrag(List<FingerTouch> drags)
    {
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
        if (drags.Count == 1)
        {
            SendGesture(new Gesture(GestureType.Drag, drags[0].position, drags[0].NormalizedDelta, Time.time - drags[0].startTime, drags[0].colliders));
            // Se envián también el horizontal y vertical
            if (drags[0].NormalizedTravel.x > minDragScreenTravel)
                SendGesture(new Gesture(GestureType.HoriztonalDrag, drags[0].position, new Vector2(drags[0].NormalizedDelta.x, 0), Time.time - drags[0].startTime, drags[0].colliders));
            if (drags[0].NormalizedTravel.y > minDragScreenTravel)
                SendGesture(new Gesture(GestureType.VerticalDrag, drags[0].position, new Vector2(0, drags[0].NormalizedDelta.y), Time.time - drags[0].startTime, drags[0].colliders));
        }
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
        if (drags.Count > 0)
        {
            foreach (FingerTouch drag in drags)
            {
                if (drag.fingerID == 0)
                    SendGesture(new Gesture(GestureType.LeftMouseDrag, drags[0].position, drags[0].NormalizedDelta, Time.time - drags[0].startTime, drag.colliders));
                if (drag.fingerID == 1)
                    SendGesture(new Gesture(GestureType.RightMouseDrag, drags[0].position, drags[0].NormalizedDelta, Time.time - drags[0].startTime, drag.colliders));
                if (drag.fingerID == 2)
                    SendGesture(new Gesture(GestureType.MiddleMouseDrag, drags[0].position, drags[0].NormalizedDelta, Time.time - drags[0].startTime, drag.colliders));
            }
        }
#endif
    }

    void DetectDoubleDrag(List<FingerTouch> drags)
    {
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
        if (drags.Count == 2)
        {
            // Comprobar si el ángulo entre los dos vectores es inferior al mínimo establecido
            if (Mathf.Abs(1 - Vector2.Dot(drags[0].Travel.normalized, drags[1].Travel.normalized)) < maxParallelDotDeviation)
            {
                Vector2 meanPosition = (drags[0].position + drags[1].position) / 2;
                Vector2 meanNormalizedDelta = (drags[0].NormalizedDelta + drags[1].NormalizedDelta) / 2;
                SendGesture(new Gesture(GestureType.DoubleDrag, meanPosition, meanNormalizedDelta, Time.time - drags[0].startTime, drags[0].colliders));
            }
        }
#endif
    }

    void DetectPinch(List<FingerTouch> drags)
    {
        
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
        if (drags.Count == 2)
        {
            // Comprobar si el ángulo entre los dos vectores es superior al máximo establecido para que sean opuestos
            if (Mathf.Abs(-1 - Vector2.Dot(drags[0].Travel.normalized, drags[1].Travel.normalized)) < maxOpposedDotDeviant)
            {
                float startTime = drags[0].startTime < drags[1].startTime ? drags[0].startTime : drags[1].startTime;
                SendGesture(new Gesture(GestureType.Pinch, drags[0].position, drags[0].NormalizedDelta, drags[1].position, drags[1].NormalizedDelta, Time.time - startTime, new Collider[0]));
            }
        }
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
        // Wheel back
        if (drags.Count > 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                SendGesture(new Gesture(GestureType.MouseWheel, Input.mousePosition, new Vector2(Input.GetAxis("Mouse ScrollWheel"), 0), Time.time - drags[0].startTime, new Collider[0]));
            }
            // Wheel forward
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                SendGesture(new Gesture(GestureType.MouseWheel, Input.mousePosition, new Vector2(Input.GetAxis("Mouse ScrollWheel"), 0), Time.time - drags[0].startTime, new Collider[0]));
            }
        }
#endif
    }

    void RecordPatterns()
    {
        
        // Eliminar del diccionario los toques marcados como finalizados
        List<int> itemsToRemove = new List<int>();
        foreach (var pair in fingerTouches)
        {
            if (pair.Value.ended)
                itemsToRemove.Add(pair.Key);
        }
        foreach (int item in itemsToRemove)
        {
            fingerTouches.Remove(item);
        }

#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
        // Detectar toques, utilizado los eventos táctiles de Unity        
        foreach (Touch touch in Input.touches)
        {
            if (fingerTouches.ContainsKey(touch.fingerId))
            {
                /*if (Time.time - fingerTouches[touch.fingerId].startTime > maxDuration)
                {
                    fingerTouches.Remove(touch.fingerId);
                }
                else*/
                {
                    FingerTouch fingerTouch = fingerTouches[touch.fingerId];
                    switch (touch.phase)
                    {
                        case TouchPhase.Ended:
                            // Marcar toque como finalizado
                            fingerTouch.ended = true;
                            break;
                        case TouchPhase.Canceled:
                            fingerTouches.Remove(touch.fingerId);
                            break;
                        case TouchPhase.Moved:
                            fingerTouches[touch.fingerId].MoveFinger(touch.position);
                            break;
                        case TouchPhase.Stationary:
                            fingerTouches[touch.fingerId].MoveFinger(touch.position);
                            break;
                    }                    
                }
            }
            else if (touch.phase == TouchPhase.Began)
            {
                fingerTouches[touch.fingerId] = new FingerTouch(touch.fingerId, touch.position);
            }
        }
#endif

#if UNITY_STANDALONE || UNITY_EDITOR
        for (int touchID = 0; touchID < 3; touchID++)
        {
            // Si pulsamos el ratón por primera vez, se añade
            if (Input.GetMouseButtonDown(touchID))
            {
                fingerTouches[touchID] = new FingerTouch(touchID, Input.mousePosition);
            }

            // Si levantamos el ratón, se marca para eliminar
            else if (Input.GetMouseButtonUp(touchID))
            {
                fingerTouches[touchID].ended = true;
            }

            // Si estña pulsado pero no lo hemos pulsado por primera vez ni levantado
            else if (Input.GetMouseButton(touchID))
            {
                fingerTouches[touchID].MoveFinger(Input.mousePosition);
            }
        }  
#endif
    }
}
