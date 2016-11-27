using UnityEngine;
using UnityEngine.UI;

public class ControlMP : MonoBehaviour
{
    public MotionPhysicsMP motionPhysics;
    public GameObject barrel;
    public GameObject startPos;
    public GameObject playerSphere;
    public Vector3 fireVector;
    public Vector3 velocity;
    public Text velocityLabel;
    public Text angleLabel;
    public Text medalLabel;

    float maxVelocity;
    float minVelocity;
    float barrelAngle;
    float startVelocity;
    float barrelIncrement;
    float velocityIncrement;

    void Start()
    {
        maxVelocity = 100;
        minVelocity = 40;
        barrelAngle = 0;
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

    void Update()
    {
        if (!motionPhysics.fired)
        {
            playerSphere.transform.position = startPos.transform.position;
        }

        //Kontrollering av begge kanonene

        ////////////////////1 Player (BLUE)///////////////////////////////////

        if (Input.GetKey(KeyCode.UpArrow) && barrel.tag == "Player1") //Øk vinkelen på kanonen
        {
            if ((barrelAngle <= 44))
            {
                barrelAngle += barrelIncrement;
                barrel.transform.Rotate(0, 0, barrelIncrement);
                angleLabel.text = barrelAngle.ToString("0") + " Degrees";
            }
        }

        if (Input.GetKey(KeyCode.DownArrow) && barrel.tag == "Player1") //Reduser vinkelen på kanonen
        {
            if ((barrelAngle >= -44))
            {
                barrelAngle -= barrelIncrement;
                barrel.transform.Rotate(0, 0, -barrelIncrement);
                angleLabel.text = barrelAngle.ToString("0") + " Degrees";
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) && barrel.tag == "Player1") //Reduser start hastigheten
        {
            if (startVelocity > minVelocity)
            {
                startVelocity -= velocityIncrement;
                velocityLabel.text = startVelocity.ToString("0") + " m/s";
            }
        }

        if (Input.GetKey(KeyCode.RightArrow) && barrel.tag == "Player1") //Øk start hastigheten
        {
            if (startVelocity < maxVelocity)
            {
                startVelocity += velocityIncrement;
                velocityLabel.text = startVelocity.ToString("0") + " m/s";
            }
        }

        ////////////////////2 Player (RED)///////////////////////////////////

        if (Input.GetKey(KeyCode.W) && barrel.tag == "Player2") //Øk vinkelen på kanonen
        {
            if ((barrelAngle <= 44))
            {
                barrelAngle += barrelIncrement;
                barrel.transform.Rotate(0, 0, barrelIncrement);
                angleLabel.text = barrelAngle.ToString("0") + " Degrees";
            }
        }

        if (Input.GetKey(KeyCode.S) && barrel.tag == "Player2") //Reduser vinkelen på kanonen
        {
            if ((barrelAngle >= -44))
            {
                barrelAngle -= barrelIncrement;
                barrel.transform.Rotate(0, 0, -barrelIncrement);
                angleLabel.text = barrelAngle.ToString("0") + " Degrees";
            }
        }

        if (Input.GetKey(KeyCode.A) && barrel.tag == "Player2") //Reduser start hastigheten
        {
            if (startVelocity > minVelocity)
            {
                startVelocity -= velocityIncrement;
                velocityLabel.text = startVelocity.ToString("0") + " m/s";
            }
        }

        if (Input.GetKey(KeyCode.D) && barrel.tag == "Player2") //Øk start hastigheten
        {
            if (startVelocity < maxVelocity)
            {
                startVelocity += velocityIncrement;
                velocityLabel.text = startVelocity.ToString("0") + " m/s";
            }
        }

        //////////////////////////////////////////////////////////////////

        if (Input.GetKey(KeyCode.Space)) //Avfyr kulene!
        {
            if (motionPhysics.fired != true)
            {
                fireVector = startPos.transform.position - barrel.transform.position;
                velocity = fireVector.normalized * startVelocity;
                motionPhysics.setVelocity(velocity);
                motionPhysics.Fire();
            }
        }

        if (Input.GetKey(KeyCode.Return)) //Reset begge
        {
            medalLabel.text = "";
            motionPhysics.fired = false;
            motionPhysics.medalGiven = false;
            playerSphere.transform.position = startPos.transform.position;
        }
    }
}
