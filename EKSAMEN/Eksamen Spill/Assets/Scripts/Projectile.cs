using UnityEngine;

public class Projectile : ODE
{

    public static float G = -9.81f;

    public Projectile() { }
    public Projectile(Vector3 position, Vector3 velocity, float time) : base(6)
    {
        setS(time);
        setQ(velocity.x, 0);
        setQ(position.x, 1);
        setQ(velocity.y, 2);
        setQ(position.y, 3);
        setQ(velocity.z, 4);
        setQ(position.z, 5);
    }

    public Vector3 getPosition()
    {
        return new Vector3(getX(), getY(), getZ());
    }

    public Vector3 getVelocity()
    {
        return new Vector3(getVx(), getVy(), getVz());
    }

    public float getVx()
    {
        return getQ(0);
    }

    public float getVy()
    {
        return getQ(2);
    }

    public float getVz()
    {
        return getQ(4);
    }

    public float getX()
    {
        return getQ(1);
    }

    public float getY()
    {
        return getQ(3);
    }

    public float getZ()
    {
        return getQ(5);
    }

    public float getTime()
    {
        return getS();
    }

    public virtual void updateLocationAndVelocity(float dt)
    {
        float time = getS();
        float vx0 = getQ(0);
        float x0 = getQ(1);
        float vy0 = getQ(2);
        float y0 = getQ(3);
        float vz0 = getQ(4);
        float z0 = getQ(5);

        float vy = vy0 + G * dt;
        float x = x0 + vx0 * dt;
        float y = y0 + vy0 * dt + 0.5f * G * dt * dt;
        float z = z0 + vz0 * dt ;

        time = time + dt;

        setS(time);
        setQ(x, 1);
        setQ(y, 3);
        setQ(vy, 2);
        setQ(z, 5); 
    }

    public override float[] getRightHandSide(float s, float[] q, float[] deltaQ, float ds, float qScale)
    {
        return new float[1];
    }
}
