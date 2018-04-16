using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMinionController : MonoBehaviour {
    
    public float speed;
    public float timeLimit = 1.5f;
    public bool cooperate;
    public int maxHealth = 5;
    public int teamID = 2;
    //public enum Dimension { both, x, y };
    //public Dimension wanderDimension;
    //public float x_range;
    //public float y_range;

    private GameObject cam;
    private Rigidbody2D rb;
    private RaycastHit2D hit;
    private BlinkAnimation blinkAnimation;
    private float timer;
    private int currentHealth;
    private bool pursuePlayer;
    private Transform playerTransform;
    //private Vector3 targetPosition;


    // Use this for initialization
    void Start () {
        cam = GameObject.FindWithTag("MainCamera");
        rb = transform.GetComponent<Rigidbody2D>();
        blinkAnimation = transform.GetComponent<BlinkAnimation>();
        rb.velocity = transform.right * speed;
        timer = timeLimit;
        pursuePlayer = false;
        //SetRandomTarget();
        currentHealth = maxHealth;
	}

    private void Update()
    {
    }

    public Transform GetPlayerTransform()
    {
        if (pursuePlayer)
        {
            return playerTransform;
        }
        else
        {
            return null;
        }
    }

    private Vector2 getDirection()
    {
        float angle = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return direction;
    }

    private void FixedUpdate()
    {
        Vector2 dir = getDirection();
        Vector2 orig = new Vector2(transform.position.x, transform.position.y) + (dir * 0.5f);       

        hit = Physics2D.Raycast(orig, dir, Mathf.Infinity);
        if (hit.transform.CompareTag("Player"))
        {
            pursuePlayer = true;
            playerTransform = hit.transform;
        }
        else if (hit.transform.CompareTag("Minion"))
        {
            if (cooperate)
            {
                Transform pursueTransform = hit.transform.GetComponent<SimpleMinionController>().GetPlayerTransform();
                if (pursueTransform != null)
                {
                    pursuePlayer = true;
                    playerTransform = pursueTransform;
                }
            }
            
        }
        else
        {
            pursuePlayer = false;
            playerTransform = null;
        }

        if (!pursuePlayer)
        {
            if (timer <= 0)
            {
                // Wander around
                // Get random direction to turn to
                Vector2 randomDir = Random.insideUnitCircle;
                float aimHorizontal = randomDir.x;
                float aimVertical = randomDir.y;
                float angle = Mathf.Atan2(aimVertical, aimHorizontal) * Mathf.Rad2Deg;
                Quaternion newDir = Quaternion.Euler(0.0f, 0.0f, angle);

                SetDirection(newDir);

                timer = timeLimit;
            }
            else
            {
                timer = timer - Time.fixedDeltaTime;
            }

            //Vector3 offset = (targetPosition - transform.position);
            //while (offset.magnitude < 0.3f)
            //{
            //    SetRandomTarget();                
            //    Quaternion newDir = GetDirectionToTarget(targetPosition);
            //    SetDirection(newDir);
            //    offset = (targetPosition - transform.position);
            //}
            //Debug.Log(targetPosition);
        }
        else
        {
            //Quaternion newDir = GetDirectionToTarget(playerPosition);
            Quaternion newDir = GetDirectionToTarget(playerTransform.position);
            SetDirection(newDir);
        }
        
    }

    private Quaternion GetDirectionToTarget(Vector3 targetPos)
    {
        Vector3 targetOffset = targetPos - transform.position;
        float angle = Mathf.Atan2(targetOffset.y, targetOffset.x) * Mathf.Rad2Deg;
        return (Quaternion.Euler(0.0f, 0.0f, angle));
    }

    private void SetDirection(Quaternion newDir)
    {
        rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.Lerp(transform.rotation, newDir, Time.time);
        rb.velocity = transform.right * speed;
    }

    //private void SetRandomTarget()
    //{
    //    if (wanderDimension == Dimension.both)
    //    {
    //        targetPosition = new Vector3(Random.Range(-x_range, x_range), Random.Range(-y_range, y_range), transform.position.z);
    //    }
    //    else if (wanderDimension == Dimension.x)
    //    {
    //        targetPosition = new Vector3(Random.Range(-x_range, x_range), transform.position.y, transform.position.z);
    //    }
    //    else if (wanderDimension == Dimension.y)
    //    {
    //        targetPosition = new Vector3(transform.position.x, Random.Range(-y_range, y_range), transform.position.z);
    //    }

    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            pursuePlayer = true;
            playerTransform = collision.transform;
            Quaternion newDir = GetDirectionToTarget(playerTransform.position);
            SetDirection(newDir);
        }
        else if (collision.transform.CompareTag("Minion"))
        {
            Transform pursueTransform = collision.transform.GetComponent<SimpleMinionController>().GetPlayerTransform();
            if (pursueTransform != null)
            {
                pursuePlayer = true;
                playerTransform = pursueTransform;
                Quaternion newDir = GetDirectionToTarget(playerTransform.position);
                SetDirection(newDir);
            }
        }
        else
        {
            pursuePlayer = false;
            Quaternion newDir = transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180f);
            //SetRandomTarget();
            //Quaternion newDir = GetDirectionToTarget(targetPosition);
            SetDirection(newDir);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Shot"))
        {
            ShotAttributes shot = col.GetComponent<ShotAttributes>();
            if (shot.getTeamID() != teamID)
            {
                TakeDamage(shot.damage);
                Destroy(col.gameObject);
            }
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        blinkAnimation.startAnimation();
        if(currentHealth <= 0)
        {
            cam.GetComponent<CameraController>().CamShake(0.2f, 0.15f);
            Destroy(gameObject);
        }
    }
}
