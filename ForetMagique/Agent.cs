using System;
using System.Collections.Generic;
using System.Threading;

public class Agent
{
    public Thread thread;
    private Capteur capteur;
    private Effecteur effecteur;


    private Zone[,] environnement; // Belief
    private List<string> listeActions = new List<string>(); // Intentions

    public Agent(ForetEnvironnement env)
    {
        thread = new Thread(new ThreadStart(ThreadLoop));
        capteur = new Capteur(env);
        effecteur = new Effecteur(env);
    }

    public void Agir()
    {

    }

    public void ThreadLoop()
    {
        // Tant que le thread n'est pas tué, on travaille
        while (Thread.CurrentThread.IsAlive)
        {

        }
    }
}
