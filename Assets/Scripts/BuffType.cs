using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class BuffType : MonoBehaviour
{
    private string buffName = "";
    private float speedMultiplier = 1f;
    private float jumpMultiplier = 1f;
    private float accelMultiplier = 1f;
    private float gravityMultiplier = 1f;
    private bool dash = false;
    private bool intervalOnly = false;
    private bool isActive = false;
    private Image sprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<Image>();
    }
    //apply buff to the player
    public void applyBuff(PlayerScript player, float interval)
    {
        isActive = true;
        Debug.Log(buffName + " activated!");
        sprite.color = Color.yellow;
        player.setMultipliers(speedMultiplier, jumpMultiplier, gravityMultiplier);
        if (intervalOnly) StartCoroutine(intervalBuffTimer(player, interval));
        player.setDash(dash);
    }

    public void setBuff(int id)
    {
        string readFromFilePath = Application.streamingAssetsPath + "\\AugmentData.txt";
        List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();

        for (int i = 0; i < fileLines.Count; i++)
        {
            if (i == id)
            {
                buffName = fileLines[i].Substring(0, fileLines[i].IndexOf('|'));
                fileLines[i] = fileLines[i].Substring(fileLines[i].IndexOf('|') + 1);
                while (fileLines[i].Length > 1)
                {
                    if (fileLines[i].IndexOf(',') != -1)
                    {
                        string argOverride = fileLines[i].Substring(0, fileLines[i].IndexOf(','));
                        switch (argOverride.Substring(0, 4))
                        {
                            case "sMult":
                                speedMultiplier = float.Parse(argOverride.Substring(7));
                                break;
                            case "jMult":
                                jumpMultiplier = float.Parse(argOverride.Substring(7));
                                break;
                            case "aMult":
                                accelMultiplier = float.Parse(argOverride.Substring(7));
                                break;
                            case "gMult":
                                gravityMultiplier = float.Parse(argOverride.Substring(7));
                                break;
                            case "dash":
                                dash = bool.Parse(argOverride.Substring(7));
                                break;
                            case "intO":
                                intervalOnly = bool.Parse(argOverride.Substring(7));
                                break;
                        }
                        fileLines[i] = fileLines[i].Substring(fileLines[i].IndexOf(',') + 1);
                    }
                    else
                    {
                        string argOverride = fileLines[i].Substring(0, fileLines[i].IndexOf(';'));
                        switch (argOverride.Substring(0, 4))
                        {
                            case "sMlt":
                                speedMultiplier = float.Parse(argOverride.Substring(7));
                                break;
                            case "jMlt":
                                jumpMultiplier = float.Parse(argOverride.Substring(7));
                                break;
                            case "aMlt":
                                accelMultiplier = float.Parse(argOverride.Substring(7));
                                break;
                            case "gMlt":
                                gravityMultiplier = float.Parse(argOverride.Substring(7));
                                break;
                            case "dash":
                                dash = bool.Parse(argOverride.Substring(7));
                                break;
                            case "intO":
                                intervalOnly = bool.Parse(argOverride.Substring(7));
                                break;
                        }
                        fileLines[i] = "";
                    }
                }
            }
        }
    }

    //Sets all variables to default values; use when removing an augment from a slot.
    public void factoryReset()
    {
        buffName = "";
        speedMultiplier = 1f;
        jumpMultiplier = 1f;
        accelMultiplier = 1f;
        gravityMultiplier = 1f;
        dash = false;
        intervalOnly = false;
        isActive = false;
    }

    public void deactivate(PlayerScript player)
    {
        if (isActive)
        {
            sprite.color = Color.white;
            player.removeMultipliers(speedMultiplier, jumpMultiplier, gravityMultiplier);
            isActive = false;
        }
    }

    private IEnumerator intervalBuffTimer(PlayerScript player, float interval)
    {
        yield return new WaitForSeconds(interval);
        deactivate(player);
    }
    private IEnumerator applyDash(PlayerScript player)
    {
        player.setDash(true);
        yield return new WaitForSeconds(0.5f);
        player.setDash(false);
    }
}
