using UnityEngine;
using TMPro;

public class ManipulationUI : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] ListGenerator listGenerator;
    public void OnClickAddButton()
    {
        if (int.TryParse(inputField.text, out int countToAdd))
        {
            listGenerator.AddToList(countToAdd);
        }
    }
    public void OnClickRemoveButton()
    {
        listGenerator.RemoveFromList();
    }
}
