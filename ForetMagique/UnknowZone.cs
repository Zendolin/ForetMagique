using System;

public class UnknowZone
{
    public int coordX;
    public int coordY;

    // zone sans contenu car l'agent n'y est pas encore allé
    public UnknowZone(int x ,int y)
	{
        coordX = x;
        coordY = y;
	}
}
