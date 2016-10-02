using UnityEngine;
using System.Collections;

public class DragProjectile : Projectile {

    public float mass;
    public float area;
    public float rho;
    public float dragCo;

    public DragProjectile(Vector3 position, Vector3 velocity, 
                          float time, float mass, float area, float rho, float dragCo) : base(position, velocity, time)
    {
        this.mass = mass;
        this.area = area;
        this.rho = rho;
        this.dragCo = dragCo;
    }

    public float getMass()
    {
        return mass;
    }

    public float getArea()
    {
        return area;
    }

    public float getRho()
    {
        return rho;
    }

    public float getDragCo()
    {
        return dragCo;
    }

    public override void updateLocationAndVelocity(float dt)
    {
        ODESolver.RungeKutta4(this, dt);
    }

    public override float[] getRightHandSide(float s, float[] q, float[] deltaQ, float ds, float qScale)
    {
        float[] dQ = new float[6];
        float[] newQ = new float[6];

        for (int i = 0; i < 6; ++i)
        {
            newQ[i] = q[i] + qScale * deltaQ[i];
        }

        float vx = newQ[0];
        float vy = newQ[2];
        float vz = newQ[4];

        //v = sqrt(vx^2 + vy^2 + vz^2). "Mathf.Pow(10, -8)" er for å hindre divide by zero exception
        float v = Mathf.Sqrt(vx * vx + vy * vy + vz * vz) + Mathf.Pow(10, -8);

        //F_D = 1 / 2 \rho A *C_D * v ^ 2   algebraisk => Drag force
        float Fd = 0.5f * rho * area * dragCo * v * v;

        dQ[0] = -ds * Fd * vx / (mass * v);
        dQ[1] = ds * vx;
        dQ[2] = ds * (G - Fd * vy / (mass * v));
        dQ[3] = ds * vy;
        dQ[4] = -ds * Fd * vz / (mass * v);
        dQ[5] = ds * vz;

        return dQ;
    }
}
