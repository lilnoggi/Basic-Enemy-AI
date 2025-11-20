using TMPro;
using UnityEngine;

public class ShowPillarNumber : MonoBehaviour
{
    public TextMeshProUGUI pillarName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pillarName.text = gameObject.name;
    }
}
