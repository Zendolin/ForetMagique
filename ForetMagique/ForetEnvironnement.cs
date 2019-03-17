using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class ForetEnvironnement
{
    public Thread thread;
    public int NBPIECESLIGNE = 3;
    private int posAgentX;
    private int posAgentY;
    private Zone[,] zonesForet;
    private int performance = 0;
    
    public ForetEnvironnement()
    {
        thread = new Thread(new ThreadStart(ThreadLoop));
        zonesForet = new Zone[NBPIECESLIGNE, NBPIECESLIGNE];
        posAgentX = 0;
        posAgentY = 1;

        for (int x = 0; x < NBPIECESLIGNE; x++)
        {
            for (int y = 0; y < NBPIECESLIGNE; y++)
            {
                zonesForet[y, x] = new Zone(x,y,x+","+y);
            }
        }
    }

    public Zone GetCurrentZone()
    {
        return zonesForet[posAgentY, posAgentX];
    }

    public void DeplacementAgent(string direction)
    {
        performance -= 1;
        switch(direction)
        {
            case ("gauche"):
                posAgentX -=1;
                break;
            case ("droite"):
                posAgentX += 1;
                break;
            case ("haut"):
                posAgentY -= 1;
                break;
            case ("bas"):
                posAgentY += 1;
                break;
        }
        //zonesForet[posAgentY, posAgentX]
    }

    public void LancerCaillou(Zone z)
    {
        performance -= 10;
    }

    public void PasserPortail()
    {
        performance += 10 * NBPIECESLIGNE * NBPIECESLIGNE;
    }

    private void DessinerZone(Zone zone) // Dessin en ASCII du manoir
    {
        
        if (posAgentX == zone.coordsX && posAgentY == zone.coordsY)
        {
            Console.Write("[A]");
        }
        else Console.Write("[" + zone.contenu + "]");

        /*
        else if (zone.contenu == "monstre") Console.Write("[M]");
        else if (zone.contenu == "vent") Console.Write("[V]");
        else if (zone.contenu == "crevasse") Console.Write("[C]");
        else if (zone.contenu == "odeur") Console.Write("[O]");
        else if (zone.contenu == "portail") Console.Write("[P]");
        else Console.Write("[ ]"); */
        
    }

    private void DessinerForet()
    {
        for (int i = 0; i < NBPIECESLIGNE; i++)
        {
            for (int j = 0; j < NBPIECESLIGNE; j++)
            {
                DessinerZone(zonesForet[i, j]);
            }
            Console.Write("\n");
        }
    }

    public void ThreadLoop()
    {
        while (Thread.CurrentThread.IsAlive)
        {
            Console.Clear();
            DessinerForet();
            GetCurrentZone();

            Thread.Sleep(1000);
        }
    }
}

