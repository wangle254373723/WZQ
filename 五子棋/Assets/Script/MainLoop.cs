using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class MainLoop : MonoBehaviour {

	enum State
	{
		BlackGo, // 黑方(玩家)
		WhiteGo, // 白方(电脑)
		Over,    // 结束
	}

    // 棋子的模板
    public GameObject WhitePrefab;
    public GameObject BlackPrefab;
    // 结果窗口
    public ResultWindow ResultWindow;

	Text mTimerLabel;			//计时面板
	Text mCLabel;				//游戏开始倒计时

	int mWhoStart = -1;			//记录谁先出手 -用来做棋满判断当前下棋个数
	int mCountDown=3;			//开局倒计时开始给玩家准备时间
	float mTimer=0;				//下棋时间计时器
    int mMaxTimer=15;
	bool isAiCross=true;		//AI是否可以走
	bool mIsSatrt=false;		//是否正式开始
	bool mIsAgin=false;			//是否是重新开始
    // 当前状态
    State _state;

    // 棋盘显示
    Board _board;

    // 棋盘数据
    BoardModel _model;

	//AI(电脑)
    AI _ai;

    bool CanPlace( int gridX, int gridY )
    {
        // 如果这个地方可以下棋子
		if(_model==null){return false;}
        return _model.Get(gridX, gridY) == ChessType.None;        
    }

    //此处判断当前走棋后是否胜出
    bool PlaceChess(Cross cross, bool isblack)
    {
        if (cross == null)
            return false;

        // 创建棋子
        var newChess = GameObject.Instantiate<GameObject>(isblack ? BlackPrefab : WhitePrefab);
        newChess.transform.SetParent(cross.gameObject.transform, false);

		var ctype = isblack ? ChessType.Black : ChessType.White;
        // 设置数据
		_model.Set(cross.GridX, cross.GridY, ctype);

		//每次需要判断是否是满棋局
		if(Click>=288&&isblack){

			mMaxTimer = 0;
			return true;
		}

        var linkCount = _model.CheckLink(cross.GridX, cross.GridY, ctype);

        return linkCount >= BoardModel.WinChessCount;
    }


	//重新开局刷新数据
    void Restart( )
    {
		if (mIsSatrt) {
			Click = 0;
			mMaxTimer = 15;
			//随机一个数，判断该值决定先手
			int suiji= Random.Range (0, 10);
			if (suiji >= 5 ? true : false) {
				_state = State.BlackGo;
				mWhoStart = 1;
				mTimerLabel.text = "0:15";
			} else {
				_state = State.WhiteGo;
				mWhoStart = 0;
				mTimerLabel.text = "0:00";
			}
			_model = new BoardModel();
			_ai = new AI();
			//不是重新开始,需要后刷棋局,
			if (!mIsAgin) {
				_board.Reset();
				mIsAgin = false;
			}
		}
    }
		

	//重新开始点击调用
	public void SetAgin(bool isAgin){
		mIsAgin = isAgin;
		if(isAgin){_board.Reset ();}//是重新开始的话,先清空棋局
		mCountDown = 3;
		mIsSatrt = false;
		mCLabel.text =  mCountDown.ToString();
		mCLabel.transform.localPosition = Vector3.zero;
		mCLabel.gameObject.SetActive (true);
		InvokeRepeating("SetCountDown",1,1);
	}



	//设置倒计时
	 void SetCountDown(){
			mCountDown--;
			mCLabel.text =  mCountDown.ToString();
		if(mCountDown<=0){
			StartCoroutine (setHideCount());
		}
	}

	//设置倒计时动画
	IEnumerator setHideCount(){
		DOTween.To (() => mCLabel.transform.localPosition, (x) => mCLabel.transform.localPosition = x, new Vector3(0,800,0), 0.5f);
		yield return new  WaitForSeconds(0.5f);
		CancelInvoke ("SetCountDown");
		mIsSatrt = true;
		mCLabel.gameObject.SetActive (false);
		Restart ();
	}

	int Click=0;//记录当前棋子
	//当放置棋子的时候判断状态和能否放置
    public void OnClick( Cross cross )
    {
		
        if (_state != State.BlackGo)
            return;

        // 如果该位置可以放置
        if (CanPlace(cross.GridX, cross.GridY))
        {
			Click++;
            _lastPlayerX = cross.GridX;
            _lastPlayerY = cross.GridY;
            if (PlaceChess(cross , true))
            {
                // 已经胜利
                _state = State.Over;
                ShowResult(ChessType.Black);
				return;
            }
            else
			{
				//判断当前棋局是否满棋,玩家先走,287次正确走步时为满棋
				if (Click >= 287 && mWhoStart == 1) {
					mMaxTimer = 0;
					return;
				}
                // 换电脑走
                _state = State.WhiteGo;
				mTimerLabel.text = "0:00";
				mMaxTimer = 15;
            }

        }
    }

    void Start( )
    {
        _board = GetComponent<Board>();
		mTimerLabel = transform.parent.Find ("timerImage/Text").GetComponent<Text> ();
		mCLabel = transform.parent.Find ("countDown").GetComponent<Text> ();
		mCLabel.text = mCountDown.ToString();
		mTimerLabel.text = "0:00";
		InvokeRepeating("SetCountDown",1,1);
    }

    int _lastPlayerX, _lastPlayerY;



	//通知弹窗显示结果
    void ShowResult( ChessType winside )
    {
		if (_state == State.Over) {
			ResultWindow.gameObject.SetActive (true);
			ResultWindow.Show (winside);
		} else {
			ResultWindow.gameObject.SetActive (false);
		}
    }


	//判断状态做出对应逻辑
	void Update () {
		switch( _state )
		{
		// 白方(电脑)走
		case State.WhiteGo:
			{
				if(isAiCross){
					// 计算电脑下的位置
					isAiCross = false;
					int gridX, gridY;
					_ai.ComputerDo(_lastPlayerX, _lastPlayerY, out gridX, out gridY);
					StartCoroutine (SetAiCross(gridX, gridY));
				}
			}
			break;
		case State.BlackGo:
			if (!mIsSatrt)
				return;
			SetTime ();//开始计时
			break;
        }
	}

	//设定电脑下棋
	IEnumerator SetAiCross(int gridX, int gridY){
		yield return new WaitForSeconds (0.5f);
		if (PlaceChess(_board.GetCross(gridX, gridY), false))
		{
			yield return new WaitForSeconds (0.8f);
			// 电脑胜利
			_state = State.Over;
			ShowResult(ChessType.White);
		}
		else
		{
			// 换玩家走
			_state = State.BlackGo;
		}
		isAiCross=true;
	}
		
	//设置时间
	void SetTime(){
		if (mMaxTimer == null|| !mIsSatrt)
			return;
		mTimer += Time.deltaTime;
		if(mTimer>=1){
			mTimer = 0;
			mMaxTimer--;
		}
		if(mMaxTimer<=0){
			_state = State.Over;
			ShowResult (ChessType.White);
			mTimerLabel.text = "0:00";
		}
		string _sting = mMaxTimer > 9 ? mMaxTimer.ToString ("0:00") : "0:0" + mMaxTimer;
		mTimerLabel.text = _sting;
	}
}
