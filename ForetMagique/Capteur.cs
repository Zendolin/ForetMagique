using System;

public class Capteur // Classe qui récupère les données du manoir
{
    ForetEnvironnement foret;

    public Capteur(ForetEnvironnement f)
    {
        foret = f;
    }

    public  Zone GetZone() // récupère la case actuelle de l'agent
    {
        return foret.GetCurrentZone();
    }
    public Zone GetZone(int x , int y) // récupère la case actuelle de l'agent
    {
        return foret.GetZone(x,y);
    }

    public int getNBLignes()
    {
        return foret.NbZonesLigne;
    }

    public int GetPosX()
    {
        return foret.GetPosX();
    }

    public int GetPosY()
    {
        return foret.GetPosY();
    }
 
}
