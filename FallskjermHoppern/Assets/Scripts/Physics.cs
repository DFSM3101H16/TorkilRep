using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Physics : MonoBehaviour {
                                            //--Fysikk variabler--//
    public float rho = 1.225f;              //Massetettheten til luft, [rho] = kg/m^3 
    public float G = -9.81f;                //Tyngdens akselerasjon, [G] = m/s^2 
    public float area = Mathf.Pow(0.5f, 2); //Arealet til det som faller. Later som fallskjermhopperen er en sirkel (0.5m sirkulært tverrsnitt)
    public float dragCo = 0.47f;            //Drag koeffisienten (for en sphere)
    public float mass;                      //Massen til det som faller [mass] = kg
    public float velocity;                  //Farten i m/s
    public float startHeight = 200f;        //Høyden fra føyet til bakken [m] = meter

    public float time;
    float seconds;
    public float yPosition = 0f;

    Text timerLabel;
    Text conditionLabel;
    Text velocityLabel;
    Text massText;

    public bool updateStarted = false;
    public bool hitGround = false;
    public bool jumped = false;
    public string condition;

    public Physics(float _mass, float _area, float _startHeight)
    {

    }


    void Start () {
        //Henter ressurser fra UI'en m.m.
        timerLabel =        GameObject.Find("Text_Seconds").GetComponent<Text>();
        conditionLabel =    GameObject.Find("Text_Condition").GetComponent<Text>();
        velocityLabel =     GameObject.Find("Text_Velocity").GetComponent<Text>();
        massText =          GameObject.Find("InputField_Mass").GetComponentInChildren<Text>();
        
        transform.position = new Vector3(0, 0, 0);
    }

    public void Jump ()
    {
        yPosition = transform.position.y;
        mass = float.Parse(massText.text);
        jumped = true;
        updateStarted = true; //Setter i gang update funksjonen
    }

    public void Parachute ()
    {
        if (jumped == true)
        {
            area = Mathf.Pow(20f, 2); //Utløser fallskjermen => aeralet øker kraftig = større drag
        }
        
    }

    public void Reset ()
    {
        //Tilbakestiller alle viktige faktorer til orginale verdier
        transform.position = new Vector3(0, 0, 0);
        velocity = 0f;
        area = Mathf.Pow(0.5f, 2);
        hitGround = false;
        updateStarted = false;
        jumped = false;
        timerLabel.text = "0.0";
        velocityLabel.text = "0.0";
        conditionLabel.text = "Condition: ";
        time = 0f;

    }

    void OnGround ()
    {
        velocity = 0;
        updateStarted = false;
        velocityLabel.text = "0.0";

        if (velocity >= 16f)
        {
            condition = "Dead";
        }
        else if (velocity >= 7 && velocity < 16f)
        {
            condition = "Damaged";
        }
        else if (velocity < 7)
        {
            condition = "Undamaged";
        }
        conditionLabel.text = "Condition: " + condition;
    }

	void FixedUpdate () {

        if (updateStarted == true)
        {
            //Timer kode
            time += Time.deltaTime;
            seconds = time % 60;
            timerLabel.text = seconds.ToString("0.0");

            //Velocity display
            velocityLabel.text = velocity.ToString("0.0");

            //Formel: s = s0 + vt + 0.5 * a * t^2       --Oppdatering av posisjonen--
            //yPosition = yPosition + velocity * Time.fixedDeltaTime - 0.5f * G * (Time.fixedDeltaTime * Time.fixedDeltaTime);

            transform.position = new Vector3(0, yPosition, 0);


            //a) F_D = 1/2 \rho A * C_D * v^2   algebraisk => Drag

            //b) dv/dt = a = Sum F / m          diff       => Acceleration

            //c) Sum F = F_D - mg               algebraisk => Sum av kreftene

            //d) dy/dt = v                      diff       => Hastigheten



            //Sjekker om fallskjermhopperen har truffet bakken
            if (transform.position.y <= -7.5f)
            {
                hitGround = true;
            }
            if (hitGround == true)
            {
                OnGround(); //Hvis ja, kalles truffet bakke funksjonen
            }
        }
	}
}