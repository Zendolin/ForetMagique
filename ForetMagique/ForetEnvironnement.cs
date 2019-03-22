using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class ForetEnvironnement
{
    public Thread thread;
    public int NbZonesLigne = 9;

    private int posAgentX;
    private int posAgentY;
    private Zone[,] zonesForet;
    private int performance = 0;
    
    public ForetEnvironnement()
    {
        thread = new Thread(new ThreadStart(ThreadLoop));
        zonesForet = new Zone[NbZonesLigne, NbZonesLigne];
        posAgentX = 0;
        posAgentY = 0;
        Console.WriteLine("Debut Creation Foret");
        for (int x = 0; x < NbZonesLigne; x++)
        {
            for (int y = 0; y < NbZonesLigne; y++)
            {
                zonesForet[y, x] = new Zone(x,y);
            }
        }
        Console.WriteLine("Creation ForetOK");
        RemplirForet();
    }

    private void RemplirForet()
    {
        zonesForet[0, 0].contenu.Add("agent");
        zonesForet[0, 0].visité = true;
        Random rnd = new Random();
        zonesForet[rnd.Next(1,NbZonesLigne), rnd.Next(1, NbZonesLigne)].contenu.Add("portail");

        for (int x = 0; x < NbZonesLigne; x++)
        {
            for (int y = 0; y < NbZonesLigne; y++)
            {
                if (zonesForet[y, x].contenu.Count == 0)
                {
                    int chance = rnd.Next(0, 100);
                    if (chance < 10) zonesForet[y, x].contenu.Add("monstre");
                }
            }
        }
        for (int x = 0; x < NbZonesLigne; x++)
        {
            for (int y = 0; y < NbZonesLigne; y++)
            {
                if (zonesForet[y, x].contenu.Count == 0)
                {
                    int chance = rnd.Next(0, 100);
                    if (chance < 10) zonesForet[y, x].contenu.Add("crevasse");
                }
            }
        }

        Console.WriteLine("Monstres et crevasses placés");
        for (int x = 0; x < NbZonesLigne; x++)
        {
            for (int y = 0; y < NbZonesLigne; y++)
            {
                if (MonstreEstProche(x, y)) zonesForet[y, x].contenu.Add("odeur");
                if (CrevasseEstProche(x, y)) zonesForet[y, x].contenu.Add("vent");
            }
        }
    }

    private bool MonstreEstProche(int x ,int y)
    {
        if (x > 0 && zonesForet[y, x - 1].contenu.Contains("monstre")) return true;
        else if (x < NbZonesLigne - 1 && zonesForet[y, x + 1].contenu.Contains("monstre")) return true;
        else if (y > 0 && zonesForet[y - 1, x].contenu.Contains("monstre")) return true;
        else if (y < NbZonesLigne - 1 && zonesForet[y + 1, x].contenu.Contains("monstre")) return true;
        else return false;
    }

    private bool CrevasseEstProche(int x, int y)
    {
        if (x > 0 && zonesForet[y, x - 1].contenu.Contains("crevasse")) return true;
        else if (x < NbZonesLigne - 1 && zonesForet[y, x + 1].contenu.Contains("crevasse")) return true;
        else if (y > 0 && zonesForet[y - 1, x].contenu.Contains("crevasse")) return true;
        else if (y < NbZonesLigne - 1 && zonesForet[y + 1, x].contenu.Contains("crevasse")) return true;
        else return false;
    }

    public Zone GetCurrentZone()
    {
        return zonesForet[posAgentY, posAgentX];
    }

    public Zone GetZone(int x, int y)
    {
        return zonesForet[y, x];
    }

    public void DeplacementAgent(string direction)
    {
        performance -= 1;
        zonesForet[posAgentY, posAgentX].contenu.RemoveAt(0);
        switch (direction)
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
        zonesForet[posAgentY, posAgentX].contenu.Insert(0, "agent");
        zonesForet[posAgentY, posAgentX].visité = true;
    }

    public void AllerSur(int x ,int y)
    {
        zonesForet[posAgentY, posAgentX].contenu.RemoveAt(0);
        posAgentX = x;
        posAgentY = y;
        zonesForet[posAgentY, posAgentX].contenu.Insert(0, "agent");
        zonesForet[posAgentY, posAgentX].visité = true;
    }

    public void LancerCaillou(Zone z)
    {
        performance -= 10;
        if (z.contenu.Contains("monstre")) z.contenu.Remove("monstre");
    }

    public void PasserPortail()
    {
        performance += 10 * NbZonesLigne * NbZonesLigne;
        //TODO niveau suivant
    }

    private void DessinerZone(Zone zone) // Dessin en ASCII de la foret
    {
        if (posAgentX == zone.coordX && posAgentY == zone.coordY)
        {
            Console.Write("[A]");
        }
        else if (zone.contenu.Contains("portail")) Console.Write("[P]");
        else if (zone.contenu.Contains("monstre") ) Console.Write("[M]");
        else if (zone.contenu.Contains("crevasse")) Console.Write("[C]");
        else if (zone.contenu.Contains("vent")) Console.Write("[V]");
        else if (zone.contenu.Contains("odeur")) Console.Write("[O]"); 
        else Console.Write("[ ]"); 
    }

    private void DessinerForet()
    {
        for (int i = 0; i < NbZonesLigne; i++)
        {
            for (int j = 0; j < NbZonesLigne; j++)
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

            Thread.Sleep(1000);
        }
    }

    public int GetPosX() { return posAgentX; }
    public int GetPosY() { return posAgentY; }
}

