using UnityEngine;
using UnityEngine.UI;
/*
棋盘交叉数据
*/
// 每个交叉点逻辑
public class Cross : MonoBehaviour {

    // 位置
    public int GridX;
    public int GridY;
	public bool IsClick = false;//记录当前位置是否被点击
    public MainLoop mainLoop;


	void Start () {
        GetComponent<Button>().onClick.AddListener( ( )=>{
			AudioManage.instance.SetAudio("click");
            mainLoop.OnClick(this);
        });
	}
}
