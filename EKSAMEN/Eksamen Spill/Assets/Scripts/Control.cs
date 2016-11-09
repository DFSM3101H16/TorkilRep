using UnityEngine;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    public MotionPhysics motionPhysics;
    public GameObject barrel;
    public GameObject startPos;
    public GameObject playerSphere;
    public Vector3 fireVector;
    public Vector3 velocity;
    //public TrailRenderer tr;
    Text velocityLabel;
    Text angleLabel;
    public float barrelAngle;
    public float maxVelocity;
    public float minVelocity;
    public float startVelocity;

    void Start () {
        barrel = GameObject.Find("Cannon Barrel");
        startPos = GameObject.Find("StartPos");
        playerSphere = GameObject.Find("PlayerSphere");
        velocityLabel = GameObject.Find("Text_VelocityVal").GetComponent<Text>();
        angleLabel = GameObject.Find("Text_AngleVal").GetComponent<Text>();
        motionPhysics = GameObject.Find("PlayerSphere").GetComponent<MotionPhysics>();

        barrelAngle = 45;
        maxVelocity = 80;
        minVelocity = 10;
        startVelocity = 50;
        velocityLabel.text = startVelocity.ToString("0");
    }

    public Vector3 getVelocity()
    {
        return velocity;
    }

    void Update () {
        
        if (!motionPhysics.fired)
        {
            playerSphere.transform.position = startPos.transform.position;
        }
        
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) //Øk vinkelen på kanonen (opp mot 90gr)
        {         
            if ((barrelAngle <= 89))
            {
                barrelAngle++;
                barrel.transform.Rotate(0, 0, 1);
                angleLabel.text = barrelAngle.ToString();
            }           
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) //Reduser vinkelen på kanonen (ned mot 0gr)
        {            
            if ((barrelAngle >= 1))
            {
                barrelAngle--;
                barrel.transform.Rotate(0, 0, -1);
                angleLabel.text = barrelAngle.ToString();
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) //Reduser start hastigheten
        {
            if (startVelocity > minVelocity)
            {
                startVelocity -= 0.1f;
                velocityLabel.text = startVelocity.ToString("0");
            }
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) //Øk start hastigheten
        {
            if (startVelocity < maxVelocity)
            {
                startVelocity += 0.1f;
                velocityLabel.text = startVelocity.ToString("0");
            }
        }

        if (Input.GetKey(KeyCode.Space)) //Avfyr kulen!
        {
            if (motionPhysics.fired != true)
            {
                fireVector = startPos.transform.position - barrel.transform.position;
                velocity = fireVector.normalized * startVelocity;
                motionPhysics.setVelocity(velocity);
                motionPhysics.Fire();
                //Debug.Log("Vel in Control = " + velocity);
            }
        }

        if (Input.GetKey(KeyCode.Return)) //Reset
        {
            motionPhysics.fired = false;
            playerSphere.transform.position = startPos.transform.position;
        }
    }
}
