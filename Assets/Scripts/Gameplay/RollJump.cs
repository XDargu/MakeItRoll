using UnityEngine;
using System.Collections;

public class RollJump : MonoBehaviour {

    public float roleDropForceX = 20f;
    public float roleDropForceY = 45f;
    public float roleFlipForce = 200f;

    public float randomFlip = 50f;
    public float randomForceX = 5f;
    public float randomForceY = 8f;

    public MoveDown parent;
    public bool isGolden = false;

    public Sprite roll;
    public Sprite goldenRoll;

    public AudioClip[] throwRollSounds;
    public AudioClip[] fallRollSounds;

    bool outOfScreenDone = false;

	// Use this for initialization
	void Start () {
        if (isGolden)
        {
            GetComponent<SpriteRenderer>().sprite = goldenRoll;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = roll;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!outOfScreenDone)
        {
            if (OutOfScreen())
            {
                GetComponent<AudioSource>().clip = fallRollSounds[Random.Range(0, fallRollSounds.Length)];
                GetComponent<AudioSource>().Play();
                Invoke("PoolObject", GetComponent<AudioSource>().clip.length);
                outOfScreenDone = true;
            }
        }
        if (isGolden)
        {
            if (Input.touchCount > 0)
            {
                Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Vector2 touchPos = new Vector2(wp.x, wp.y);
                if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                {
                    parent.StartGoldenRoll();
                    ObjectPool.instance.PoolObject(gameObject);
                }
            }
        }
	}

    void PoolObject()
    {
        ObjectPool.instance.PoolObject(gameObject);
    }

    public void Restart()
    {
        outOfScreenDone = false;
        GetComponent<AudioSource>().clip = throwRollSounds[Random.Range(0, throwRollSounds.Length)];
        GetComponent<AudioSource>().Play();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        GetComponent<Rigidbody2D>().AddTorque(roleFlipForce + Random.Range(-randomFlip * 0.5f, randomFlip * 0.5f));
        GetComponent<Rigidbody2D>().AddForce(Vector3.up * (roleDropForceY + Random.Range(-randomForceX * 0.5f, randomForceX * 0.5f)) + Vector3.left * (roleDropForceX + Random.Range(-randomForceY * 0.5f, randomForceY * 0.5f)));
        if (isGolden)
        {
            GetComponent<SpriteRenderer>().sprite = goldenRoll;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = roll;
        }
    }

    private bool OutOfScreen()
    {
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(-0.5f, -0.5f, Camera.main.nearClipPlane));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1.5f, 1.5f, Camera.main.nearClipPlane));
        return (transform.position.x < bottomLeft.x) || (transform.position.x > topRight.x) || (transform.position.y > topRight.y) || (transform.position.y < bottomLeft.y);
    }
}

