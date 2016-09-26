using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Physics : MonoBehaviour
{
    //--Fysikk variabler--//
    public float rho = 1.225f;              //Massetettheten til luft, [rho] = kg/m^3 
    public float G = -9.81f;                //Tyngdens akselerasjon, [G] = m/s^2 
    public float area = Mathf.Pow(0.5f, 2); //Arealet til det som faller. Later som fallskjermhopperen er en sirkel (0.5m sirkulært tverrsnitt)
    public float dragCo = 0.47f;            //Drag koeffisienten (for en sphere)
    public float mass;                      //Massen til det som faller [mass] = kg
    public float startHeight = 200f;        //Høyden fra føyet til bakken [m] = meter
    public Vector3 velocity = new Vector3(0, 0, 0);  //Farten i m/s
    public Projectile ODEprojectile;

    public float height = 500;
    public float time;
    float seconds;
    public float yPosition = 0f;

    Text timerLabel;
    Text conditionLabel;
    Text velocityLabel;
    Text massText;

    public bool updateStarted = false;
    public bool jumped = false;
    public string condition;


    void Start()
    {
        //Henter ressurser fra UI'en m.m.
        timerLabel = GameObject.Find("Text_Seconds").GetComponent<Text>();
        conditionLabel = GameObject.Find("Text_Condition").GetComponent<Text>();
        velocityLabel = GameObject.Find("Text_Velocity").GetComponent<Text>();
        massText = GameObject.Find("InputField_Mass").GetComponentInChildren<Text>();

        Reset();
    }

    public void Jump()
    {
        yPosition = transform.position.y;
        mass = float.Parse(massText.text);
        jumped = true;
        ODEprojectile = new Projectile(transform.position, velocity, Time.time);
        Time.timeScale = 1;
    }

    public void Parachute()
    {
        if (jumped == true)
        {
            area = Mathf.Pow(20f, 2); //Utløser fallskjermen => aeralet øker kraftig = større drag
        }

    }

    public void Reset()
    {
        //Tilbakestiller alle viktige faktorer til orginale verdier
        transform.position = new Vector3(0, 0, 0);
        velocity.y = 0f;
        area = Mathf.Pow(0.5f, 2);
        Time.timeScale = 0;
        jumped = false;
        timerLabel.text = "0.0";
        velocityLabel.text = "0.0";
        conditionLabel.text = "Condition: ";
        time = 0f;

    }

    void OnGround()
    {
        Time.timeScale = 0;
        velocityLabel.text = "0.0";

        if (Mathf.Abs(velocity.y) >= 16f)
        {
            condition = "Dead";
        }
        else if (Mathf.Abs(velocity.y) >= 7 && Mathf.Abs(velocity.y) < 16f)
        {
            condition = "Damaged";
        }
        else if (Mathf.Abs(velocity.y) < 7)
        {
            condition = "Undamaged";
        }
        conditionLabel.text = "Condition: " + condition;
    }

    void Update()
    {
        //Timer kode
        time += Time.deltaTime;
        timerLabel.text = Mathf.Abs(time).ToString("0.0");

        //Velocity display
        velocityLabel.text = Mathf.Abs(velocity.y).ToString("0.0");
    }

    void FixedUpdate()
    {
        if (ODEprojectile != null)
        {
            ODEprojectile.updateLocationAndVelocity(Time.fixedDeltaTime);
            transform.position = ODEprojectile.getPosition() * 7.87f / height;
            velocity = ODEprojectile.getVelocity();

            //Sjekker om fallskjermhopperen har truffet bakken
            if (ODEprojectile.getY() <= -height)
            {
                OnGround(); //Hvis ja, kalles truffet bakke funksjonen
            }
        }





        //a) F_D = 1/2 \rho A * C_D * v^2   algebraisk => Drag

        //b) dv/dt = a = Sum F / m          diff       => Acceleration

        //c) Sum F = F_D - mg               algebraisk => Sum av kreftene

        //d) dy/dt = v                      diff       => Hastigheten



        
    }
}