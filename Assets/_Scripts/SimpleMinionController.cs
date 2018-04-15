using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMinionController : MonoBehaviour {

    public float speed;
    public float timeLimit = 1.5f;
    public bool cooperate;
    //public enum Dimension { both, x, y };
    //public Dimension wanderDimension;
    //public float x_range;
    //public float y_range;

    private Rigidbody2D rb;
    private RaycastHit2D hit;
    private float timer;
    private bool pursuePlayer;
    private Vector3 playerPosition;
    //private Vector3 targetPosition;


    // Use this for initialization
    void Start () {

        rb = transform.GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        timer = timeLimit;
        pursuePlayer = false;
        //SetRandomTarget();
	}

    private void Update()
    {
    }

    public Vector3 GetPlayerPosition()
    {
        if (pursuePlayer)
        {
            return playerPosition;
        }
        else
        {
            return Vector3.positiveInfinity;
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
            playerPosition = hit.transform.position;
        }
        else if (hit.transform.CompareTag("Minion"))
        {
            if (cooperate)
            {
                Vector3 pursuePos = hit.transform.GetComponent<SimpleMinionController>().GetPlayerPosition();
                if (!pursuePos.Equals(Vector3.positiveInfinity))
                {
                    pursuePlayer = true;
                    playerPosition = pursuePos;
                }
            }
            
        }
        else
        {
            pursuePlayer = false;
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
            Quaternion newDir = GetDirectionToTarget(playerPosition);
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
            playerPosition = collision.transform.position;
            Quaternion newDir = GetDirectionToTarget(playerPosition);
            SetDirection(newDir);
        }
        else if (collision.transform.CompareTag("Minion"))
        {
            Vector3 pursuePos = collision.transform.GetComponent<SimpleMinionController>().GetPlayerPosition();
            if (!pursuePos.Equals(Vector3.positiveInfinity))
            {
                pursuePlayer = true;
                playerPosition = pursuePos;
                Quaternion newDir = GetDirectionToTarget(playerPosition);
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
}
