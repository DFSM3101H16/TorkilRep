using UnityEngine;

public class MotionPhysicsMP : MonoBehaviour
{
    public MotionPhysicsMP opponent; //Objekt for å bruke variable fra motstander kulens script
    public ControlMP control;
    public AudioSource audio_;  //Audio object
    Medal medal;
    GameObject[] medals;       //Gameobject array for "medaljoboksene" (se i inspectoren)

    public float e;            //Coefficient of restitution
    public float rho;          //Massetettheten til luft, [rho] = kg/m^3  (høyde 0 moh)
    public float mass;         //Massen til spiller kulen [mass] = kg
    public float area;         //Arealet til spiller kulen
    public float angle;        //Vinkelen mellom spilleren og kollisjons objektet
    public float dragCo;       //Drag koeffisienten (for en sphere)
    public float massOther;    //Kollisjons objektet sin masse
    public float playerRadius; //Radiusen til spiller kulen
    public bool fired;         //Boolean for å sjekke om man har skutt kanonen
    public bool medalGiven;
    public Vector3 velocity;   //Farten i m/s
    DragProjectile DragProjectile; //Kalkulasjon med drag

    void Start()
    {
        e            = 0.8f;
        rho          = 1.225f;
        mass         = 1;
        area         = 0.5f;
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

                if (Physics.SphereCast(transform.position,
                    playerRadius,
                    directionVector,
                    out hit,
                    directionVector.magnitude) &&
                    hit.collider.tag != "MedalTag")
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red, 1, false);

                    audio_.Play();

                    angle = Mathf.Deg2Rad * Vector3.Angle(Vector3.right, hit.normal);

