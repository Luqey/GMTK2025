using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int negativeId;
    public string negativeDescription;
    public AugmentSelectionManager asm;
    public int index;
    public TMP_Text negativeText;
    public float lastTouchingTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (negativeId == -1)
        {
            negativeText.text = "Choose a downside";
        }
        else
        {
            negativeText.text = negativeDescription;
            negativeText.color = Color.red;
        }
    }
    public void Populate(int r, int i, string d, bool isApplying)
    {
        if (i == -1)
        {
            id = -1;
            return;
        }
        AugmentDataTransfer adt = GameObject.FindGameObjectWithTag("adt").GetComponent<AugmentDataTransfer>();
        adt.ids[index] = i;
        adt.rarityDisps[index] = r;
        rarity = r;
        id = i;
        description = d;
        Debug.Log(adt.negativeids[index]);
        rarityDisplay.SetActive(true);
        rarityDisplay.GetComponent<SpriteRenderer>().sprite = rarityLabels[rarity];
        rarityDisplay2.GetComponent<SpriteRenderer>().sprite = rarityLabels2[rarity];
        DescriptionBox.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = description;
        popup.SetActive(true);
        popup.GetComponent<AugmentPopup>().canLowerDone = false;
        popup.GetComponent<AugmentPopup>().StartCoroutine(popup.GetComponent<AugmentPopup>().waitCanLowerDone());
        if (isApplying)
        {
            adt.negativeids[index] = -1;
            SceneManager.LoadScene(1);
        }
        else
        {
            asm.regenerateNegativeOptions(this);
        }
        StartCoroutine(waitToLower());
    }

    public void updateNegative(int i, string d)
    {
        negativeId = i;
        negativeDescription = d;
        AugmentDataTransfer adt = GameObject.FindGameObjectWithTag("adt").GetComponent<AugmentDataTransfer>();
        adt.negativeids[index] = i;
    }
    public void show()
    {
        if (id == -1)
            return;
        if (popup.activeSelf)
            return;
        popup.SetActive(true);
        popup.GetComponent<AugmentPopup>().canLowerDone = false;
        popup.GetComponent<AugmentPopup>().StartCoroutine(popup.GetComponent<AugmentPopup>().waitCanLowerDone());
        StartCoroutine(waitToLower());
    }

    IEnumerator waitToLower()
    {
        yield return new WaitForSeconds(1.5f);
        if (Mathf.Abs(Time.time - lastTouchingTime) < 0.1f)
            StartCoroutine(waitToLower());
        else
            popup.GetComponent<AugmentPopup>().lower();
    }
}
