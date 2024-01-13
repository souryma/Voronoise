using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTextOnSliderValue : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Update is called once per frame
    public void OnSliderChange(float value)
    {
        text.text = value.ToString();
    }
}
