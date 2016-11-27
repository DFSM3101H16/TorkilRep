using UnityEngine;
using System.Collections;

public class Medal : MonoBehaviour {

    string medalName;
    string medalNameP1;
    string medalNameP2;
    public GameObject player1;
    public GameObject player2;

    void OnTriggerEnter(Collider other) //Når en kule entrer en medaljeboks blir medaljenavnet satt
    {
        //Sjekker hvilken kule som triggerer, setter medaljen utifra det

        if (other.gameObject == player1)
        {
            medalNameP1 = this.gameObject.name;
        }
        else if (other.gameObject == player2)
        {
            medalNameP2 = this.gameObject.name;
        }
        else
        {
            medalName = this.gameObject.name;
        }
    }

    void OnTriggerExit(Collider other) //Når kulen går ut blir medaljenavnet resetta
    {
        if (other.gameObject == player1)
        {
            medalNameP1 = "null";
        }
        else if (other.gameObject == player2)
        {
            medalNameP2 = "null";
        }
        else
        {
            medalName = "null";
        }
    }

    public string giveMedal()
    {
        return medalName;
    }

    public string giveMedalP1()
    {
        return medalNameP1;
    }

    public string giveMedalP2()
    {
        return medalNameP2;
    }
}
