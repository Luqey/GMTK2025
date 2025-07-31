using TMPro;
using UnityEngine;

public class AugmentSlot : MonoBehaviour
{
    public int rarity;
    public int id;
    public string description;
    public GameObject rarityDisplay;
    public Sprite[] rarityLabels;
    public GameObject rarityDisplay2;
    public Sprite[] rarityLabels2;
    public GameObject DescriptionBox;
    public GameObject popup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Populate(int r, int i, string d)
    {
        rarity = r;
        id = i;
        description = d;
        rarityDisplay.SetActive(true);
        rarityDisplay.GetComponent<SpriteRenderer>().sprite = rarityLabels[rarity];
        rarityDisplay2.GetComponent<SpriteRenderer>().sprite = rarityLabels2[rarity];
        DescriptionBox.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = description;
        popup.SetActive(true);
    }
}
