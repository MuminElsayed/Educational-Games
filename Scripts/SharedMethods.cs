using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImageClass
{
    public string name;
    public AudioClip[] audioClips;
    public Sprite sprite;
    public Vector2 spriteScale = Vector2.one;
}

[System.Serializable]
public class SharedMethods : MonoBehaviour
{
    public static SharedMethods instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public List<E> ShuffleList<E>(List<E> inputList)
    {
        List<E> currentList = new List<E>(inputList);
        List<E> randomList = new List<E>();

        System.Random r = new System.Random();
        int randomIndex = 0;
        while (currentList.Count > 0)
        {
            randomIndex = r.Next(0, currentList.Count); //Choose a random object in the list
            randomList.Add(currentList[randomIndex]); //add it to the new, random list
            currentList.RemoveAt(randomIndex); //remove to avoid duplicates
        }

        return randomList; //return the new random list
    }
}
