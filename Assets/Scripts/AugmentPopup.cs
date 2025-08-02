using System.Collections;
using UnityEngine;

public class AugmentPopup : MonoBehaviour
{
    public GameObject rarityDisplay;
    public GameObject DescriptionBox;
    public bool canLowerDone;
    bool popupReset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PopupFinish()
    {
        if (!popupReset)
            return;
        DescriptionBox.SetActive(true);
        rarityDisplay.SetActive(true);
        GetComponent<Animator>().ResetTrigger("lower");
        popupReset = false;
    }
    public void lower()
    {
        DescriptionBox.SetActive(false);
        rarityDisplay.SetActive(false);
        canLowerDone = true;
        GetComponent<Animator>().SetTrigger("lower");
    }
    public void lowerDone()
    {
        if(canLowerDone)
            gameObject.SetActive(false);
    }
    public IEnumerator waitCanLowerDone()
    {
        popupReset = true;
        yield return new WaitForSeconds(0.1f);
        canLowerDone = true;
    }
}
