using System.Collections.Generic;
using UnityEngine;

public class AugmentSelectionManager : MonoBehaviour
{
    public List<Augment> possibleAugments;
    public List<Augment> possibleNegatives;

    public GameObject option1;
    public GameObject option2;
    public GameObject option3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        possibleAugments = new List<Augment>() { new Augment(0, 0, 100, "Speed I"), new Augment(1, 1, 30, "Speed II"), new Augment(2, 2, 8, "Spring Shoes"), new Augment(3, 2, 3, "Athletic") };
        possibleNegatives = new List<Augment>() { new Augment(8, -1, 100, "test negative"), new Augment(8, -1, 30, "test negative (internally rare)"), new Augment(8, -1, 8, "test negative (internally legendary)"), new Augment(8, -1, 3, "test negative (internally absurdly rare)") };
        StartCoroutine(waitToRegen());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void regenerateAugmentOptions()
    {
        for (int i = 0; i < 3; i++)
        {
            float iRFactorSum = 0;
            for (int o = 0; o < possibleAugments.Count; o++)
            {
                iRFactorSum += possibleAugments[o].inverseRarityFactor;
            }
            float selection = Random.Range(0, iRFactorSum);
            Augment selected = new Augment(0, 0, 0, "error: no augment selected");
            for (int o = 0; o < possibleAugments.Count; o++)
            {
                selection -= possibleAugments[o].inverseRarityFactor;
                if (selection < 0)
                {
                    selected = possibleAugments[o];
                    o = possibleAugments.Count;
                }
            }
            switch (i)
            {
                case 0:
                    option1.GetComponent<AugmentOption>().id = selected.id;
                    option1.GetComponent<AugmentOption>().rarity = selected.displayRarity;
                    option1.GetComponent<AugmentOption>().description = selected.description;
                    option1.GetComponent<AugmentOption>().regenerateParts();
                    break;
                case 1:
                    option2.GetComponent<AugmentOption>().id = selected.id;
                    option2.GetComponent<AugmentOption>().rarity = selected.displayRarity;
                    option2.GetComponent<AugmentOption>().description = selected.description;
                    option2.GetComponent<AugmentOption>().regenerateParts();
                    break;
                case 2:
                    option3.GetComponent<AugmentOption>().id = selected.id;
                    option3.GetComponent<AugmentOption>().rarity = selected.displayRarity;
                    option3.GetComponent<AugmentOption>().description = selected.description;
                    option3.GetComponent<AugmentOption>().regenerateParts();
                    break;
            }
        }
    }

    public void regenerateNegativeOptions(AugmentSlot augS)
    {
        for (int i = 0; i < 3; i++)
        {
            float iRFactorSum = 0;
            for (int o = 0; o < possibleNegatives.Count; o++)
            {
                iRFactorSum += possibleNegatives[o].inverseRarityFactor;
            }
            float selection = Random.Range(0, iRFactorSum);
            Augment selected = new Augment(0, 0, 0, "error: no augment selected");
            for (int o = 0; o < possibleNegatives.Count; o++)
            {
                selection -= possibleNegatives[o].inverseRarityFactor;
                if (selection < 0)
                {
                    selected = possibleNegatives[o];
                    o = possibleNegatives.Count;
                }
            }
            switch (i)
            {
                case 0:
                    option1.SetActive(true);
                    option1.GetComponent<AugmentOption>().id = selected.id;
                    option1.GetComponent<AugmentOption>().rarity = -1;
                    option1.GetComponent<AugmentOption>().description = selected.description;
                    option1.GetComponent<AugmentOption>().slotForNegative = augS;
                    option1.GetComponent<AugmentOption>().regenerateParts();
                    break;
                case 1:
                    option2.SetActive(true);
                    option2.GetComponent<AugmentOption>().id = selected.id;
                    option2.GetComponent<AugmentOption>().rarity = -1;
                    option2.GetComponent<AugmentOption>().description = selected.description;
                    option2.GetComponent<AugmentOption>().slotForNegative = augS;
                    option2.GetComponent<AugmentOption>().regenerateParts();
                    break;
                case 2:
                    option3.SetActive(true);
                    option3.GetComponent<AugmentOption>().id = selected.id;
                    option3.GetComponent<AugmentOption>().rarity = -1;
                    option3.GetComponent<AugmentOption>().description = selected.description;
                    option3.GetComponent<AugmentOption>().slotForNegative = augS;
                    option3.GetComponent<AugmentOption>().regenerateParts();
                    break;
            }
        }
    }

    private System.Collections.IEnumerator waitToRegen()
    {
        yield return new WaitForSeconds(0.2f);
        regenerateAugmentOptions();
    }
}

public class Augment
{
    public int id;
    public int displayRarity;
    // higher number = more common
    public float inverseRarityFactor;
    public string description;

    public Augment(int i, int dR, float iRF, string d)
    {
        id = i;
        displayRarity = dR;
        inverseRarityFactor = iRF;
        description = d;
    }
}