using System.Security.Cryptography;
using UnityEngine;

public class PlayerSfx : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepSfx;
    [SerializeField] private AudioClip[] jumpSfx;
    [SerializeField] private AudioClip[] landingSfx;
    [SerializeField] private AudioClip[] slideSfx;
    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSource2;

    void Update()
    {
        audioSource1.volume = AudioManager.instance.updateVolume();
        audioSource2.volume = AudioManager.instance.updateVolume();
    }

    public void playFootstep()
    {
        if (footstepSfx.Length == 0) return;
        int rand = RandomNumberGenerator.GetInt32(0, footstepSfx.Length);
        audioSource1.clip = footstepSfx[rand];
        audioSource1.Play();
    }

    public void playJump()
    {
        if (jumpSfx.Length == 0) return;
        int rand = RandomNumberGenerator.GetInt32(0, jumpSfx.Length);
        audioSource2.clip = jumpSfx[rand];
        audioSource2.Play();
    }
    public void playLand()
    {
        if (landingSfx.Length == 0) return;
        int rand = RandomNumberGenerator.GetInt32(0, landingSfx.Length);
        audioSource2.clip = landingSfx[rand];
        audioSource2.Play();
    }
    public void playSlide()
    {
        if (slideSfx.Length == 0) return;
        int rand = RandomNumberGenerator.GetInt32(0, slideSfx.Length);
        audioSource1.clip = slideSfx[rand];
        audioSource1.Play();
    }
}
