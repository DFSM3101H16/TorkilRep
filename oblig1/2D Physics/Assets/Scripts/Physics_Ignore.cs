using UnityEngine;
using System.Collections;

public class Physics : MonoBehaviour {



    Collider2D myCollider;

    public Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 gravity;
    public Vector3 force;
    public float mass = 2f;
    public Vector3 drag;
    public Vector3 windForce;

    // Use this for initialization
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        acceleration = new Vector3(0, 0, 0);
        velocity = new Vector3(0, 0, 0);
        gravity = new Vector3(0, -9.81f, 0);
        force = new Vector3(0, 0, 0);
        drag = new Vector3(0, 0, 0);
        windForce = new Vector3(2.5f, 0, 0);



    }

    // Update is called once per frame
    void Update()
    {
        //force = gravity * mass;
        updateDrag();
        updateAcceleration();
        updateVelocity();

        updatePosition();

       // updateCollision();

        if (Input.GetKeyDown("space"))
        {
            gravity *= -1;
        }
        if (Input.GetKey("a"))
        {
            force = new Vector3(-10, force.y, force.z);
        }
        if (Input.GetKey("d"))
        {
            force = new Vector3(10, force.y, force.z);
        }


    }

    //void updateCollision()
    //{
    //    if (transform.position.y < 0)
    //    {
    //        transform.position = new Vector3(0, 0, 0);
    //        velocity = velocity * 0;
    //    }
    //}

    void updateDrag()
    {
        drag = new Vector3(-Mathf.Sign(velocity.x) * ((1.05f * Mathf.Pow(velocity.x, 2)) / 2),
                  -Mathf.Sign(velocity.y) * ((1.05f * Mathf.Pow(velocity.y, 2)) / 2), 0);
    }

    void updateAcceleration()
    {
        acceleration = (((gravity * mass) + drag + force) / mass);
        force *= 0;
    }

    void updateVelocity()
    {
        velocity += (acceleration) * Time.deltaTime;
    }

    void updatePosition()
    {
        transform.position += (velocity * Time.deltaTime);
    }

    void checkCollision()
    {

        //collider.
    }
}
