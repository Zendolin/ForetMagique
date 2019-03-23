using System;
using System.Collections.Generic;
using System.IO;

public class GestionConsole // Cette classe permet d'afficher le manoir en ascii et du texte en dessous
{
    List<string> texteConsole;
    public bool useLog;
    String s ="";


    public GestionConsole()
	{
        texteConsole = new List<string>();
        s = "log" + DateTime.Now.ToLongTimeString() + ".txt";
        s = s.Replace(':', '_');
    }

    public void AddConsole(string txt) // Pour afficher du texte sans effacer le manoir
    {   
        if(useLog)
        {
            using (StreamWriter w = File.AppendText(s))
            {
                Log(txt, w);
            }
        }
        
        texteConsole.Add(txt);
        if (texteConsole.Count > 26)
        {
            texteConsole.RemoveAt(0);
        }
    }

    public void Write()
    {
        List<string> texteConsoleCopie = new List<string>(texteConsole);
        texteConsoleCopie.ForEach(txt => Console.WriteLine(txt));
    }

    public static void Log(string logMessage, TextWriter w) // on peut logger le texte dans un fichier pour relire après éxécution
    {
        w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}" +" : " +  logMessage);
    }
}
