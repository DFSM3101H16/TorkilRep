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
    public Text velocityLabel;
    public Text angleLabel;
    public Text medalLabel;

    float barrelAngle;
    float maxVelocity;
    float minVelocity;
    float startVelocity;
    float barrelIncrement;
    float velocityIncrement;

    void Start () {

        barrelAngle = 45;
        maxVelocity = 100;
        minVelocity = 40;
        startVelocity = 60;
        barrelIncrement = 0.5f;
        velocityIncrement = 0.2f;
        medalLabel.text = "";
        angleLabel.text = barrelAngle.ToString("0") + " Degrees";
        velocityLabel.text = startVelocity.ToString("0") + " m/s";
    }

    public Vector3 getVelocity()
    {
        return velocity;
    }

    void Update () {
        
        if (!motionPhysics.fired) //Hvis ikke avfyrt, endres kule posisjonen til hvor kanonløpet går
        {
            playerSphere.transform.position = startPos.transform.position;
        }
        
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) //Øk vinkelen på kanonen (opp mot 90gr)
        {         
            if (barrelAngle <= 89)
            {
                barrelAngle += barrelIncrement;
                barrel.transform.Rotate(0, 0, barrelIncrement);
                angleLabel.text = barrelAngle.ToString("0") + " Degrees";
            }           
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) //Reduser vinkelen på kanonen (ned mot 0gr)
        {            
            if (barrelAngle >= 1)
            {
                barrelAngle -= barrelIncrement;
                barrel.transform.Rotate(0, 0, -barrelIncrement);
                angleLabel.text = barrelAngle.ToString("0") + " Degrees";
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) //Reduser start hastigheten
        {
            if (startVelocity > minVelocity)
            {
                startVelocity -= velocityIncrement;
                velocityLabel.text = startVelocity.ToString("0") + " m/s";
            }
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) //Øk start hastigheten
        {
            if (startVelocity < maxVelocity)
            {
                startVelocity += velocityIncrement;
                velocityLabel.text = startVelocity.ToString("0") + " m/s";
            }
        }

        if (Input.GetKey(KeyCode.Space)) //Avfyr kulen!
        {
            if (motionPhysics.fired != true)
            {
                fireVector = startPos.transform.position - barrel.transform.position; //Skyte retningen
                velocity = fireVector.normalized * startVelocity; //Hastigheten omgjort til vector
                motionPhysics.setVelocity(velocity);
                motionPhysics.Fire();
            }
        }

        if (Input.GetKey(KeyCode.Return)) //Reset
        {
            medalLabel.text = "";
            motionPhysics.medalGiven = false;
            motionPhysics.fired = false;
            playerSphere.transform.position = startPos.transform.position;
        }
    }
}
