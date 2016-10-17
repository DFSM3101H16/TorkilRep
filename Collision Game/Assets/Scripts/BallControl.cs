using UnityEngine;
using System.Collections;

public class BallControl : MonoBehaviour {

    //public static bool dropped = false;

    void Start () {
	}
	
	void Update () {

        float xPlus = 0;
        if (Input.GetKey(KeyCode.D) && transform.position.x < 8.9f)
        {
            xPlus = 0.1f;
        }
        transform.Translate(xPlus, 0, 0);

        float xMinus = 0;

        if (Input.GetKey(KeyCode.A) && transform.position.x > -8.9f)
        {
            xMinus = -0.1f;
        }
        transform.Translate(xMinus, 0, 0);

        if (Input.GetKey(KeyCode.Space))
        {
        }
    }

}
