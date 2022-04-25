using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class spriteImporter : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;

    void OnEnable()
    {
        int counter = 0;
        foreach (Sprite sprite in sprites)
        {
            GameObject obj = new GameObject("Sprite " + counter);
            obj.AddComponent<SpriteRenderer>().sprite = sprites[counter];
            obj.transform.parent = transform;
            // Instantiate(obj, transform);
            counter ++;
        }
    }
}
