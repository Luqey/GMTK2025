using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AugmentLoopUIManager : MonoBehaviour
{
    public List<AugmentSlot> aslots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(setup());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator setup()
    {
        yield return new WaitForFixedUpdate();
        AugmentDataTransfer adt = GameObject.FindGameObjectWithTag("adt").GetComponent<AugmentDataTransfer>();
        string readFromFilePath = Application.streamingAssetsPath + "\\AugmentData.txt";
        List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();
        for (int i = 0; i < aslots.Count; i++)
        {
            Debug.Log(aslots.Count + "," + adt.rarityDisps.Count + "," + adt.ids.Count + "," + fileLines.Count);
            if (adt.ids[i] != -1)
            {
                aslots[i].Populate(adt.rarityDisps[i], adt.ids[i], fileLines[adt.ids[i]].Substring(0, fileLines[adt.ids[i]].IndexOf('|')));
                aslots[i].negativeId = adt.negativeids[i];
            }
            else
            {
                aslots[i].Populate(-2, -1, "null");
            }
        }
    }
}
