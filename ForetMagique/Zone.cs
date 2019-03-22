using System;
using System.Collections.Generic;

public class Zone
{
    public int coordX;
    public int coordY;
    public List<String> contenu = new List<string>(); // monstre,vent,crevasse,odeur,portail,Agent

    //A*
    public int distanceSomme;
    public int distanceDepart;
    public int distanceEstimeArrive;
    public Zone parent;
    public bool visité;

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
