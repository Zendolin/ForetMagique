using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class ForetEnvironnement
{
   // public Thread thread;
    public int NbZonesLigne = 3;
    GestionConsole gc;
    private int posAgentX;
    private int posAgentY;
    private Zone[,] zonesForet;
    private int performance = 0;
    public Agent agent;

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
        int xPortail = rnd.Next(1, NbZonesLigne);
        int yPortail = rnd.Next(1, NbZonesLigne);
        zonesForet[yPortail, xPortail].contenu.Add("portail");
        List<Zone> listesZonesMonstres = new List<Zone>();
        List<Zone> listesZonesCrevasses = new List<Zone>();

        //ajout aléatoire de monstres
        for (int x = 0; x < NbZonesLigne; x++)
        {
            for (int y = 0; y < NbZonesLigne; y++)
            {
                if (zonesForet[y, x].contenu.Count == 0)
                {
                    int chance = rnd.Next(0, 100);
                    if (chance < 10)
                    {
                        zonesForet[y, x].contenu.Add("monstre");
                        listesZonesMonstres.Add(zonesForet[y, x]);
                    }
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
                    if (chance < 10)
                    {
                        zonesForet[y, x].contenu.Add("crevasse");
                        listesZonesCrevasses.Add(zonesForet[y, x]);
                    }
                }
            }
        }

        Console.WriteLine("Monstres et crevasses placés");
        
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
        //ajout des informations
        foreach (Zone z in listesZonesCrevasses)
        {
            if (z.voisinBas != null) z.voisinBas.contenu.Add("vent");
            if (z.voisinHaut != null) z.voisinHaut.contenu.Add("vent");
            if (z.voisinGauche != null) z.voisinGauche.contenu.Add("vent");
            if (z.voisinDroite != null) z.voisinDroite.contenu.Add("vent");
        }
        foreach (Zone z in listesZonesMonstres)
        {
            if (z.voisinBas != null) z.voisinBas.contenu.Add("odeur");
            if (z.voisinHaut != null) z.voisinHaut.contenu.Add("odeur");
            if (z.voisinGauche != null) z.voisinGauche.contenu.Add("odeur");
            if (z.voisinDroite != null) z.voisinDroite.contenu.Add("odeur");   
        }
        if (zonesForet[yPortail, xPortail].voisinGauche != null) zonesForet[yPortail, xPortail].voisinGauche.contenu.Add("lumiere");
        if (zonesForet[yPortail, xPortail].voisinDroite != null) zonesForet[yPortail, xPortail].voisinDroite.contenu.Add("lumiere");
        if (zonesForet[yPortail, xPortail].voisinBas != null) zonesForet[yPortail, xPortail].voisinBas.contenu.Add("lumiere");
        if (zonesForet[yPortail, xPortail].voisinHaut != null) zonesForet[yPortail, xPortail].voisinHaut.contenu.Add("lumiere");

        gc.AddConsole("Fin generation Foret");
    }

    public Zone GetCurrentZone()
    {
        return zonesForet[posAgentY, posAgentX];
    }

    public Zone GetZone(int x, int y)
    {
        return zonesForet[y, x];
    }

    public int AllerSur(int x ,int y)
    {
        zonesForet[posAgentY, posAgentX].contenu.RemoveAt(0);
        ig.SetZone(zonesForet[posAgentY, posAgentX]);
        posAgentX = x;
        posAgentY = y;
        performance -= 1;
        ig.UpdatePerf(performance);
        Zone z = zonesForet[posAgentY, posAgentX];
        z.contenu.Insert(0, "agent");
        ig.SetZone(z);
        z.visité = true;
        z.estFrontiere = false;
        if (z.contenu.Contains("monstre") || z.contenu.Contains("crevasse"))
            return -1;
        else if (z.contenu.Contains("portail")) return 1;
        else return 0;
    }

    public void LancerCaillou(Zone z)
    {
        performance -= 10;
        if (z.contenu.Contains("monstre")) { z.contenu.Remove("monstre"); z.contenu.Insert(0,"monstre_mort"); }
        ig.SetZone(zonesForet[z.coordY, z.coordX]);
        ig.UpdatePerf(performance);
    }

    public void PasserPortail()
    {
        performance += 10 * NbZonesLigne * NbZonesLigne;
        ig.UpdatePerf(performance);
        gc.AddConsole("Portail Atteint ! performance" + performance);
        //niveau suivant
        NbZonesLigne += 1;
        zonesForet = new Zone[NbZonesLigne, NbZonesLigne];
        this.ig.SetPanelThreadSafe(NbZonesLigne, NbZonesLigne);
        posAgentX = 0;
        posAgentY = 0;
        Console.WriteLine("Debut Creation Foret");
        for (int x = 0; x < NbZonesLigne; x++)
        {
            for (int y = 0; y < NbZonesLigne; y++)
            {
                zonesForet[y, x] = new Zone(x, y);
            }
        }
        Console.WriteLine("Creation Foret OK");
        RemplirForet();


        agent = new Agent(this, this.gc);
        agent.thread.Start();
    }

    public void Mourir()
    {
        performance -= 10 * NbZonesLigne * NbZonesLigne;
        ig.UpdatePerf(performance);
        gc.AddConsole("Perdu!");

        zonesForet = new Zone[NbZonesLigne, NbZonesLigne];
        this.ig.SetPanelThreadSafe(NbZonesLigne, NbZonesLigne);
        posAgentX = 0;
        posAgentY = 0;
        Console.WriteLine("Debut Creation Foret");
        for (int x = 0; x < NbZonesLigne; x++)
        {
            for (int y = 0; y < NbZonesLigne; y++)
            {
                zonesForet[y, x] = new Zone(x, y);
            }
        }
        Console.WriteLine("Creation ForetOK");
        RemplirForet();


        agent = new Agent(this, this.gc);
        agent.thread.Start();
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
    /*
    public void ThreadLoop()
    {
        while (Thread.CurrentThread.IsAlive)
        {
            Console.Clear();


            DessinerForet();

            Thread.Sleep(1000);
        }
    }*/

    public int GetPosX() { return posAgentX; }
    public int GetPosY() { return posAgentY; }
}

