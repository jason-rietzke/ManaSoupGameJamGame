using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Image TutPfeil;
    public CanvasGroup TextHintergrund;
    public GameObject button;
    public Animator animTextBG;


    public void TutorialEnd()
    {
        TutPfeil.enabled = false;
        animTextBG.SetTrigger("FadeOut");
        button.SetActive(false);
    }



}
