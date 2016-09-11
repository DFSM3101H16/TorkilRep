using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrictionPhysics : MonoBehaviour {

    //Creating variables for the different attributes of the box
    public float my;
    public float mass;
    public float velocity;
    public float G = 9.81f;
    public float xPosition = -7f;

    //Boolean for when the fixedUpdate function is gonna run
    public bool updateStarted = false;

    public Text frictionText;
    public Text massText;
    public Text velocityText;

    TrailRenderer tr;

    //This function is run when the game is launched
	public void Start () {
        transform.position = new Vector3(-7, 0, 0);

        tr = GetComponent<TrailRenderer>();

        //Gets the text from the thext object and input fields from the user
        frictionText = GameObject.Find("Text - Friction Coefficient").GetComponent<Text>();
        massText = GameObject.Find("Mass Input").GetComponentInChildren<Text>();
        velocityText = GameObject.Find("Velocity Input").GetComponentInChildren<Text>();

    }

    //This function is run when the start button is pressed
    public void PressGo()
    {
        xPosition = transform.position.x;

        //The variables get the text from the user, converted to float
        my = float.Parse(frictionText.text);
        mass = float.Parse(massText.text);
        velocity = float.Parse(velocityText.text);

        updateStarted = true;
    }

    //Run when reset button is pressed
    public void PressReset()
    {
        tr.Clear(); //Clears the trail behind box (pure visual)
        transform.position = new Vector3(-7, 0, 0); 
        xPosition = -7f; //Resets the position of the box
        updateStarted = false; //Stops the fixed update function
    }

    /*
        Time.fixedDeltaTime is used instead of creating a own variable for time. It slows the game down so
        the calculations don't take a split second, and look more realistic
    */

    void FixedUpdate () {

        if (updateStarted == true)
        {
            //Formula: s = s0 + vt + 0.5 * a * t^2     --With some adjustments for the friction
            xPosition = xPosition + velocity * Time.fixedDeltaTime - 0.5f * my * G * (Time.fixedDeltaTime * Time.fixedDeltaTime);

            transform.position = new Vector3(xPosition, 0, 0); //Updates the position of the box

            //Formula: v = v0 + at     --With some adjustments for the friction
            velocity = velocity - my * G * Time.fixedDeltaTime;

            if (velocity <= 0.0f) //If the box velocity hits 0, the function will stop updating
            {
                updateStarted = false;
            }
        }
    }
}
