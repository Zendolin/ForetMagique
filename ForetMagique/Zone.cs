using System;
using System.Collections.Generic;

public class Zone
{
    public int coordX;
    public int coordY;
    public List<String> contenu = new List<string>(); // monstre,vent,crevasse,odeur,lumiere,portail,Agent

    public Zone voisinGauche;
    public Zone voisinDroite;
    public Zone voisinHaut;
    public Zone voisinBas;
    public double probaPortail = 0;
    public double probaMonstre = 0;
    public double probaCrevasse = 0;

    //A*
    public int distanceSomme;
    public int distanceDepart;
    public int distanceEstimeArrive;
    public Zone parent;
    public bool visité;
    public bool estFrontiere;

    public Zone(int x, int y)
    {
        coordX = x;
        coordY = y;
    }

    public Zone(int x, int y,string contenu)
    {
        coordX = x;
        coordY = y;
        this.contenu.Add(contenu);
    }

    
}
