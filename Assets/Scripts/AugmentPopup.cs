using UnityEngine;

public class AugmentPopup : MonoBehaviour
{
    public GameObject rarityDisplay;
    public GameObject DescriptionBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void PopupFinish()
    {
        DescriptionBox.SetActive(true);
        rarityDisplay.SetActive(true);
    }
}
