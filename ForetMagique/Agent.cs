using System;
using System.Collections.Generic;
using System.Threading;
using static System.Linq.Enumerable;
public class Agent
{
    public Thread thread;
    GestionConsole gc;
    private Capteur capteur;
    private Effecteur effecteur;
    private bool threadActif = true;
    public bool peutAvancer = false;
    public bool modeAuto = false;

    private Zone[,] zonesConnues; // Belief
    private List<Zone> frontière; // Les prochaines zones accessibles par l'agent

    public Agent(ForetEnvironnement env,GestionConsole gc)
    {
        thread = new Thread(new ThreadStart(ThreadProc));
        this.gc = gc;
        capteur = new Capteur(env);
        effecteur = new Effecteur(env);
        //frontière = new List<Zone>();
        //zonesConnues = new Zone[capteur.getNBLignes(),capteur.getNBLignes()];
    }

    public Zone Reflechir()
    {
        int posX = capteur.GetPosX();
        int posY = capteur.GetPosY();
        zonesConnues[posY, posX] = capteur.GetZone();

        //lister frontiere
        gc.AddConsole("Listage frontiere");
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
        foreach (Zone z in frontière) effecteur.DessinerZone(z);
            //calculer risques
            gc.AddConsole("Calcul des risques");
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
            int n = capteur.getNBLignes();
            // une zone sur le bord de la carte est plus risqué
            if (z.probaMonstre >= 0.25 && z.probaMonstre < 1 && (z.coordX == 0)) z.probaMonstre += 0.25;
            if (z.probaMonstre >= 0.25 && z.probaMonstre < 1 && (z.coordY == 0)) z.probaMonstre += 0.25;
            if (z.probaMonstre >= 0.25 && z.probaMonstre < 1 && (z.coordX == n - 1)) z.probaMonstre += 0.25;
            if (z.probaMonstre >= 0.25 && z.probaMonstre < 1 && (z.coordY == n - 1)) z.probaMonstre += 0.25;

            if (z.probaCrevasse >= 0.25 && z.probaCrevasse < 1 && (z.coordX == 0 || z.coordY == 0 || z.coordX == n - 1 || z.coordY == n - 1)) z.probaMonstre += 0.25;
            VerifierExistance(z);
        }
        List<Zone> frontiereTriée = new List<Zone>();

