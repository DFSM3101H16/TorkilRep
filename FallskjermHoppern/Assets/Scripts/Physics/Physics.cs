using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Physics : MonoBehaviour
{
    //--Fysikk variabler--//
    public float rho = 1.225f;                      //Massetettheten til luft, [rho] = kg/m^3 
    public float G = -9.81f;                        //Tyngdens akselerasjon, [G] = m/s^2 
    public float area = 0.5f;                      //Arealet til det som faller. Later som fallskjermhopperen er en sirkel (0.5m sirkulært tverrsnitt)
    public float dragCo = 0.47f;                    //Drag koeffisienten (for en sphere)
    public float mass;                              //Massen til det som faller [mass] = kg
    public Vector3 velocity = new Vector3(0, 0, 0); //Farten i m/s
    public DragProjectile DragProjectile;           //Kalkulasjon med drag

    public float height = 200;
    public float time;

    Text timerLabel;
    Text conditionLabel;
    Text velocityLabel;
    Text massText;

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
        if (jumped == false)
        {
            mass = float.Parse(massText.text);
            jumped = true;
            DragProjectile = new DragProjectile(transform.position, velocity, Time.time, mass, area, rho, dragCo);
            Time.timeScale = 1;
        }
    }

    public void Parachute()
    {
        if (jumped == true)
        {
            DragProjectile.area = 20f; //Utløser fallskjermen => aeralet øker kraftig = større drag
            DragProjectile.dragCo = 1.4f;
        }

    }

    public void Reset()
    {
        //Tilbakestiller alle viktige faktorer til orginale verdier
        transform.position = new Vector3(0, 0, 0);
        dragCo = 0.47f;
        area = 0.5f;
        velocity.y = 0f;
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

        //Sjekker hva farten er når man treffer bakken, og setter condition deretter
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
        if (DragProjectile != null)
        {
            DragProjectile.updateLocationAndVelocity(Time.fixedDeltaTime);

            transform.position = DragProjectile.getPosition() * 7.87f / height;

            velocity = DragProjectile.getVelocity();

            //Sjekker om fallskjermhopperen har truffet bakken
            if (DragProjectile.getY() <= -height)
            {
                OnGround(); //Hvis ja, kalles truffet bakke funksjonen
            }
        }
    }
}