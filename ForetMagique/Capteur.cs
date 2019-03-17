using System;

public class Capteur // Classe qui récupère les données du manoir
{
    ForetEnvironnement foret;

    public Capteur(ForetEnvironnement f)
    {
        foret = f;
    }

    public  Zone getZone() // récupère une copie de l'environnement actuel
    {
        return null;
    }

    public int getNBLignes()
    {
        return foret.NbZonesLigne;
    }

    public int getPosX()
    {
        return 0;
    }

    public int getPosY()
    {
        return 0;
    }
 
}
