using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void PlayVFX(GameObject effectObject, Vector3 effectPosition)
    {
        GameObject vfxGameObject = Instantiate(
            effectObject, 
            effectPosition, 
            Quaternion.identity
        );

        ParticleSystem[] particleSystems = vfxGameObject.GetComponentsInChildren<ParticleSystem>();

        float maxlength = 0f;

        foreach (ParticleSystem individualParticleSystem in particleSystems)
        {
            float currentKnowMaxlength = individualParticleSystem.main.duration + 
            individualParticleSystem.main.startLifetime.constantMax;

            if (currentKnowMaxlength > maxlength)
                maxlength = currentKnowMaxlength;
        }

        Destroy(vfxGameObject, maxlength);

    }
}
