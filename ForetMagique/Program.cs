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
        Console.Write("Debut");

        ForetMagique.IGForet ig = new ForetMagique.IGForet();

        ForetEnvironnement foret = new ForetEnvironnement(ig);
        Agent agent = new Agent(foret);

        Console.Write("ok");
        foret.thread.Start();

        Application.EnableVisualStyles();
        Application.Run(ig);
    }
}

