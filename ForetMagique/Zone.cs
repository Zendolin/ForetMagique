using System;

public class Zone
{
    public int coordsX;
    public int coordsY;
    public string contenu; // monstre,vent,crevasse,odeur,portail

    public Zone(int x, int y)
    {
        coordsX = x;
        coordsY = y;
    }

    public Zone(int x, int y,string contenu)
    {
        coordsX = x;
        coordsY = y;
        this.contenu = contenu;
    }
}
