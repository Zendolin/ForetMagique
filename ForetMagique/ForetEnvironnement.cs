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
    private readonly int NBMONSTRES = 2;
    private readonly int NBCREVASSES = 2;

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
        List<String> aPlacer = new List<String>();
        aPlacer.Add("portail");
        zonesForet[0, 0].contenu.Add("agent");
        for (int i = 0; i < NBMONSTRES; i++) aPlacer.Add("monstre");
        for (int i = 0; i < NBCREVASSES; i++) aPlacer.Add("crevasse");

        Random rnd = new Random();
        while (aPlacer.Count>0)
        {
            int x = rnd.Next(0, NbZonesLigne);
            int y = rnd.Next(0, NbZonesLigne);
            Console.WriteLine("Loop generation,count : " + aPlacer.Count + ", " + aPlacer.First() + " ,pos:" + x + ":" + y);
            if (zonesForet[y, x].contenu.Count == 0)
            {
                zonesForet[y, x].contenu.Add(aPlacer.First());
                aPlacer.RemoveAt(0);
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
        if (posAgentX == zone.coordsX && posAgentY == zone.coordsY)
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
}

