using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{

    private ListGenerator listGenerator;

    [SerializeField] TextMeshProUGUI label;
    [SerializeField] Toggle checkbox;
    private int curIndex;

    public void SetListHolder(ListGenerator listGenerator)
    {
        this.listGenerator = listGenerator;
    }

    public void UpdateContent(int index)
    {
        if (!listGenerator || index>= listGenerator.actualList.Count) return;
        curIndex = index;
        label.text = listGenerator.actualList[index].Item2;
        checkbox.isOn = listGenerator.actualList[index].Item1;
    }

    public void OnToggled(bool isOn)
    {
        if (!listGenerator) return;
        var item = listGenerator.actualList[curIndex];
        item.Item1 = isOn;
        listGenerator.actualList[curIndex] = item;
    }
}
