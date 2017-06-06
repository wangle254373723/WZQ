using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*
主界面
*/
public class Title : MonoBehaviour {

    public Button StartButton;
    public Button ExitButton;

    // 游戏界面
    public GameObject board;


	void Start () {
        StartButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            board.SetActive(true);
        });

        ExitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
	}
	
}
