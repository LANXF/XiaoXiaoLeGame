using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSweet : MonoBehaviour {

    private int x;
    public int X
    {
        get
        {
            return x;
        }

        set
        {
            if (CanMove())
            {
                x = value;
            }
        }
    }

    private int y;
    public int Y
    {
        get
        {
            return y;
        }

        set
        {
            if (CanMove())
            {
                y = value;
            }
        }
    }

    //类型
    public GameManager.SweetsType Type
    {
        get
        {
            return type;
        }
    }
    private GameManager.SweetsType type;

    [HideInInspector]
    public GameManager gameManager;

    //移动组件
    private MovedSweet movedComponent;
    public MovedSweet MovedComponent
    {
        get
        {
            return movedComponent;
        }
    }
    //变色组件
    private ColorSweet colorComponent;
    public ColorSweet ColorComponent
    {
        get
        {
            return colorComponent;
        }
    }

    //是否可以移动
    public bool CanMove()
    {
        return movedComponent != null;
    }

    //是否可以设置颜色
    public bool CanColor()
    {
        return ColorComponent != null;
    }

    public void Init(int _x,int _y,GameManager _gameManager, GameManager.SweetsType _type)
    {
        x = _x;
        y = _y;
        gameManager = _gameManager;
        type = _type;
    }

    private void Awake()
    {
        movedComponent = GetComponent<MovedSweet>();
        colorComponent = GetComponent<ColorSweet>();
    }
}
