using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class ForetEnvironnement
{
    public int NBPIECESLIGNE = 3;
    int posAgentX;
    int posAgentY;
    Zone[,] zonesForet;
    
    public ForetEnvironnement()
    {
        zonesForet = new Zone[NBPIECESLIGNE, NBPIECESLIGNE];
        posAgentX = 0;
        posAgentY = 0;

        for (int i = 0; i < NBPIECESLIGNE; i++)
        {
            for (int j = 0; j < NBPIECESLIGNE; j++)
            {
                zonesForet[i, j] = new Zone(i, j,i+","+j);
            }
        }
    }

    private void DessinerZone(Zone zone) // Dessin en ASCII du manoir
    {
        if (posAgentX == zone.coordsX && posAgentY == zone.coordsY)
        {
            //Console.Write("[A]"); // piece contient l'agent Aspirateur
            Console.Write("[" + zone.contenu + "]");
        }
        else Console.Write("["+zone.contenu+"]"); // piece contient un bijoux
    }

    private void DessinerForet()
    {
        for (int i = 0; i < NBPIECESLIGNE; i++)
        {
            for (int j = 0; j < NBPIECESLIGNE; j++)
            {
                DessinerZone(zonesForet[j, i]);
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

            Thread.Sleep(1000);
        }
    }
}

