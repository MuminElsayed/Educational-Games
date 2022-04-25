using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class move : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed, minSpeed, maxSpeed;
    [SerializeField]
    [Range(-1,1)]
    private float minHeight, maxHeight;
    [SerializeField]
    private Vector3 moveDirection;
    [SerializeField]
    private bool changeSprite;
    private Image image;
    private int totalSprites;

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed, Space.World);
        image = GetComponent<Image>();
        totalSprites = GuessABC.instance.fishSprites.Length;
    }

    void OnTriggerEnter2D()
    {
        // transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        moveSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        // Vector3 randomScreenEdge = new Vector3 ((Screen.width / 2f + 100) * -moveDirection.x, Random.Range(-Screen.height / 2f + 100, Screen.height / 2f - 100), 0);
        Vector3 randomScreenEdge = new Vector3 ((Screen.width / 2f + 100) * -moveDirection.x, Random.Range(-Screen.height / 2f * minHeight * -1 + 100, Screen.height / 2f * maxHeight - 100), 0);
        transform.parent.localPosition = randomScreenEdge;
        transform.localPosition = Vector3.zero;
        if (changeSprite)
        {
            image.sprite = GuessABC.instance.fishSprites[Random.Range(0, totalSprites)];
        }
    }
}