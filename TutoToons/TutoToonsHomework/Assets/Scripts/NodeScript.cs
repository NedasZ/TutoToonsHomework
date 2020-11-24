using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    public GameObject text;
    public int Count = -1;
    private bool clicked = false;
    public GameObject level;
    public Sprite blueSprite;
    private bool setText = true;

    private Color alphaColor;
    private float timeToFade = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        alphaColor = text.GetComponent<TextMesh>().color;
        alphaColor.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Count != -1 && setText)
        {
            text.GetComponent<TextMesh>().text = "" + Count;
            setText = false;
        }

        if(clicked)
        {
            text.GetComponent<TextMesh>().color = Color.Lerp(text.GetComponent<TextMesh>().color, alphaColor, timeToFade * Time.deltaTime);
        }
    }

    void OnMouseDown()
    {
        if(level.GetComponent<GenerateLevel>().counter == Count)
        {
            this.GetComponent<SpriteRenderer>().sprite = blueSprite;
            clicked = true;
            level.GetComponent<GenerateLevel>().counter++;
        }
    }
}
