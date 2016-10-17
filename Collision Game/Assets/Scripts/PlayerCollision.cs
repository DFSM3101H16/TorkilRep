using UnityEngine;
using System.Collections;

public class PlayerCollision : BallControl{

    public float mass1 = 1;
    public float mass2 = 2;
    public float vx;
    public float vy;
    public float angle;
    public float playerRadius = 0.5f;
    public float G = 9.81f;
    Vector3 gravVector = new Vector3(0, -9.81f, 0);
    public float yPosition;
    public float xPosition;
    public float e = 0.7f;

    public bool dropped = false;

    void Start () {
        yPosition = transform.position.y;
    }
    

    
    void Update ()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            xPosition = transform.position.x;
            dropped = true;
        }
    }

    void FixedUpdate () {


        if (dropped == true)
        {

            //FORMULA: y = y * Time.deltaTime + 0.5f * G * Time.deltaTime * Time.deltaTime
            Vector3 velocity = new Vector3(vx, vy) + gravVector * Time.fixedDeltaTime;

            Vector3 newPosition = transform.position + velocity * Time.deltaTime - 0.5f * gravVector * Time.deltaTime * Time.deltaTime;

            Vector3 directionVector = newPosition - transform.position;
            RaycastHit hit;


            if (Physics.SphereCast(transform.position, playerRadius, directionVector, out hit, directionVector.magnitude))
            {
                angle = Mathf.Deg2Rad * Vector3.Angle(directionVector, hit.normal);

                velocity = getPostVelocity(vx, vy, mass1, mass2, e, angle);
                newPosition = transform.position + velocity * Time.deltaTime - 0.5f * gravVector * Time.deltaTime * Time.deltaTime;
            }
            vx = velocity.x;
            vy = velocity.y;
            transform.position = newPosition;

        }
	}

    public Vector3 getPostVelocity(float xVelocity, float yVelocity, float mass1, float mass2, float e, float angle)
    {

        float Vp_1 = (xVelocity * Mathf.Cos(angle)) + (yVelocity * Mathf.Sin(angle));
        float Vn_1 = (-xVelocity * Mathf.Sin(angle)) + (yVelocity * Mathf.Cos(angle));

        float Vp_1After = ((mass1 - e * mass2) / (mass1 + mass2)) * Vp_1;

        //float Vn_1After = Vn_1;

        Vector3 postVelocity = new Vector3(Vp_1After * Mathf.Cos(angle) - (Vn_1) * Mathf.Sin(angle), Vp_1After * Mathf.Sin(angle) + Vn_1 * Mathf.Cos(angle), 0);

        return postVelocity;
    }


}
