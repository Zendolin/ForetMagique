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
    private List<UnknowZone> zonesVisitables; // Les prochaines zones accessibles par l'agent

    public Agent(ForetEnvironnement env)
    {
        thread = new Thread(new ThreadStart(ThreadLoop));
        capteur = new Capteur(env);
        effecteur = new Effecteur(env);
        zonesVisitables = new List<UnknowZone>();
    }

    public void Reflechir()
    {

    }

    public void Agir()
    {

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

            var adjacentSquares = GetZonesProches(zoneAct.coordY, zoneAct.coordY);
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
        // Tant que le thread n'est pas tué, on travaille
        while (Thread.CurrentThread.IsAlive)
        {
            Reflechir();
            Agir();
        }
    }

    private List<Zone> GetZonesProches(int x, int y)
    {
        List<Zone> zonesProches = new List<Zone>();
        if (x > 0 && zonesConnues[y, x - 1] != null) zonesProches.Add(zonesConnues[y, x - 1]);
        if (x <  capteur.getNBLignes()-1 &&  zonesConnues[y, x + 1] != null) zonesProches.Add(zonesConnues[y, x+1 ]);
        if (y > 0 && zonesConnues[y-1, x] != null) zonesProches.Add(zonesConnues[y-1, x]);
        if (y < capteur.getNBLignes() - 1 && zonesConnues[y+1, x] != null) zonesProches.Add(zonesConnues[y+1, x]);
        return zonesProches;
    }

    //calcul distance destination
    static int ComputeHScore(int x, int y, int targetX, int targetY)
    {
        return Math.Abs(targetX - x) + Math.Abs(targetY - y);
    }
}
