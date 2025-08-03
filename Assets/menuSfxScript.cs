using UnityEngine;

public class menuSfxScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void playClickSfx() => AudioManager.instance.playMenuClick();
    public void playStartSfx() => AudioManager.instance.playMenuStart();
}
