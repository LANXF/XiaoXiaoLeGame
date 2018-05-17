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
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //
    public Vector2 CorrectPosition(int x, int y)
    {
        return new Vector2(transform.position.x - xColumn/2f + x, transform.position.y + yRow/2f - y);
    }

    private GameSweet CreateNewSweet(int x, int y, SweetsType type)
    {
        GameObject newObj = Instantiate(sweetPrefabDict[type], CorrectPosition(x,y),Quaternion.identity);
        newObj.transform.SetParent(transform);

        GameSweet gameSweet = newObj.GetComponent<GameSweet>();
        gameSweet.Init(x, y, this, type);
        //数组赋值
        sweets[x, y] = gameSweet;

        return gameSweet;
    }
    
    //全部填充的方法
    private void AllFill()
    {

    }
    //分部填充
    private void Fill()
    {
        bool filledNotFinshed = false;//这个部分布尔值是用来判断本次填充是否完成。
        
    }
}
