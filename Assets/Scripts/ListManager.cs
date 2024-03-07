using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListManager : MonoBehaviour
{
    public RectTransform Container;
    public Mask Mask;
    public ListItem Prefab;
    public int Num = 1;
    public float Spacing;

    private RectTransform maskRT;
    private int numVisible;
    private int numBuffer = 2;
    private float containerHalfSize;
    private float prefabSize;
    [SerializeField] private Scrollbar scrollbar;

    private List<RectTransform> listItemRect = new List<RectTransform>();
    private List<ListItem> listItems = new List<ListItem>();
    private int numItems = 0;
    private Vector3 startPos;
    private Vector3 offsetVec;
    private float lastFloatPos;

    public void InitializeList(ListGenerator listGenerator)
    {
        Container.anchoredPosition3D = new Vector3(0, 0, 0);
        Num = listGenerator.actualList.Count;
        maskRT = Mask.GetComponent<RectTransform>();

        Vector2 prefabScale = Prefab.GetComponent<RectTransform>().rect.size;
        prefabSize = prefabScale.y + Spacing;

        Container.sizeDelta = new Vector2(prefabScale.x, prefabSize * Num);
        containerHalfSize = Container.rect.size.y * 0.5f;

        numVisible = Mathf.CeilToInt(maskRT.rect.size.y / prefabSize);

        offsetVec = Vector3.down;
        startPos = Container.anchoredPosition3D - (offsetVec * containerHalfSize) + (offsetVec * prefabScale.y * 0.5f);
        numItems = Mathf.Min(Num, numVisible + numBuffer);
        for (int i = 0; i < numItems; i++)
        {
            ListItem obj = Instantiate(Prefab, Container.transform);
            RectTransform t = obj.GetComponent<RectTransform>();
            t.anchoredPosition3D = startPos + (offsetVec * i * prefabSize);
            listItemRect.Add(t);
            obj.gameObject.SetActive(true);

            listItems.Add(obj);
            obj.SetListHolder(listGenerator);
            obj.UpdateContent(i);
        }
        Container.anchoredPosition3D += offsetVec * (containerHalfSize - (maskRT.rect.size.y * 0.5f));
    }

    public void ChangeListSize(int newCount)
    {
        int difference = newCount - Num;
        Num = newCount;
        Container.sizeDelta = new Vector2(Prefab.GetComponent<RectTransform>().rect.size.x, prefabSize * Num);
        containerHalfSize = Container.rect.size.y * 0.5f;
        startPos = -(offsetVec * containerHalfSize) + (0.5f * Prefab.GetComponent<RectTransform>().rect.size.y * offsetVec);
        Container.anchoredPosition3D += offsetVec * (prefabSize * 0.5f * difference);
        ReorderItemsByPos(lastFloatPos);
    }


    public void ReorderItemsByPos(float normPos)
    {
        if (numItems == 0) return;
        lastFloatPos = normPos;
        normPos = 1f - normPos;
        int numOutOfView = Mathf.CeilToInt(normPos * (Num - numVisible));
        int firstIndex = Mathf.Max(0, numOutOfView - numBuffer);
        int originalIndex = firstIndex % numItems;

        int newIndex = firstIndex;

        for (int i = 0; i < numItems; i++)
        {
            if (newIndex >= Num) break;

            int currentIndex = (originalIndex + i) % numItems;
            moveItemByIndex(listItemRect[currentIndex], newIndex);
            listItems[currentIndex].UpdateContent(newIndex);
            newIndex++;
        }

    }

    private void moveItemByIndex(RectTransform item, int index)
    {
        item.anchoredPosition3D = startPos + (offsetVec * index * prefabSize);
    }
}
