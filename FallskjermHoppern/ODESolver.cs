using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ODESolver
{

    public static void RungeKutta4(ODE ode, float ds)
    {
        int j;
        int numEqns = ode.getNumEqns();
        float s;
        float[] q;
        float[] dq1 = new float[numEqns];
        float[] dq2 = new float[numEqns];
        float[] dq3 = new float[numEqns];
        float[] dq4 = new float[numEqns];

        s = ode.getS();
        q = ode.getAllQ();

        dq1 = ode.getRightHandSide(s, q, q, ds, 0.0f);
        dq2 = ode.getRightHandSide(s + 0.5f * ds, q, dq1, ds, 0.5f);
        dq3 = ode.getRightHandSide(s + 0.5f * ds, q, dq2, ds, 0.5f);
        dq4 = ode.getRightHandSide(s + ds, q, dq3, ds, 1.0f);

        ode.setS(s + ds);

        for (j = 0; j < numEqns; ++j)
        {
            q[j] = q[j] + (dq1[j] + 2.0f * dq2[j] + 2.0f * dq3[j] + dq4[j]) / 6.0f;
            ode.setQ(q[j], j);
        }

        return;
    }
}