using System;

public class Effecteur // Classe qui agit sur le manoir , déplace l'aspirateur et nettoye les salles
{
    ForetEnvironnement foret;

    public Effecteur(ForetEnvironnement f)
	{
        foret = f;
    }

    public void DeplacerGauche()
    {
        foret.DeplacementAgent("gauche");
    }
    public void DeplacerDroite()
    {
        foret.DeplacementAgent("droite");
    }
    public void DeplacerHaut()
    {
        foret.DeplacementAgent("haut");
    }
    public void DeplacerBas()
    {
        foret.DeplacementAgent("bas");
    }

    public void AllerSur(int x ,int y)
    {
        foret.AllerSur(x, y);
    }

    public void LancerCaillou(Zone z)
    {
        foret.LancerCaillou(z);
    }

    public void PasserPortail()
    {
        foret.PasserPortail();
    }
}
