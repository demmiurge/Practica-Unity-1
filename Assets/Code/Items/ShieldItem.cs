public class ShieldItem : Item
{
    public float m_Shield;

    public override void Pick(FPSPlayerControllerV1 Player)
    {
        if (Player.GetShield() <= 0.0f)
        {
            Player.AddShield(m_Shield);
            gameObject.SetActive(false);
        }
    }
}
