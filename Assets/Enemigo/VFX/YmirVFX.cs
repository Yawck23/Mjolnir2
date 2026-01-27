using UnityEngine;

public class YmirVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] vfxAliento;
    [SerializeField] private ParticleSystem[] vfxArrastrar;
    [SerializeField] private ParticleSystem[] vfxAplastar;

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
}