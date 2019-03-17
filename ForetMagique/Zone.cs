using System;
using System.Collections.Generic;

public class Zone
{
    public int coordsX;
    public int coordsY;
    public List<String> contenu = new List<string>(); // monstre,vent,crevasse,odeur,portail,Agent

    public Zone(int x, int y)
    {
        coordsX = x;
        coordsY = y;
    }

    public Zone(int x, int y,string contenu)
    {
        coordsX = x;
        coordsY = y;
        this.contenu.Add(contenu);
    }
}
