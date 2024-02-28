using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ListGenerator : MonoBehaviour
{
    private const string urlBase = "https://random-word-api.herokuapp.com/word?number=";
    string[] words;
    [SerializeField] private int initialListCount; //to prevent reusing words
    [SerializeField] private ListManager listManager;
    public List<(bool, string)> actualList { get; private set; }



    void Start()
    {
        GetWordsList(initialListCount, () =>
        {
            actualList = GenerateList(initialListCount);
            listManager.InitializeList(this);
        });

    }

    void GetWordsList(int amount, Action OnDoneAction)
    {
        string url = urlBase + amount;
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SendWebRequest().completed += operation =>
        {
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch Pokemon list: " + www.error + url);
                return;
            }

            // Deserialize the JSON array directly into a string array
            words = JsonConvert.DeserializeObject<string[]>(www.downloadHandler.text);
            OnDoneAction?.Invoke();
            www.Dispose();
        };
    }

    public List<(bool, string)> GenerateList(int count)
    {

        List<(bool, string)> generatedList = new List<(bool, string)>();
        for (int i = 0; i < count; i++)
        {
            generatedList.Add((false, words[i]));
        }

        return generatedList;
    }

    public void AddToList(int countToAdd)
    {
        GetWordsList(countToAdd, () =>
        {
            actualList.AddRange(GenerateList(countToAdd));
            listManager.ChangeListSize(actualList.Count);
        });
    }

    public void RemoveFromList()
    {
        List<(bool, string)> newList = new List<(bool, string)>();

        foreach (var element in actualList)
        {
            if (!element.Item1)
            {
                newList.Add(element);
            }
        }

        // Replace the original list with the new one
        actualList = newList;

        listManager.ChangeListSize(actualList.Count);
    }

}
