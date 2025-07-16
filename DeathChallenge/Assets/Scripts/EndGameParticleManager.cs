using UnityEngine;

public class EndGameParticleManager : MonoBehaviour
{
    [Header("Particle Systems")]
    public ParticleSystem hellParticles;
    public ParticleSystem overlayParticles;

    [Header("Settings")]
    public float particleDuration = 5f;
    public bool autoPlay = true;

    private void Start()
    {
        if (autoPlay)
        {
            PlayParticles();
        }
    }

    public void PlayParticles()
    {
        if (hellParticles != null)
        {
            ConfigureHellParticles();
            hellParticles.Play();
        }

        if (overlayParticles != null)
        {
            ConfigureOverlayParticles();
            overlayParticles.Play();
        }
    }

    private void ConfigureHellParticles()
    {
        var main = hellParticles.main;
        main.startColor = Color.red;
        main.startSpeed = 5f;
        main.startLifetime = particleDuration;
        main.maxParticles = 1000;

        var emission = hellParticles.emission;
        emission.rateOverTime = 10f;

        var shape = hellParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 1f;
    }

    private void ConfigureOverlayParticles()
    {
        var main = overlayParticles.main;
        main.startColor = Color.white;
        main.startSpeed = 3f;
        main.startLifetime = particleDuration;
        main.maxParticles = 500;

        var emission = overlayParticles.emission;
        emission.rateOverTime = 5f;

        var shape = overlayParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 2f;
    }

    public void StopParticles()
    {
        if (hellParticles != null)
            hellParticles.Stop();

        if (overlayParticles != null)
            overlayParticles.Stop();
    }
}