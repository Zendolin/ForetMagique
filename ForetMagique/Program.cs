using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        GestionConsole gc = new GestionConsole();

        ForetMagique.IGForet ig = new ForetMagique.IGForet();

        ForetEnvironnement foret = new ForetEnvironnement(ig,gc);
        Agent agent = new Agent(foret,gc);
        foret.agent = agent;

        ig.foret = foret;

        agent.thread.Start();
        Application.EnableVisualStyles();
        Application.Run(ig);
        
    }
}

