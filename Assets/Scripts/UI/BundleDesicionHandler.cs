using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BundleDesicionHandler : MonoBehaviour
{
    public BundleCreator Bundles;

    public Button button1;
    public Button button2;
    public Button button3;

    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    public TextMeshProUGUI button3Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShowOptions();
    }

    void ShowOptions()
    {
        string[] nameArray1 = new string[Bundles.option1.Count];
        for (int i = 0; i < Bundles.option1.Count; i++) 
        {
            nameArray1[i] = Bundles.option1[i].name;
        }
        string[] nameArray2 = new string[Bundles.option2.Count];
        for (int i = 0; i < Bundles.option2.Count; i++)
        {
            nameArray2[i] = Bundles.option2[i].name;
        }
        string[] nameArray3 = new string[Bundles.option3.Count];
        for (int i = 0; i < Bundles.option3.Count; i++)
        {
            nameArray3[i] = Bundles.option3[i].name;
        }

        button1.enabled = true;
        button2.enabled = true;
        button3.enabled = true;

        button1Text.text = string.Join(", ", nameArray1);
        button2Text.text = string.Join(", ", nameArray2);
        button3Text.text = string.Join(", ", nameArray3);


    }
}
