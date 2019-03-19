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

        ForetEnvironnement foret = new ForetEnvironnement();
        Agent agent = new Agent(foret);

        Console.Write("ok");
        foret.thread.Start();

        Application.EnableVisualStyles();
        Application.Run(new ForetMagique.IGForet());
    }
}

