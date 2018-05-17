using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSweet : MonoBehaviour {

	public enum ColorType
    {
        YELLOW,
        PURPLE,
        RED,
        BLUE,
        GREEN,
        PINK,
        ANY,
        COUNT
    }

    //字典
    private Dictionary<ColorType, Sprite> colorSpriteDict;
    //渲染器
    private SpriteRenderer spriteR;

    public int numColors
    {
        get{ return colorStructs.Length; }
    }

    public ColorType Color
    {
        get
        {
            return color;
        }

        set
        {
            SetColor(value);
        }
    }
    private ColorType color;

    [System.Serializable]
    public struct ColorStruct
    {
        public ColorType color;
        public Sprite sprite;
    }

    public ColorStruct[] colorStructs;

    private void Awake()
    {
        spriteR = transform.Find("Sweet").GetComponent<SpriteRenderer>();

        colorSpriteDict = new Dictionary<ColorType, Sprite>();

        for (int i = 0; i < colorStructs.Length; i++)
        {
            if (!colorSpriteDict.ContainsKey(colorStructs[i].color))
            {
                colorSpriteDict.Add(colorStructs[i].color, colorStructs[i].sprite);
            }
        }
    }

    public void SetColor(ColorType newColor)
    {
        color = newColor;
        if (colorSpriteDict.ContainsKey(newColor))
        {
            spriteR.sprite = colorSpriteDict[newColor];
        }
    }
}
