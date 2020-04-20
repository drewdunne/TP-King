using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShoppingCart : MonoBehaviour
{
    private string loadLocation = "ShoppingCart";
    private Image imageComponent;
    private Sprite[] sprites;
    public GameObject scoreText;
    public GameObject smallScoreText;


    private void Start()
    {
        imageComponent = gameObject.GetComponent<Image>();
        sprites = Resources.LoadAll<Sprite>(loadLocation);

        Debug.Log("Cart Sprite Loading complete...");

        foreach (var sprite in sprites)
        {
            Debug.Log(sprite.name);
        }
    }

    public void UpdateShoppingCart(int score)
    {
        if(score <= 23)
        {
            scoreText.SetActive(false);
            smallScoreText.SetActive(true);
            imageComponent.sprite = sprites[score];
            smallScoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
        }

        if(score > 23)
        {
            imageComponent.sprite = sprites[sprites.Length -1];
            scoreText.SetActive(true);
            smallScoreText.SetActive(false);
            scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
        }

    }


}
