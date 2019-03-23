using System;
using System.Collections.Generic;
using System.Threading;
using static System.Linq.Enumerable;
public class Agent
{
    public Thread thread;
    private Capteur capteur;
    private Effecteur effecteur;

    private Zone[,] zonesConnues; // Belief
    private List<Zone> frontière; // Les prochaines zones accessibles par l'agent

    public Agent(ForetEnvironnement env)
    {
        thread = new Thread(new ThreadStart(ThreadLoop));
        capteur = new Capteur(env);
        effecteur = new Effecteur(env);
        frontière = new List<Zone>();
        zonesConnues = new Zone[capteur.getNBLignes(),capteur.getNBLignes()];
    }

    public Zone Reflechir()
    {
        int posX = capteur.GetPosX();
        int posY = capteur.GetPosX();
        zonesConnues[posY, posX] = capteur.GetZone();

        //lister frontiere
        frontière = new List<Zone>();
        foreach(Zone z in zonesConnues)
        {
            if (z != null)
            {
                frontière.AddRange(GetZonesVisitables(z.coordX, z.coordY));
                frontière = frontière.Distinct<Zone>().ToList();
            }
        }
        foreach (Zone z in frontière) if(z!=null) z.estFrontiere = true;

        //calculer risques
        foreach(Zone z in frontière)
        {
            z.probaPortail = 0;
            z.probaMonstre = 0;
            z.probaCrevasse = 0;
            if(z.voisinGauche != null && z.voisinGauche.visité)
            {
                if (z.voisinGauche.contenu.Contains("odeur")) z.probaMonstre += 0.25;
                if (z.voisinGauche.contenu.Contains("vent")) z.probaCrevasse += 0.25;
                if (z.voisinGauche.contenu.Contains("lumiere")) z.probaPortail += 0.25;
            }
            if (z.voisinDroite != null && z.voisinDroite.visité)
            {
                if (z.voisinDroite.contenu.Contains("odeur")) z.probaMonstre += 0.25;
                if (z.voisinDroite.contenu.Contains("vent")) z.probaCrevasse += 0.25;
                if (z.voisinDroite.contenu.Contains("lumiere")) z.probaPortail += 0.25;
            }
            if (z.voisinHaut != null && z.voisinHaut.visité)
            {
                if (z.voisinHaut.contenu.Contains("odeur")) z.probaMonstre += 0.25;
                if (z.voisinHaut.contenu.Contains("vent")) z.probaCrevasse += 0.25;
                if (z.voisinHaut.contenu.Contains("lumiere")) z.probaPortail += 0.25;
            }
            if (z.voisinBas != null && z.voisinBas.visité)
            {
                if (z.voisinBas.contenu.Contains("odeur")) z.probaMonstre += 0.25;
                if (z.voisinBas.contenu.Contains("vent")) z.probaCrevasse += 0.25;
                if (z.voisinBas.contenu.Contains("lumiere")) z.probaPortail += 0.25;
            }
        }
        List<Zone> frontiereTriée = new List<Zone>();
        // choisir + safe

        //priorité au portail
        foreach (Zone z in frontière) if (z.probaPortail == 1) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaPortail == 0.75) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaPortail == 0.5) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaPortail == 0.25) frontiereTriée.Add(z);
        frontière = frontière.Except(frontiereTriée).ToList();
        //priorité au monstra car on peut l'eliminer
        foreach (Zone z in frontière) if (z.probaMonstre == 1) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaMonstre == 0.75) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaMonstre == 0.5) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaMonstre == 0.25) frontiereTriée.Add(z);
        frontière = frontière.Except(frontiereTriée).ToList();
        // si monstre alors lancer caillou
        foreach (Zone z in frontière) if (z.probaCrevasse == 0.25) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaCrevasse == 0.5) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaCrevasse == 0.75) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaCrevasse == 1) frontiereTriée.Add(z);
        frontière = frontière.Except(frontiereTriée).ToList();

        //go zone
        return frontière[0];
    }

    public void Agir(Zone zoneChoisie)
    {
        if (zoneChoisie.probaPortail <= 0.25 && zoneChoisie.probaMonstre >= 0.50)
            effecteur.LancerCaillou(zoneChoisie);
        AStar(zoneChoisie.coordX, zoneChoisie.coordY);
    }

    public void AStar(int x , int y)
    {
        Zone zoneAct = null;
        var start = capteur.GetZone();
        var target = capteur.GetZone(x,y);
        var listeZonesPossibles = new List<Zone>();
        var listeZoneParcourus = new List<Zone>();
        int distanceDepart = 0;

        while (listeZonesPossibles.Count > 0)
        {
            // get the square with the lowest F score
            var plusPetit = listeZonesPossibles.Min(l => l.distanceSomme);
            zoneAct = listeZonesPossibles.First(l => l.distanceSomme == plusPetit);

            // add the current square to the closed list
            listeZoneParcourus.Add(zoneAct);

            // show current square on the map
            effecteur.AllerSur(zoneAct.coordX, zoneAct.coordY);
            Thread.Sleep(1000);

            // remove it from the open list
            listeZonesPossibles.Remove(zoneAct);

            // if we added the destination to the closed list, we've found a path
            if (listeZoneParcourus.FirstOrDefault(l => l.coordX == target.coordY && l.coordY == target.coordY) != null)
                break;

            var adjacentSquares = GetZonesProches(zoneAct.coordY, zoneAct.coordY,target.coordX,target.coordY);
            distanceDepart++;

            foreach (var adjacentSquare in adjacentSquares)
            {
                // if this adjacent square is already in the closed list, ignore it
                if (listeZoneParcourus.FirstOrDefault(l => l.coordX == adjacentSquare.coordX
                        && l.coordX == adjacentSquare.coordY) != null)
                    continue;

                // if it's not in the open list...
                if (listeZonesPossibles.FirstOrDefault(l => l.coordX == adjacentSquare.coordX
                        && l.coordY == adjacentSquare.coordY) == null)
                {
                    // compute its score, set the parent
                    adjacentSquare.distanceDepart = distanceDepart;
                    adjacentSquare.distanceEstimeArrive = ComputeHScore(adjacentSquare.coordX, adjacentSquare.coordY, target.coordX, target.coordY);
                    adjacentSquare.distanceSomme = adjacentSquare.distanceDepart + adjacentSquare.distanceEstimeArrive;
                    adjacentSquare.parent = zoneAct;

                    // and add it to the open list
                    listeZonesPossibles.Insert(0, adjacentSquare);
                }
                else
                {
                    // test if using the current G score makes the adjacent square's F score
                    // lower, if yes update the parent because it means it's a better path
                    if (distanceDepart + adjacentSquare.distanceEstimeArrive < adjacentSquare.distanceSomme)
                    {
                        adjacentSquare.distanceDepart = distanceDepart;
                        adjacentSquare.distanceSomme = adjacentSquare.distanceDepart + adjacentSquare.distanceEstimeArrive;
                        adjacentSquare.parent = zoneAct;
                    }
                }
            }
        }
    }

    public void ThreadLoop()
    {
        effecteur.DessinerForet();
        Thread.Sleep(1000);
        // Tant que le thread n'est pas tué, on travaille
        while (Thread.CurrentThread.IsAlive)
        {
            effecteur.DessinerForet();
            Zone z = Reflechir();
            Agir(z);
            Thread.Sleep(1000);
            
        }
    }

    //retourne les cases accessibles depuis les coordonnés en paramètres
    private List<Zone> GetZonesProches(int x, int y,int destX,int destY)
    {
        List<Zone> zonesProches = new List<Zone>();
        if ((x - 1 == destX && y == destY) || (x+1 == destX && y == destY)|| (x== destX && y-1 == destY) || (x== destX && y+1 == destY))
            zonesProches.Add(capteur.GetZone(x, y));
        if (x > 0 && zonesConnues[y, x - 1] != null) zonesProches.Add(zonesConnues[y, x - 1]);
        if (x <  capteur.getNBLignes()-1 &&  zonesConnues[y, x + 1] != null) zonesProches.Add(zonesConnues[y, x+1 ]);
        if (y > 0 && zonesConnues[y-1, x] != null) zonesProches.Add(zonesConnues[y-1, x]);
        if (y < capteur.getNBLignes() - 1 && zonesConnues[y+1, x] != null) zonesProches.Add(zonesConnues[y+1, x]);
        return zonesProches;
    }

    //retoure les cases de la frontiere
    private List<Zone> GetZonesVisitables(int x, int y)
    {
        List<Zone> zonesVisitables = new List<Zone>();
        if (x > 0 && !capteur.GetZone(x-1,y).visité)
            zonesVisitables.Add(capteur.GetZone(x-1,y));
        if (x < capteur.getNBLignes()-1 && !capteur.GetZone(x + 1, y).visité)
            zonesVisitables.Add(capteur.GetZone(x+1,y));
        if (y > 0 && !capteur.GetZone(x,y-1).visité)
            zonesVisitables.Add(capteur.GetZone(x, y - 1));
        if (y < capteur.getNBLignes() - 1 && !capteur.GetZone(x,y+1).visité)
            zonesVisitables.Add(capteur.GetZone(x, y + 1));
        return zonesVisitables;
    }

    //calcul distance destination
    static int ComputeHScore(int x, int y, int targetX, int targetY)
    {
        return Math.Abs(targetX - x) + Math.Abs(targetY - y);
    }
}
