using System.Collections;
using UnityEngine;
using TMPro;

public class AugmentOption : MonoBehaviour
{
    public int rarity;
    public int id;
    public string description;
    public GameObject rarityDisplay;
    public Sprite[] rarityLabels;
    public GameObject draggablePrefab;
    public AugmentOption[] otherOptions;
    Animator anim;
    public GameObject DescriptionBox;
    public int framesToShowDescription;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rarityDisplay.GetComponent<SpriteRenderer>().sprite = rarityLabels[rarity];
        anim = GetComponent<Animator>();
        DescriptionBox.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = description;
        // clicked();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        DescriptionBox.SetActive(framesToShowDescription >= 0);
        framesToShowDescription--;
    }

    public void clicked()
    {
        anim.SetTrigger("click");
        StartCoroutine(fadeOutRarityAndDescriptionCoroutine());
        foreach (AugmentOption o in otherOptions)
        {
            o.fadeOut();
        }
    }
    public void endAnim()
    {
        GameObject g = Instantiate(draggablePrefab, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        g.GetComponent<DragAndDropAugment>().id = id;
        g.GetComponent<DragAndDropAugment>().rarity = rarity;
        g.GetComponent<DragAndDropAugment>().description = description;
        gameObject.SetActive(false);
    }
    public void fadeOut()
    {
        StartCoroutine(fadeOutCoroutine());
    }

    IEnumerator fadeOutCoroutine()
    {
        for (int i = 0; i < 30; i++)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (30 - i) / 30.0f);
            rarityDisplay.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (30 - i) / 30.0f);
            transform.localScale = new Vector3(16 - 8 * (i / 30.0f), 16 + 4 * (i / 30.0f), 16);
            yield return new WaitForFixedUpdate();
        }
        gameObject.SetActive(false);
    }

    IEnumerator fadeOutRarityAndDescriptionCoroutine()
    {
        for (int i = 0; i < 20; i++)
        {
            rarityDisplay.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (20 - i) / 20.0f);
            DescriptionBox.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (20 - i) / 20.0f);
            DescriptionBox.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().color = new Color(1, 1, 1, (20 - i) / 20.0f);
            yield return new WaitForFixedUpdate();
        }
        rarityDisplay.SetActive(false);
    }
}
