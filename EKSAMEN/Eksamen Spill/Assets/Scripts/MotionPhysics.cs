using UnityEngine;

public class MotionPhysics : MonoBehaviour
{
    public Control control;

    public float rho;          //Massetettheten til luft, [rho] = kg/m^3  (høyde 0 moh)
    public float area;         //Arealet til spiller kulen
    public float dragCo;       //Drag koeffisienten (for en sphere)
    public float e;            //Coefficient of restitution
    public float mass;         //Massen til spiller kulen [mass] = kg
    public float massOther;    //Kollisjons objektet sin masse
    public float angle;        //Vinkelen mellom spilleren og kollisjons objektet
    public float playerRadius; //Radiusen til spiller kulen
    public bool fired;         //Boolean for å sjekke om man har skutt kanonen
    public Vector3 gravVector; //Gravitajon vector [G] = m/s^2
    public Vector3 velocity;   //Farten i m/s
    public DragProjectile DragProjectile; //Kalkulasjon med drag

    public AudioSource audio;         //Audio object

    void Start()
    {
        rho          = 1.225f;
        area         = 0.5f;
        dragCo       = 0.4f;
        mass         = 1;
        massOther    = 1000;
        angle        = 0;
        playerRadius = 0.25f;
        e            = 0.7f;
        fired        = false;        
        gravVector   = new Vector3(0, -9.81f, 0);
        audio        = GetComponent<AudioSource>();
        control      = GameObject.Find("Cannon Barrel").GetComponent<Control>();

        transform.position = control.startPos.transform.position;
    }

    public void setVelocity(Vector3 vel)
    {
        velocity = vel;
    }

    public void Fire()
    {
        fired = true;
        velocity = control.getVelocity();
        DragProjectile = new DragProjectile(transform.position, velocity, Time.time, mass, area, rho, dragCo);
    }

    void FixedUpdate()
    {
        
        if (DragProjectile != null)
        {
            DragProjectile.updateLocationAndVelocity(Time.fixedDeltaTime);
            velocity = DragProjectile.getVelocity();
            transform.position = DragProjectile.getPosition();
        }

        if (fired == true)
        {
            
            Vector3 newPosition = DragProjectile.getPosition() + DragProjectile.getVelocity() * Time.fixedDeltaTime - 0.5f * gravVector * Time.fixedDeltaTime * Time.fixedDeltaTime;


            Vector3 directionVector = newPosition - transform.position;
            RaycastHit hit;

            //Checks for collision with raycast from the player ball (sphere)
            if (Physics.SphereCast(transform.position, playerRadius, directionVector, out hit, directionVector.magnitude))
            {

                audio.Play();

                //Angle between players right vector and collision object's normal, then converted to radians
                angle = Mathf.Deg2Rad * Vector3.Angle(Vector3.right, hit.normal);

                //velocity = getPostVelocity(velocity.x, velocity.y, mass, massOther, e, angle); //Returns the velocity after collision                
                velocity = getPostVelocity(DragProjectile.getVelocity().x, DragProjectile.getVelocity().y, mass, massOther, e, angle); //Returns the velocity after collision

                //FORMULA: x(t) = x0 + v0 * t + 1/2 * a * t^2
                newPosition = transform.position + velocity * Time.deltaTime - 0.5f * gravVector * Time.deltaTime * Time.deltaTime;

                DragProjectile = new DragProjectile(newPosition, velocity, Time.time, mass, area, rho, dragCo);
                
            }
        }
    }

    public Vector3 getPostVelocity(float xVelocity, float yVelocity, float mass, float massOther, float e, float angle)
    {
        float Vp_1 = (xVelocity * Mathf.Cos(angle)) + (yVelocity * Mathf.Sin(angle));  //FORMULA: vp = vx * cos(angle) + vy * sin(angle)
        float Vn_1 = (-xVelocity * Mathf.Sin(angle)) + (yVelocity * Mathf.Cos(angle)); //FORMULA: vn = -vx * sin(angle) + vy * cos(angle)

        //FORMULA: velocityAfter = ((m1 - e*m2) / (m1 + m2)) * v --- Second part of equation dropped because its 0
        float Vp_1After = ((mass - e * massOther) / (mass + massOther)) * Vp_1;

        //FORMULA: vxAfter = vpAfter * cos(angle) - vn * sin(angle)
        //AND vyAfter = vpAfter * sin(angle) + vn * cos(angle)
        Vector3 postVelocity = new Vector3(Vp_1After * Mathf.Cos(angle) - (Vn_1) * Mathf.Sin(angle), Vp_1After * Mathf.Sin(angle) + Vn_1 * Mathf.Cos(angle), 0);

        return postVelocity;
    }


}