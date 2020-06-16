using UnityEngine;
using UnityEngine.UI;

public class DownInfoItem : MonoBehaviour
{

    private Text _downloadInfo;
    private Toggle _chooseState;

    private void Awake()
    {
        _downloadInfo = transform.Find("DownInfo").GetText();
        _chooseState = transform.Find("ToggleGroup/Toggle").GetComponent<Toggle>();
    }

    public void SetData()
    {
        
    }
}
