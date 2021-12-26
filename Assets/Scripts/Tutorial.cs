using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Image TutPfeil;
    [SerializeField] private CanvasGroup TextHintergrund;
    [SerializeField] private GameObject button;
    [SerializeField] private Animator animTextBG;

    public void TutorialEnd()
    {
        TutPfeil.enabled = false;
        animTextBG.SetTrigger("FadeOut");
        button.SetActive(false);
    }
}
