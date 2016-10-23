using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour{

    public float mass1 = 1;             //Player mass
    public float mass2 = 2;             //Collision object's mass
    public float vx;                    //x direction velocity
    public float vy;                    //y direction velocity
    public float angle;                 //Angle variable
    public float playerRadius = 0.5f;   //Radius of player ball
    public float yPosition;             //Position on x-axis
    public float xPosition;             //Position on y-axis
    public float e = 0.7f;              //Coefficient of restitution

    Vector3 gravVector = new Vector3(0, -9.81f, 0); //Gravity vector

    AudioSource audio;                              //Audio object
    public bool dropped;                            //Boolean for when the ball is dropped

    void Start () {
        audio = GetComponent<AudioSource>();
        yPosition = transform.position.y;
        dropped = false;
    }

    public void Reset() // Resets the variables needed
    {
        dropped = false;
        vx = 0;
        vy = 0;
        angle = 0;
        yPosition = 8.97f;
        transform.position = new Vector3(transform.position.x, yPosition, 0);
    }
    
    void Update () //Control of the ball at the start 
    {
        if (dropped == false)
        {
            float xPlus = 0;
            if (Input.GetKey(KeyCode.D) && transform.position.x < 8.9f)
            {
                xPlus = 0.1f;
            }
            transform.Translate(xPlus, 0, 0);

            float xMinus = 0;

            if (Input.GetKey(KeyCode.A) && transform.position.x > -8.9f)
            {
                xMinus = -0.1f;
            }
            transform.Translate(xMinus, 0, 0);
        }

        if (Input.GetKey(KeyCode.Space))
        {         
            dropped = true;
        }
    }

    void FixedUpdate () { //Collition detection, velocity and position manipulation

        if (dropped == true) //The fixed update code will run after the ball is dropped
        {
            //FORMULA: vf = v0 + a * t
            Vector3 velocity = new Vector3(vx, vy) + gravVector * Time.fixedDeltaTime;

            //FORMULA: x(t) = x0 + v0 * t + 1/2 * a * t^2
            Vector3 newPosition = transform.position + velocity * Time.deltaTime - 0.5f * gravVector * Time.deltaTime * Time.deltaTime;

            Vector3 directionVector = newPosition - transform.position;
            RaycastHit hit;

            //Checks for collision with raycast from the player ball (sphere)
            if (Physics.SphereCast(transform.position, playerRadius, directionVector, out hit, directionVector.magnitude))
            {
                if (transform.position.y > -7) //To not create constant sound when on the bottom of the level
                {
                    audio.Play(); //Plays a neat audio clip on collision
                }
                //Angle between players right vector and collision object's normal, then converted to radians
                angle = Mathf.Deg2Rad * Vector3.Angle(Vector3.right, hit.normal);

                velocity = getPostVelocity(vx, vy, mass1, mass2, e, angle); //Returns the velocity after collision

                //FORMULA: x(t) = x0 + v0 * t + 1/2 * a * t^2
                newPosition = transform.position + velocity * Time.deltaTime - 0.5f * gravVector * Time.deltaTime * Time.deltaTime;
            }
            vx = velocity.x;
            vy = velocity.y;
            transform.position = newPosition;
        }
	}

    public Vector3 getPostVelocity(float xVelocity, float yVelocity, float mass1, float mass2, float e, float angle) //The physics!
    {
        float Vp_1 = (xVelocity * Mathf.Cos(angle)) + (yVelocity * Mathf.Sin(angle));  //FORMULA: vp = vx * cos(angle) + vy * sin(angle)
        float Vn_1 = (-xVelocity * Mathf.Sin(angle)) + (yVelocity * Mathf.Cos(angle)); //FORMULA: vn = -vx * sin(angle) + vy * cos(angle)

        //FORMULA: velocityAfter = ((m1 - e*m2) / (m1 + m2)) * v --- Second part of equation dropped because its 0
        float Vp_1After = ((mass1 - e * mass2) / (mass1 + mass2)) * Vp_1;

        //FORMULA: vxAfter = vpAfter * cos(angle) - vn * sin(angle)
        //AND vyAfter = vpAfter * sin(angle) + vn * cos(angle)
        Vector3 postVelocity = new Vector3(Vp_1After * Mathf.Cos(angle) - (Vn_1) * Mathf.Sin(angle), Vp_1After * Mathf.Sin(angle) + Vn_1 * Mathf.Cos(angle), 0);

        return postVelocity;
    }
}
