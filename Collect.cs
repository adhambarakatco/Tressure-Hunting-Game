using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Vuforia;

public class Collect : MonoBehaviour
{
    public GameObject gameObject1;
    public GameObject gameObject2;
    public GameObject gameObject3;
    public GameObject gameObject4;
    public GameObject gameObject5;
    public GameObject gameObject6;
    public GameObject gameObject7;
    public GameObject gameObject8;
    public TMPro.TextMeshProUGUI counterText;
    public TMPro.TextMeshProUGUI message;
    public AudioSource treasureSound;
    public AudioSource sharkSound;
    public AudioSource gunSound;
    public UnityEngine.UI.Button interactButton;
    public UnityEngine.UI.Button exitButton; 

    public static Dictionary<GameObject, int> targetCounts = new Dictionary<GameObject, int>();

    void Start()
    {
        interactButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false); 
        if (treasureSound != null)
        {
            treasureSound.playOnAwake = false; 
        }

        
        exitButton.onClick.AddListener(ExitApplication);
    }

    void Update()
    {
        UpdateInteractButtonVisibility();
    }

    public void interact()
    {
        var trackedObject = DetectTrackedObject();

        if (trackedObject != null)
        {
            string tmp = trackedObject.name;
            if (tmp[0] == 'c' && tmp[1] == 'l' && tmp[2] == 'u' && tmp[3] == 'e')
            {
                if (tmp[4] == '1')
                {
                    message.text = "The sword of the brave warrior destroyed the ground";
                }
                else if (tmp[4] == '2')
                {
                    message.text = "These towers once were in most popular city in the US";
                }
                else if (tmp[4] == '3')
                {
                    message.text = "4,500 years, and this monument still stands";
                }
                else if (tmp[4] == '4')
                {
                    message.text = "WOAH, cool gun";
                    gunSound.Play();
                }
                else if (tmp[4] == '5')
                {
                    message.text = "AHHHHHHH SHARK!!!!!!";
                    sharkSound.Play();
                }
                StartCoroutine(ClearMessageAfterDelay());
            }
            else
            {
                targetCounts.Add(trackedObject, 0);
                counterText.text = "Treasures found: " + targetCounts.Count.ToString();
                trackedObject.SetActive(false);
                StartCoroutine(ShowTreasureFoundMessage());

                if (targetCounts.Count >= 3)
                {
                    message.text = "YOU FOUND ALL THE HIDDEN TREASURES!";
                    exitButton.gameObject.SetActive(true); 
                    
                }
            }
        }
    }

    private GameObject DetectTrackedObject()
    {
        ObserverBehaviour[] observers = FindObjectsOfType<ObserverBehaviour>();

        foreach (var observer in observers)
        {
            if (observer.gameObject.name == "ARCamera")
                continue;

            if (observer.TargetStatus.Status == Status.TRACKED || observer.TargetStatus.Status == Status.EXTENDED_TRACKED)
            {
                return observer.gameObject;
            }
        }
        return null;
    }

    private void UpdateInteractButtonVisibility()
    {
        GameObject trackedObject = DetectTrackedObject();
        interactButton.gameObject.SetActive(trackedObject != null);
    }

    private IEnumerator ShowTreasureFoundMessage()
    {
        message.text = "TREASURE FOUND!";
        treasureSound.Play();
        yield return new WaitForSeconds(1);
        message.text = "";
    }

    private IEnumerator ClearMessageAfterDelay()
    {
        yield return new WaitForSeconds(2);
        message.text = "";
    }

    private void ExitApplication()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
