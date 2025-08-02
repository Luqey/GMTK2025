using System.Security.Cryptography;
using UnityEngine;

public class PlayerSfx : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepSfx;
    [SerializeField] private AudioClip[] jumpSfx;
    [SerializeField] private AudioClip[] landingSfx;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playFootstep()
    {
        if (footstepSfx.Length == 0) return;
        int rand = RandomNumberGenerator.GetInt32(0, footstepSfx.Length);
        audioSource.clip = footstepSfx[rand];
        audioSource.Play();
    }

    public void playJump()
    {
        if (jumpSfx.Length == 0) return;
        int rand = RandomNumberGenerator.GetInt32(0, jumpSfx.Length);
        audioSource.clip = jumpSfx[rand];
        audioSource.Play();
    }
    public void playLand()
    {
        if (landingSfx.Length == 0) return;
        int rand = RandomNumberGenerator.GetInt32(0, landingSfx.Length);
        audioSource.clip = landingSfx[rand];
        audioSource.Play();
    }
}
