public abstract class ODE
{
    private int numEqns; //Antall likninger som skal løses
    private float[] q;   //Array av avhengige variabler
    private float s;     //Uavhengig variabel

    public ODE(int numEqns)
    {
        this.numEqns = numEqns;
        q = new float[numEqns];
    }

    public ODE() { }

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

    //Abstrakt funksjon for getRightHandSide, skal bli videre implementert ettersom hva man trenger å
    //bruke den til. I mitt tilfelle blir det med klassen DragProjectile, kalkulering av skråkast og luftmotstand.
    public abstract float[] getRightHandSide(float s, float[] q, float[] deltaQ, float ds, float qScale);
}