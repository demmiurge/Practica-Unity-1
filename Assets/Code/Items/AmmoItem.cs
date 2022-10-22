public class AmmoItem : Item
{
    public int m_Ammo;

    public override void Pick(FPSPlayerControllerV1 Player)
    {
        if(Player.GetAmmo() < 50f)
        {
            Player.AddAmmo(m_Ammo);
            gameObject.SetActive(false);
        }
    }
}
