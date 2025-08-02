using UnityEngine;

public class WindParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem frontParticles;
    [SerializeField] private ParticleSystem midParticles;
    [SerializeField] private ParticleSystem backParticles;
    [SerializeField] private float speedScaling = 1f;

    private void Update()
    {
        float simulationSpeed = GameManager.Instance.GetPlayerSpeed() * speedScaling;

        ParticleSystem.MainModule mainModule = frontParticles.main;
        mainModule.simulationSpeed = simulationSpeed;

        mainModule = midParticles.main;
        mainModule.simulationSpeed = simulationSpeed;

        mainModule = backParticles.main;
        mainModule.simulationSpeed = simulationSpeed;
    }
}