                    //Hvis spillerne kollidere med hverandre
                    if ((hit.collider.tag == "Red" && tag == "Blue") || (hit.collider.tag == "Blue" && tag == "Red"))
                    {
                        //De nye hastighetene og retningene blir returnert fra getPostVelocityP2P()
                        //som returnerer et vector array på størrelse 2
                        //Siden begge kulene bruker samme scriptet oppdaterer de hverandre på denne måten:
                        Vector3[] newVelocity = getPostVelocityP2P(velocity.x, velocity.y, mass, e, angle);
                        velocity = newVelocity[0];
                        opponent.velocity = newVelocity[1];
                    }
                    else //Hvis det er kollisjon med et stillestående objekt (samme som i single player)
                    {
                        velocity = getPostVelocity(velocity.x, velocity.y, mass, massOther, e, angle);
                    }
                    transform.position += velocity * Time.fixedDeltaTime;
                }

                else
                {
                    velocity = DragProjectile.getVelocity();
                    transform.position = DragProjectile.getPosition();
                }
            }
            Debug.DrawLine(transform.position, transform.position + velocity, Color.green, 0.05f, false);
        }

        if (tooSlow())
        {
            giveMedal();
        }
    }

    public Vector3 getPostVelocity(float xVelocity, float yVelocity, float mass, float massOther, float e, float angle)
    {
        float Vp_1 = (xVelocity * Mathf.Cos(angle)) + (yVelocity * Mathf.Sin(angle));
        float Vn_1 = (-xVelocity * Mathf.Sin(angle)) + (yVelocity * Mathf.Cos(angle));

        float Vp_1After = ((mass - e * massOther) / (mass + massOther)) * Vp_1;

        Vector3 postVelocity = new Vector3(Vp_1After * Mathf.Cos(angle) - (Vn_1) * Mathf.Sin(angle),
                               Vp_1After * Mathf.Sin(angle) + Vn_1 * Mathf.Cos(angle), 0);

        return postVelocity;
    }

    //Funksjon for kollisjon mellom spiller kulene (Player 2 Player)
    public Vector3[] getPostVelocityP2P(float xVelocity, float yVelocity, float mass, float e, float angle)
    {
        //Skaffer nødvendig informasjon fra motstanderen
        float mass_Other = opponent.mass;
        float velX_Other = opponent.velocity.x;
        float velY_Other = opponent.velocity.y;

        float Vp_1 = (xVelocity * Mathf.Cos(angle)) + (yVelocity * Mathf.Sin(angle));  //FORMULA: vp = vx * cos(angle) + vy * sin(angle)
        float Vn_1 = (-xVelocity * Mathf.Sin(angle)) + (yVelocity * Mathf.Cos(angle)); //FORMULA: vn = -vx * sin(angle) + vy * cos(angle)

        float Vp_2 = (velX_Other * Mathf.Cos(angle)) + (velY_Other * Mathf.Sin(angle));  //FORMULA: vp = vx * cos(angle) + vy * sin(angle)
        float Vn_2 = (-velX_Other * Mathf.Sin(angle)) + (velY_Other * Mathf.Cos(angle)); //FORMULA: vn = -vx * sin(angle) + vy * cos(angle)

        //Nye hastigheten for spiller 1 etter kollisjonen (formel i dok)
        float Vp_1After = ((mass - e * massOther) / (mass + massOther)) * Vp_1 + (((1 + e) * massOther) / ((mass + massOther))) * Vp_2;

        //Nye hastigheten for spiller 2 etter kollisjonen (formel i dok)
        float Vp_2After = (((1 + e) * mass) / (mass + mass_Other)) * Vp_1 + (mass_Other - (e * mass) / (mass + mass_Other)) * Vp_2;

        //Ny hastighet og retning for begge kulene
        Vector3 postVelocity1 = new Vector3(Vp_1After * Mathf.Cos(angle) - (Vn_1) * Mathf.Sin(angle),
                                            Vp_1After * Mathf.Sin(angle) + Vn_1 * Mathf.Cos(angle), 0);
        Vector3 postVelocity2 = new Vector3(Vp_2After * Mathf.Cos(angle) - (Vn_2) * Mathf.Sin(angle),
                                            Vp_2After * Mathf.Sin(angle) + Vn_2 * Mathf.Cos(angle), 0);

        return new Vector3[] { postVelocity1, postVelocity2 }; //Disse blir returnert og brukt i FixedUpdate() i spherecast
    }

    public bool tooSlow()
    {
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
        int i = 0;
        while (!medalGiven)
        {
            medal = medals[i].GetComponent<Medal>();

            if (tag == "Blue") //Medalje valg for spiller 1 (blå)
            {
                if (medal.giveMedalP1() == "GrassTrigger")
                {
                    control.medalLabel.text = "Epic Fail";
                    medalGiven = true;
                }
                else if (medal.giveMedalP1() == "BronzeTriggerLeft" || medal.giveMedalP1() == "BronzeTriggerRight")
                {
                    control.medalLabel.text = "Bronze medal...";
                    medalGiven = true;
                }
                else if (medal.giveMedalP1() == "SilverTriggerLeft" || medal.giveMedalP1() == "SilverTriggerRight")
                {
                    control.medalLabel.text = "Silver medal!";
                    medalGiven = true;
                }
                else if (medal.giveMedalP1() == "GoldTrigger")
                {
                    control.medalLabel.text = "GOLD MEDAL!!";
                    medalGiven = true;
                }
            }

            else if (tag == "Red") //Medalje valg for spiller 2 (rød)
            {
                if (medal.giveMedalP2() == "GrassTrigger")
                {
                    control.medalLabel.text = "Epic Fail";
                    medalGiven = true;
                }
                else if (medal.giveMedalP2() == "BronzeTriggerLeft" || medal.giveMedalP2() == "BronzeTriggerRight")
                {
                    control.medalLabel.text = "Bronze medal...";
                    medalGiven = true;
                }
                else if (medal.giveMedalP2() == "SilverTriggerLeft" || medal.giveMedalP2() == "SilverTriggerRight")
                {
                    control.medalLabel.text = "Silver medal!";
                    medalGiven = true;
                }
                else if (medal.giveMedalP2() == "GoldTrigger")
                {
                    control.medalLabel.text = "GOLD MEDAL!!";
                    medalGiven = true;
                }
            }
            i++;
        }
        return "Something went wrong...";
    }
}