        // choisir + safe
        //priorité au portail
        foreach (Zone z in frontière) if (z.probaPortail >= 1) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaPortail >= 0.75 && z.probaPortail < 1) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaPortail >= 0.5 && z.probaPortail < 0.75) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaPortail >= 0.25 && z.probaPortail < 0.5) frontiereTriée.Add(z);
        frontière = frontière.Except(frontiereTriée).ToList();
        foreach (Zone z in frontière) if (z.probaMonstre == 0 && z.probaCrevasse == 0) frontiereTriée.Add(z);
        frontière = frontière.Except(frontiereTriée).ToList();
        
        //priorité au monstre car on peut l'eliminer
        foreach (Zone z in frontière) if (z.probaMonstre >= 1) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaPortail >= 0.75 && z.probaPortail < 1) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaPortail >= 0.5 && z.probaPortail < 0.75) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaPortail >= 0.25 && z.probaPortail < 0.5) frontiereTriée.Add(z);
        frontière = frontière.Except(frontiereTriée).ToList();

        foreach (Zone z in frontière) if (z.probaPortail >= 0.25 && z.probaPortail < 0.5) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaPortail >= 0.5 && z.probaPortail < 0.75) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaPortail >= 0.75 && z.probaPortail < 1) frontiereTriée.Add(z);
        foreach (Zone z in frontière) if (z.probaCrevasse >= 1) frontiereTriée.Add(z);
        frontière = frontière.Except(frontiereTriée).ToList();
        
        foreach (Zone z in frontière)frontiereTriée.Add(z);

        //go zone
        gc.AddConsole("Case choisie " + frontiereTriée[0]);
        return frontiereTriée[0];
    }

    public void Agir(Zone zoneChoisie)
    {
        AStar(zoneChoisie.coordX, zoneChoisie.coordY);
    }

    public void AStar(int x , int y)
    {
        gc.AddConsole("Debut A*");
        Zone zoneAct = null;
        Zone start = capteur.GetZone();
        Zone target = capteur.GetZone(x,y);
        List<Zone> listeZonesPossibles = new List<Zone>();
        List<Zone> listeZoneParcourus = new List<Zone>();
        int distanceDepart = 0;

        listeZonesPossibles.Add(start);
        gc.AddConsole("("+target.coordX+","+target.coordY+")");
        gc.Write();
        while (listeZonesPossibles.Count > 0)
        {
            // get the square with the lowest F score
            int plusPetit = listeZonesPossibles.Min(l => l.distanceSomme);
            zoneAct = listeZonesPossibles.First(l => l.distanceSomme == plusPetit);

            // add the current square to the closed list
            listeZoneParcourus.Add(zoneAct);

            // show current square on the map
            int etat = effecteur.AllerSur(zoneAct.coordX, zoneAct.coordY);
            if(etat == 1)
            {
                threadActif = false;
                Thread.Sleep(1000);
                gc.AddConsole("J'ai trouvé le Portail!");
                effecteur.PasserPortail();
            }
            else if (etat == -1)
            {
                threadActif = false;
                effecteur.Mourir();
            }
            Thread.Sleep(300);

            // remove it from the open list
            listeZonesPossibles.Remove(zoneAct);

            // if we added the destination to the closed list, we've found a path
            if (listeZoneParcourus.FirstOrDefault(l => l.coordX == target.coordX && l.coordY == target.coordY) != null)
                break;

            var adjacentSquares = GetZonesProches(zoneAct.coordX, zoneAct.coordY,target.coordX,target.coordY);
            distanceDepart++;

            foreach (var adjacentSquare in adjacentSquares)
            {
                // if this adjacent square is already in the closed list, ignore it
                if (listeZoneParcourus.FirstOrDefault(l => l.coordX == adjacentSquare.coordX
                        && l.coordY == adjacentSquare.coordY) != null)
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

    public void ThreadProc()
    {
        frontière = new List<Zone>();
        zonesConnues = new Zone[capteur.getNBLignes(), capteur.getNBLignes()];
        effecteur.DessinerForet();
        threadActif = true;
        Thread.Sleep(1000);
        // Tant que le thread n'est pas tué, on travaille
        while (threadActif)
        {
            if(peutAvancer || modeAuto)
            {
                Zone z = Reflechir();
                Agir(z);
                peutAvancer = false;
            }
            Console.Clear();
           // effecteur.DessinerForet();

            //gc.AddConsole(posAspiX + ":" + posAspiY);
            gc.Write();
            Thread.Sleep(300);    
        }
    }

    //retourne les cases accessibles depuis les coordonnés en paramètres
    private List<Zone> GetZonesProches(int x, int y,int destX,int destY)
    {
        List<Zone> zonesProches = new List<Zone>();
        if ((x - 1 == destX && y == destY) || (x+1 == destX && y == destY)|| (x== destX && y-1 == destY) || (x== destX && y+1 == destY))
        {
            Zone z = capteur.GetZone(destX, destY);
            zonesProches.Add(z);
            if (z.probaMonstre >= 0.25) effecteur.LancerCaillou(z);
        }
            
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

    // Si l'une des zone voisines ne contient pas d'indicateur , alors on est certain qu'il n'y a rien sur la zone
    public void VerifierExistance(Zone z)
    {
        bool peutContenirMonstre = true;
        bool peutContenirCrevasse = true;

        if (z.voisinGauche != null && z.voisinGauche.visité)
        {
            if (!z.voisinGauche.contenu.Contains("odeur")) peutContenirMonstre = false;
            if (!z.voisinGauche.contenu.Contains("vent")) peutContenirCrevasse = false;
        }
        if (z.voisinDroite != null && z.voisinDroite.visité)
        {
            if (!z.voisinDroite.contenu.Contains("odeur")) peutContenirMonstre = false;
            if (!z.voisinDroite.contenu.Contains("vent")) peutContenirCrevasse = false;
        }
        if (z.voisinHaut != null && z.voisinHaut.visité)
        {
            if (!z.voisinHaut.contenu.Contains("odeur")) peutContenirMonstre = false;
            if (!z.voisinHaut.contenu.Contains("vent")) peutContenirCrevasse = false;
        }
        if (z.voisinBas != null && z.voisinBas.visité)
        {
            if (!z.voisinBas.contenu.Contains("odeur")) peutContenirMonstre = false;
            if (!z.voisinBas.contenu.Contains("vent")) peutContenirCrevasse = false;
        }
        if (!peutContenirMonstre) z.probaMonstre = 0;
        if (!peutContenirCrevasse) z.probaCrevasse = 0;
    }

    //calcul distance destination
    static int ComputeHScore(int x, int y, int targetX, int targetY)
    {
        return Math.Abs(targetX - x) + Math.Abs(targetY - y);
    }
}
