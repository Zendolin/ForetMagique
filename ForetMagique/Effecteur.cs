using System;

public class Effecteur // Classe qui agit sur le manoir , déplace l'aspirateur et nettoye les salles
{
    ForetEnvironnement foret;

    public Effecteur(ForetEnvironnement f)
	{
        foret = f;
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

    public void DessinerZone(Zone z)
    {
        foret.DessinerZone(z);
    }
    public void DessinerForet()
    {
        foret.DessinerForet();
    }
}
