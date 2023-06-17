using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterScript : MonoBehaviour
{
    public MeshRenderer Mesh;
    public Sprite CapitalAlphabetUI;
    public Sprite SmallAlphabetUI;
    // Start is called before the first frame update
    void Start()
    {
        //ShowLetter(false);
        Mesh.enabled = false;
    }

    /*    public void ShowLetter(bool SHOW)
        {
            Mesh.enabled = SHOW;
        }*/

    public void DisplayUI()
    {
        if (MasterScript.instance.TryGetComponent(out AlphabetUI AUI) && CapitalAlphabetUI != null && SmallAlphabetUI != null)
        {
            AUI.CapitalAlphabetRect.GetComponent<Image>().sprite = CapitalAlphabetUI;
            AUI.SmallAlphabetRect.GetComponent<Image>().sprite = SmallAlphabetUI;
            AUI.AnimationSequence();
        }
    }

    public void HideUI()
    {
        if (MasterScript.instance.TryGetComponent(out AlphabetUI AUI) && CapitalAlphabetUI != null && SmallAlphabetUI != null)
        {
            AUI.HideUI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
