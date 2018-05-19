using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //甜品的种类
    public enum SweetsType
    {
        EMPTY,
        NORMAL,
        BARRIER,
        ROM_CLEAR,
        COLUMN_CLEAR,
        RAINBOWCANDY,
        COUNT//标记类型
    }

    //字典
    public Dictionary<SweetsType, GameObject> sweetPrefabDict;

    [System.Serializable]
    public struct SweetPrefab
    {
        public SweetsType type;
        public GameObject prefab;
    }

    //结构体数组
    public SweetPrefab[] SweetPrefabs;

    //单例
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    //大网格的长列数
    public int xColumn;
    public int yRow;

    //填充时间
    public float fillTime = 0;

    public GameObject girdPrefab;

    //甜品数组
    private GameSweet[,] sweets;

    private void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start () {

        //初始化字典
        sweetPrefabDict = new Dictionary<SweetsType, GameObject>();
        for (int i = 0; i < SweetPrefabs.Length; i++)
        {
            SweetPrefab sp = SweetPrefabs[i];
            if (!sweetPrefabDict.ContainsKey(sp.type))
            {
                sweetPrefabDict.Add(sp.type, sp.prefab);
            }
        }

        //实例化巧克力背景
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                GameObject chocolate = Instantiate(girdPrefab, CorrectPosition(x,y), Quaternion.identity);
                chocolate.transform.SetParent(transform);
            }
        }

        //初始化甜品的二维数组
        sweets = new GameSweet[xColumn,yRow];

        //实例化空甜品
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                CreateNewSweet(x, y, SweetsType.EMPTY);
            }
        }

        Destroy(sweets[4, 4].gameObject);
        CreateNewSweet(4, 4, SweetsType.BARRIER);

        StartCoroutine(AllFill());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //校正甜品的位置
    public Vector2 CorrectPosition(int x, int y)
    {
        return new Vector2(transform.position.x - xColumn/2f + x, transform.position.y + yRow/2f - y);
    }

    //创建一个甜品
    private GameSweet CreateNewSweet(int x, int y, SweetsType type)
    {
        GameObject newObj = Instantiate(sweetPrefabDict[type], CorrectPosition(x,y),Quaternion.identity);
        newObj.transform.SetParent(transform);

        GameSweet gameSweet = newObj.GetComponent<GameSweet>();
        gameSweet.Init(x, y, this, type);

        //如果是普通类型就创建一个随机的颜色的甜品
        if (type == SweetsType.NORMAL)
        {
            gameSweet.ColorComponent.SetColor((ColorSweet.ColorType)Random.Range(0, gameSweet.ColorComponent.numColors));
        }
        //数组赋值
        sweets[x, y] = gameSweet;

        return gameSweet;
    }
    
    //全部填充的方法
    private IEnumerator AllFill()
    {
        while (Fill())
        {
            yield return new WaitForSeconds(fillTime);
            Debug.Log("填充中");
        }

    }
    //分部填充
    private bool Fill()
    {
        bool filledNotFinished = false;//这个部分布尔值是用来判断本次填充是否完成。
        for (int y = yRow-2; y >= 0; y--)
        {
            for (int x = 0; x < xColumn; x++)
            {
                GameSweet sweet = sweets[x, y];
                if (sweet.CanMove())//如果无法移动，则无法往下填充
                {
                    GameSweet sweetBelow = sweets[x, y + 1];
                    //垂直填充
                    if (sweetBelow.Type == SweetsType.EMPTY)
                    {
                        sweet.MovedComponent.Move(x, y + 1, fillTime);
                        sweets[x, y + 1] = sweet;
                        Destroy(sweetBelow.gameObject);
                        //填充上面哪行
                        CreateNewSweet(x, y, SweetsType.EMPTY);

                        filledNotFinished = true;
                    }
                    else
                    {
                        //斜方向填充
                        //如果不可以移动，就查看坐下和右下是否可以移动
                        for (int i = -1; i <= 1; i++)
                        {
                            int downX = x + i;

                            if (i != 0)
                            {
                                if (downX >= 0 && downX < xColumn)
                                {
                                    GameSweet downSweet = sweets[downX, y + 1];
                                    if (downSweet.Type == SweetsType.EMPTY)
                                    {
                                        bool canFill = true;
                                        for (int aboveY = y; aboveY >= 0; aboveY--)
                                        {
                                            GameSweet sweetAbove = sweets[downX,aboveY];
                                            if (sweetAbove.CanMove())
                                            {
                                                break;
                                            }
                                            else if (!sweetAbove.CanMove() && sweetAbove.Type != SweetsType.EMPTY)
                                            {
                                                canFill = false;
                                                break;
                                            }
                                        }

                                        if (!canFill)
                                        {
                                            Destroy(downSweet.gameObject);
                                            sweet.MovedComponent.Move(downX,y+1,fillTime);
                                            sweets[downX, y + 1] = sweet;
                                            CreateNewSweet(x, y, SweetsType.EMPTY);
                                            filledNotFinished = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (y == 0 && sweet.Type == SweetsType.EMPTY)
                {
                    GameObject newSweetObject = Instantiate(sweetPrefabDict[SweetsType.NORMAL], CorrectPosition(x, y - 1), Quaternion.identity);
                    newSweetObject.transform.SetParent(transform);

                    GameSweet sweetComponent = newSweetObject.GetComponent<GameSweet>();
                    sweetComponent.Init(x, y - 1, this, SweetsType.NORMAL);

                    sweetComponent.MovedComponent.Move(x, y, fillTime);
                    sweetComponent.ColorComponent.SetColor((ColorSweet.ColorType)Random.Range(0, sweetComponent.ColorComponent.numColors));

                    sweets[x, y] = sweetComponent;

                    //销毁空物体
                    Destroy(sweet.gameObject);

                    filledNotFinished = true;
                }
            }
        }

        return filledNotFinished;
    }
}
