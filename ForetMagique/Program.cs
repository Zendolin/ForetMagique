using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    ForetEnvironnement foret;
    Agent agent;


    public Program()
    {
        foret = new ForetEnvironnement();
        agent = new Agent(foret);
    }

    static void Main(string[] args)
    {

    }
}

