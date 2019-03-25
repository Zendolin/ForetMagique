using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class ForetEnvironnement
{
   // public Thread thread;
    public int NbZonesLigne = 9;
    GestionConsole gc;
    private int posAgentX;
    private int posAgentY;
    private Zone[,] zonesForet;
    private int performance = 0;

    private ForetMagique.IGForet ig;


    public ForetEnvironnement(ForetMagique.IGForet ig,GestionConsole gc)
    {
        this.gc = gc;
        zonesForet = new Zone[NbZonesLigne, NbZonesLigne];
        this.ig = ig;
        this.ig.UpdatePanel(NbZonesLigne, NbZonesLigne);
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

        //ajout aléatoire de monstres
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
        //ajout aléatoire de crevasses
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
        //ajout des informations
        for (int x = 0; x < NbZonesLigne; x++)
        {
            for (int y = 0; y < NbZonesLigne; y++)
            {
                if (MonstreEstProche(x, y)) zonesForet[y, x].contenu.Add("odeur");
                if (CrevasseEstProche(x, y)) zonesForet[y, x].contenu.Add("vent");
                if (PortailEstProche(x, y)) zonesForet[y, x].contenu.Add("lumiere");
            }
        }

        // ajout des cases voisines
        for (int x = 0; x < NbZonesLigne; x++)
        {
            for (int y = 0; y < NbZonesLigne; y++)
            {
                if (x > 0) zonesForet[y, x].voisinGauche = zonesForet[y, x - 1];
                if (x < NbZonesLigne-1) zonesForet[y, x].voisinDroite = zonesForet[y, x + 1];
                if (y > 0) zonesForet[y, x].voisinHaut = zonesForet[y-1, x];
                if (y < NbZonesLigne - 1) zonesForet[y, x].voisinBas = zonesForet[y+1, x];
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

    private bool PortailEstProche(int x, int y)
    {
        if (x > 0 && zonesForet[y, x - 1].contenu.Contains("portail")) return true;
        else if (x < NbZonesLigne - 1 && zonesForet[y, x + 1].contenu.Contains("portail")) return true;
        else if (y > 0 && zonesForet[y - 1, x].contenu.Contains("portail")) return true;
        else if (y < NbZonesLigne - 1 && zonesForet[y + 1, x].contenu.Contains("portail")) return true;
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

    public void AllerSur(int x ,int y)
    {
        zonesForet[posAgentY, posAgentX].contenu.RemoveAt(0);
        ig.SetZone(zonesForet[posAgentY, posAgentX]);
        posAgentX = x;
        posAgentY = y;
        performance -= 1;
        ig.updatePerf(performance);
        zonesForet[posAgentY, posAgentX].contenu.Insert(0, "agent");
        ig.SetZone(zonesForet[posAgentY, posAgentX]);
        zonesForet[posAgentY, posAgentX].visité = true;
        zonesForet[posAgentY, posAgentX].estFrontiere = false;
    }

    public void LancerCaillou(Zone z)
    {
        performance -= 10;
        if (z.contenu.Contains("monstre")) { z.contenu.Remove("monstre"); z.contenu.Add("monstre_mort"); }
        ig.SetZone(zonesForet[z.coordY, z.coordX]);
        ig.updatePerf(performance);
    }

    public void PasserPortail()
    {
        performance += 10 * NbZonesLigne * NbZonesLigne;
        ig.updatePerf(performance);
        //TODO niveau suivant
    }

    public void DessinerZone(Zone zone) // Dessin en ASCII de la foret
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

        ig.SetZone(zone);

    }

    public void DessinerForet()
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

