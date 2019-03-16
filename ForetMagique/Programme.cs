using System;

namespace Aspirobot01
{
    class Programme
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(120, 40);

            GestionConsole gc = new GestionConsole(); // Gestion COnsole gère l'affichage de texte et des logs
            ManoirEnvironnement manoir = new ManoirEnvironnement(gc);
            AgentAspirateur aspirateur = new AgentAspirateur(manoir,gc);
            // L'utilisateur peut choisir le type d'exploration et la limite de profondeur de Depth-Search
            Console.WriteLine("Recherche non-informée(1) ou recherche informée(2)?");
            string reponse = Console.ReadLine();
            while(reponse != "1" && reponse != "2")
            {
                Console.WriteLine("Veuillez entrer 1 ou 2");
                reponse = Console.ReadLine();
            }
            if (reponse == "2") aspirateur.explorationInformee = true;
            else
            {
                bool parsed = false;
                Console.WriteLine("Limite de profondeur : ( très lent a partir de 7)");
                reponse = Console.ReadLine();
                int numValue;
                parsed = Int32.TryParse(reponse, out numValue);
                while (!parsed)
                {
                    Console.WriteLine("Veuillez entrer un entier uniquement");
                    reponse = Console.ReadLine();
                    parsed = Int32.TryParse(reponse, out numValue);
                }
                aspirateur.limite = numValue;
            }
            Console.WriteLine("Et pour finir , logger le texter dans un fichier (dossier de l'executable) ? Y / N");
            reponse = Console.ReadLine();
            while (reponse != "Y" && reponse != "y" && reponse != "N" && reponse != "n")
            {
                Console.WriteLine("Veuillez entrer Y/y ou N/n");
                reponse = Console.ReadLine();
            }
            if (reponse == "Y" || reponse == "y") gc.useLog = true;
            manoir.thread.Start();
            aspirateur.thread.Start();
        }
    }
}
