using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {

    public Vector3 windForce;
    public float windForceX;
    Physics PlayerBoxPhys;


    void start()
    {
        windForce = new Vector3(-100f, 0, 0);
        windForceX = -100f;
        PlayerBoxPhys = GameObject.Find("PlayerBox").GetComponent<Physics>();
    }

    void updateWindForce()
    {
        PlayerBoxPhys.force += windForce;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLISION!");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("PLAYER COLLISION!");
            other.gameObject.GetComponent<Rigidbody>().AddForce(windForceX, 0, 0);
            //other.gameObject.GetComponent<Rigidbody>().MovePosition(0, 0, 0);
            //updateWindForce();
        }
            
    }



}
