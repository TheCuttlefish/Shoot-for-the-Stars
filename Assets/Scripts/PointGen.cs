using System.Collections.Generic;
using UnityEngine;

public class PointGen : MonoBehaviour
{
    public float speed = 5f;
    float autoSpeed = 0.1f;
    public float maxAngle = 80f;
    public float maxStep = 20f;

    public float duplicateEvery = 10f;
    public bool canDuplicate = true;

    public float lifeDistance = 20f;
    private float cloneTravel = 20f;

    public List<Vector3> points = new List<Vector3>();
    private Vector3 _prevPos;

    private float _currentAngle = 0f;
    private Vector3 _lastDuplicatePos;

    public bool moveForward = false;   // start stopped

    // NEW: budget for how far we can move from Space input
    private float moveBudget = 0f;
        private Camera cam;

    void Start()
    {
      

    _prevPos = transform.position;
        _lastDuplicatePos = transform.position;
        cam = Camera.main;

        moveBudget = 10;// on start of the game grow a bit
        //
        //Win();
    }

    public void  BigWin()
    {
        moveBudget += 20f;     // allow 10 units of growth
        moveForward = true;    // start moving again

        if (transform.position.y < cam.transform.position.y - 10f)
        {
            Destroy(gameObject);
        }
    }


    public void SmallWin()
    {
        moveBudget += 3f;     // allow 10 units of growth
        moveForward = true;    // start moving again

        if (transform.position.y < cam.transform.position.y - 10f)
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            if (transform.position.y < cam.transform.position.y - 10f)
            {
                Destroy(gameObject);
            }

        }

        // ---- INPUT: give 10 units per Space ----
        if (Input.GetKeyDown(KeyCode.Alpha3) && canDuplicate)
        {
            moveBudget += 20f;     // allow 10 units of growth
            moveForward = true;    // start moving again
        }

        if (!moveForward) return;

        Vector3 oldPos = transform.position;

        Vector3 dir = Quaternion.Euler(0f, 0f, _currentAngle) * Vector3.up;

        // ---- MOVE ONLY AS FAR AS BUDGET ALLOWS ----
        float maxThisFrame = speed * Time.deltaTime;
        float moveThisFrame = Mathf.Min(maxThisFrame, moveBudget);

        transform.position += dir * moveThisFrame;
        transform.up = dir;
        //- no autospeed from now Jan 25th
        ///moveBudget += autoSpeed * Time.deltaTime;// -- this is auto speed!!
        moveBudget -= moveThisFrame;

        if (moveBudget <= 0f)
        {
            moveForward = false; // stop until next Space press
        }

        float movedThisFrame = moveThisFrame;

        // ---- CLONE LIFETIME ----
        if (!canDuplicate)
        {
            cloneTravel += movedThisFrame;
            if (cloneTravel >= lifeDistance)
            {
                moveForward = false;
                return;
            }
        }

        // ---- DUPLICATION ----
        if (canDuplicate)
        {
            float distSinceDup = Vector3.Distance(transform.position, _lastDuplicatePos);

            if (distSinceDup >= duplicateEvery)
            {
                GameObject cloneObj = Instantiate(gameObject, transform.position, transform.rotation);
                PointGen pg = cloneObj.GetComponent<PointGen>();

                pg.canDuplicate = false;
                pg.lifeDistance = Random.Range(2, 20); // not sure this is doing anything (same with all the lines below)
                pg.cloneTravel = 0;
                pg.moveBudget = 20;       // clones don't use Space input
                pg.moveForward = true;    // clones auto-grow normally

                _lastDuplicatePos = transform.position;
            }
        }

        // ---- POINT SPAWNING ----
        if (Vector2.Distance(transform.position, _prevPos) > 0.6f)
        {
            points.Insert(0, transform.position);
            if (points.Count > 41)
                points.RemoveAt(points.Count - 1);

            _prevPos = transform.position;

            float step = Random.Range(-maxStep, maxStep);
            _currentAngle += step;
            _currentAngle = Mathf.Clamp(_currentAngle, -maxAngle, maxAngle);
        }
    }
}
