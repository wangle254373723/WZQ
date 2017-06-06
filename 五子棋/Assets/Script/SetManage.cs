using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*
设置类
*/
public class SetManage : MonoBehaviour {


	public Button mSetAudio;//设置音效
	public Button mSetGame;//设置游戏ck;
	public Button mSetBack;//游戏退出

	[SerializeField]
	GameObject mAudio_Open;
	[SerializeField]
	GameObject mAudio_Close;

	[SerializeField]
	GameObject mGame_Start;
	[SerializeField]
	GameObject mGame_Stop;
	bool mIsAudio = true;
	int mIsGame = 1;

	void Start () {
		mSetBack.onClick.AddListener (() => {
			Application.Quit();
		});
		mSetAudio.onClick.AddListener (() => {

			if (mIsAudio) {
				mIsAudio = false;
				SetHideImage (true, true);
			} else {
				mIsAudio = true;
				SetHideImage (false, true);
			}
			transform.parent.GetComponent<AudioSource> ().enabled = mIsAudio;
		});

		mSetGame.onClick.AddListener( ( )=>{
			if(mIsGame==1){
				mIsGame=0;
				SetHideImage(true,false);
			}else{
				mIsGame=1;
				SetHideImage(false,false);
			}
			Time.timeScale=mIsGame;
		});
	}




	void SetHideImage(bool isA,bool isB){
		//isB用来判断需要设置的是音效,还是游戏
		if (isB) {
			if (isA) {
				mAudio_Close.gameObject.SetActive (false);
				mAudio_Open.gameObject.SetActive (true);
			} else {
				mAudio_Close.gameObject.SetActive (true);
				mAudio_Open.gameObject.SetActive (false);
			}
		} else {
			if (isA) {
				mGame_Start.gameObject.SetActive (false);
				mGame_Stop.gameObject.SetActive (true);
			} else {
				mGame_Start.gameObject.SetActive (true);
				mGame_Stop.gameObject.SetActive (false);
			}
		}
	}
}
