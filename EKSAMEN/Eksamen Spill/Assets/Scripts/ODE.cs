public abstract class ODE
{
    private int numEqns;
    private float[] q;
    private float s;

    public ODE(int numEqns)
    {
        this.numEqns = numEqns;
        q = new float[numEqns];
    }

    public ODE() { } //Default constructor

    public int getNumEqns()
    {
        return numEqns;
    }

    public float getS()
    {
        return s;
    }

    public float getQ(int index)
    {
        return q[index];
    }

    public float[] getAllQ()
    {
        return q;
    }

    public void setS(float value)
    {
        s = value;
        return;
    }

    public void setQ(float value, int index)
    {
        q[index] = value;
        return;
    }

    public abstract float[] getRightHandSide(float s, float[] q, float[] deltaQ, float ds, float qScale);
}