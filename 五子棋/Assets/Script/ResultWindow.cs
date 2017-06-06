using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*
弹窗
*/
public class ResultWindow : MonoBehaviour {

    // 重玩按钮
    public Button ReplayButton;

    // 提示文字
    public Text Message;

    // 主循环句柄
    public MainLoop mainLoop;
	
	void Start () {
        ReplayButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
				mainLoop.SetAgin(true);
        });
	}
	
    public void Show( ChessType wintype )
    {
        switch( wintype )
        {
            case ChessType.Black:
                {
                    Message.text = string.Format("恭喜你, 你战胜了电脑!");
                }
                break;
            case ChessType.White:
                {
                    Message.text = string.Format("很遗憾,电脑胜出!");
                }
                break;
        }
    }
}
