using UnityEngine;

public class YmirVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] vfxAliento;
    [SerializeField] private ParticleSystem[] vfxArrastrar;
    [SerializeField] private ParticleSystem[] vfxAplastar;
    [SerializeField] private GameObject[] hitMarkets;

    public void ToggleVfxAliento(int estado)
    {
        //Se hace de esta forma porque los animation event no admiten bool
        //1 es activo, 0 inactivo
        bool activo = estado == 1 ? true : false;

        foreach (ParticleSystem vfx in vfxAliento)
        {
            if (activo)
                vfx.Play();
            else
                vfx.Stop();
        }
    }

    public void ToggleVfxArrastrar(int estado)
    {
        //Se hace de esta forma porque los animation event no admiten bool
        //1 es activo, 0 inactivo
        bool activo = estado == 1 ? true : false;

        foreach (ParticleSystem vfx in vfxArrastrar)
        {
            if (activo)
                vfx.Play();
            else
                vfx.Stop();
        }
    }

    public void StartVfxAplastar()
    {
        foreach (ParticleSystem vfx in vfxAplastar)
        {
            vfx.Play();
        }
    }

    public void ToggleHitMarket(int estado)
    {
        //Se hace de esta forma porque los animation event no admiten bool
        //1 es activo, 0 inactivo
        bool activo = estado == 1 ? true : false;
        
        if (activo)
        {
            foreach (GameObject market in hitMarkets)
            {
                market.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject market in hitMarkets)
            {
                market.SetActive(false);
            }
        }
    }
}