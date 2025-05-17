using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RandomEventHandler : MonoBehaviour
{
    public TextMeshProUGUI mainText;
    public Button[] buttons;
    public TextMeshProUGUI[] buttonText;
    public List<string> eventNames;
    public List<string> button0Text;
    public List<string> button1Text;
    public List<string> button2Text;
    public List<List<GameObject>> button0Option;
    public List<List<GameObject>> button1Option;
    public List<List<GameObject>> button2Option;

    // Start is called before the first frame update
    void Start()
    {
        RandomEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomEvent()
    {
        int i = Random.Range(0, eventNames.Count);

        mainText.text = eventNames[i];

        buttonText[0].text = button0Text[i];
        buttonText[1].text = button1Text[i];
        buttonText[2].text = button2Text[i];
    }
}
