using UnityEngine;

public class MotionPhysics : MonoBehaviour
{
    Medal medal;
    GameObject[] medals;
    public Control control;     
    public AudioSource audio_;

    public float e;            //Coefficient of restitution, 0 = uelastisk, 1 = elastisk
    public float rho;          //Massetettheten til luft, [rho] = kg/m^3  (høyde 0 moh)
    public float mass;         //Massen til spiller kulen [mass] = kg
    public float area;         //Arealet til spiller kulen
    public float angle;        //Vinkelen mellom spilleren og kollisjons objektet
    public float dragCo;       //Drag koeffisienten (for en sphere)
    public float massOther;    //Kollisjons objektet sin masse
    public float playerRadius; //Radiusen til spiller kulen
    public bool fired;         //Boolean for å sjekke om man har skutt kanonen
    public bool medalGiven;    //Boolean for å sjekke om medaljeteksten har blitt gitt til spilleren
    public Vector3 velocity;   //Vector farten i m/s
    public DragProjectile DragProjectile; //Kalkulasjon med drag

    void Start()
    {        
        e            = 0.8f;
        rho          = 1.225f;
        mass         = 1;
        area         = 0.5f; //Gjort mindre for å få mindre drag = mer morsomt
        angle        = 0;
        dragCo       = 0.4f;
        massOther    = 99999999;
        playerRadius = 0.25f;
        fired        = false;
        medalGiven   = false;
        medals       = GameObject.FindGameObjectsWithTag("MedalTag");

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
        DragProjectile = new DragProjectile(transform.position, velocity, Time.fixedDeltaTime, mass, area, rho, dragCo);
    }

    void FixedUpdate()
    {
        if (!tooSlow())
        {
            DragProjectile = new DragProjectile(transform.position, velocity, Time.fixedDeltaTime, mass, area, rho, dragCo);
            DragProjectile.updateLocationAndVelocity(Time.fixedDeltaTime);

            if (fired == true)
            {
                Vector3 directionVector = DragProjectile.getPosition() - transform.position;

                RaycastHit hit;
               
                if (Physics.SphereCast(transform.position, //Kollisjonssjekk ved hjelp av spherecast
                    playerRadius, 
                    directionVector, 
                    out hit, 
                    directionVector.magnitude) 
                    && hit.collider.tag != "MedalTag") //Kolliderer ikke med "usynilige" triggerbokser som ligger i scenen (Se Dokumentasjon)
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red, 1, false);

                    audio_.Play();

                    //Kalkulerer vinkelen mellom +X retningen til kula og normalen til kollisjonsobjectet, gjort om til radianer
                    angle = Mathf.Deg2Rad * Vector3.Angle(Vector3.right, hit.normal);

                    //Nye hastigheten (etter kollisjon) blir returnert fra kollisjons funksjonen
                    velocity = getPostVelocity(velocity.x, velocity.y, mass, massOther, e, angle);

                    //Oppdaterer posisjonen med den nye hastigheten og retningen etter kollisjonen
                    transform.position += velocity * Time.fixedDeltaTime;
                }

                else //Hvis ikke det skjer en kollisjon:
                {
                    //Fart og posisjon blir oppdatert som vanlig fra DragProjectile + ODE kalkulasjonene
                    velocity = DragProjectile.getVelocity();
                    transform.position = DragProjectile.getPosition();
                }
            }
            Debug.DrawLine(transform.position, transform.position + velocity, Color.green, 0.05f, false);

        } //Når kulen stopper opp blir giveMedal() funksjonen kjørt => medalje teksten blir opdatert
        if (tooSlow())
        {
            giveMedal();
        }
    }

    //Returnerer vector hastigheten etter kollisjon med stillestående objekt
    public Vector3 getPostVelocity(float xVelocity, float yVelocity, float mass, float massOther, float e, float angle)
    {
        float Vp_1 = (xVelocity * Mathf.Cos(angle)) + (yVelocity * Mathf.Sin(angle));  //FORMULA: vp = vx * cos(angle) + vy * sin(angle)
        float Vn_1 = (-xVelocity * Mathf.Sin(angle)) + (yVelocity * Mathf.Cos(angle)); //FORMULA: vn = -vx * sin(angle) + vy * cos(angle)

        //FORMULA: velocityAfter = ((m1 - e*m2) / (m1 + m2)) * v
        float Vp_1After = ((mass - e * massOther) / (mass + massOther)) * Vp_1;

        //FORMULA: vxAfter = vpAfter * cos(angle) - vn * sin(angle) , vyAfter = vpAfter * sin(angle) + vn * cos(angle)
        Vector3 postVelocity = new Vector3(Vp_1After * Mathf.Cos(angle) - (Vn_1) * Mathf.Sin(angle), 
                                           Vp_1After * Mathf.Sin(angle) + Vn_1 * Mathf.Cos(angle), 0);

        return postVelocity;
    }

    public bool tooSlow()
    {
        //Hvis kulen beveger seg for sakte på visse overflater, returneres true = kulen stopper
        //Dette er for å:
        //* Lettere kontrollere når lyd skal spilles av
        //* Ungå eventuelle kollisjonsfeil
        //* Få ting til å gå litt fortere
        if (velocity.magnitude < 1)
        {
            if (transform.position.y < 0.78 && transform.position.y > 0.74f || transform.position.y < -7.5f && transform.position.y > -7.65f)
            {
                return true;
            }
            else return false;
        }
        else return false;
    }

    public string giveMedal()
    {
        //Går igjennom alle plassene kulen kan lande
        //I Medal scriptet blir det returnert navnet på objektet der kulen sist "triggered"
        //Medalje teksten i UIen blir deretter oppdatert utifra dette

        int i = 0;
        while (!medalGiven)
        {
            medal = medals[i].GetComponent<Medal>();

            if (medal.giveMedal() == "GrassTrigger")
            {
                control.medalLabel.text = "Epic Fail";
                medalGiven = true;
            }
            else if (medal.giveMedal() == "BronzeTriggerLeft" || medal.giveMedal() == "BronzeTriggerRight")
            {
                control.medalLabel.text = "Bronze medal...";
                medalGiven = true;
            }
            else if (medal.giveMedal() == "SilverTriggerLeft" || medal.giveMedal() == "SilverTriggerRight")
            {
                control.medalLabel.text = "Silver medal!";
                medalGiven = true;
            }
            else if (medal.giveMedal() == "GoldTrigger")
            {
                control.medalLabel.text = "GOLD MEDAL!!";
                medalGiven = true;
            }
            i++;
        }
        return "Something went wrong";
    }    
}