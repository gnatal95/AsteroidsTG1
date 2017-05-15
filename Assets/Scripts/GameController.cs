using System;
using System.IO;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    private const float WarpTimeToWin = 3f;

    public int Score = 0;
    public int Distancia = 500;
    private bool end = false;
    public int Level = 0;
    public int Difficulty = 1;
    public int DeathEasy = 0;
    public int DeathNormal = 0;
    public int DeathHard = 0;
    public int DeathLimit = 0;
	[HideInInspector]public int time =0;
	public levelSelect leveSelecter =  new levelSelect();

    private bool DDA = false;
    private bool dead = false;
    private bool _isGameStarted = false;
    private bool _isGameOver = false;
    private bool _isGameWin = false;
    private bool _isJumpLevel = false;
    private GameObject _startMenu;
    private Text _copyright;
    private Text _scoreText;
    private Text _distanciaText;
    private Text _countdownText;
    private Text _gameWinText;
    private Text _gameLoseText;
    private Text _faseCompleta;
    private GameObject _credits;
    private DateTime _endTime;
    private ShipMovement _shipMovement;
    private ShipFireWeapon _shipFireWeapon;
    private CameraChase _cameraChase;

    void Start ()
	{
        if (Input.GetKey(KeyCode.Alpha1))
			SceneManager.LoadScene("level 0",LoadSceneMode.Single);

		LoadResourcers ();

    }

    void Update () 
    {
	    HandleStartGame();
	    HandleMovement();
	    HandleFireWeapons();
		UpdateTime();
		UpdateWinnigDistance ();
		UpdateDistanceText ();

        HandleExitGame();
	    HandleResetLevel();
        HandleShowCredits();

        if (Input.GetKey(KeyCode.P))
        {
            _isJumpLevel = true;
        }

        JumpLevel();
		HandleNextLevel();
    }

    private void JumpLevel() {

        if (!_isJumpLevel)
            return;

        var judge = GameObject.FindGameObjectWithTag("DDA").GetComponentInChildren<DDA>();

        
        var udp = GameObject.FindGameObjectWithTag("UDP").GetComponentInChildren<UDPObj>();
        udp.sendData("5\n");
        
        judge.TimeClear();

        if (DateTime.Now.Subtract(_endTime).Seconds < 2)
            return;

        _gameWinText.enabled = false;

        _countdownText.enabled = false;
        //MUDA PRA 6 QUANDO FOR TESTAR DE VERDADE
        if (Level == 6)
        {
            udp.sendData("4\n");
        }

        Level++;
        //Adquirir dados do teste
        if (!end)
        {
			var fileName = "L" + (Level-1).ToString() + " d" + Difficulty + DateTime.UtcNow.ToString(" dd.MM.yyyy HH.mm") + ".txt";
            var sr = File.CreateText(fileName);
            sr.WriteLine("Deaths: " + judge.deaths);
			sr.WriteLine("Time: " + time + " s");
            sr.Close();
        }
        if (Level == 6)
        {
            end = true;
            _credits.SetActive(true);
            _copyright.enabled = true;
        }


        
        judge.Resetd();
		leveSelecter.LevelChoser (Level,Difficulty);
    }

    private void HandleStartGame()
    {
        if (!_isGameStarted && Input.anyKeyDown)
        {
            _startMenu.SetActive(false);

            _copyright.enabled = false;

            _isGameStarted = true;
            
        }
    }

    private void HandleMovement()
    {
        var ship = GameObject.FindGameObjectWithTag("Player");

        if (_shipMovement == null)
            return;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            _shipMovement.RotateLeft();

        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            _shipMovement.RotateRight();

        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            && ship.transform.position.x>505
            && _shipMovement.HasWarp)
            _shipMovement.Warp();

        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            _shipMovement.MoveForward();

        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            _shipMovement.MoveBackward();

        else
            _shipMovement.Decelerate();

        if ((!Input.GetKey(KeyCode.UpArrow)&&!Input.GetKey(KeyCode.W))
            || !(ship.transform.position.x > 505) )
            _shipMovement.DeWarp();
    }

    private void HandleFireWeapons()
    {
        if (_shipFireWeapon == null)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            _shipFireWeapon.FirePrimaryWeapon();
    }

    private void HandleViewChange()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            _cameraChase.IncreaseView();

        if (Input.GetKeyDown(KeyCode.RightAlt))
            _cameraChase.DecreaseView();            
    }

    private void HandleResetLevel()
    {
        if (!_isGameOver)
            return;

        if (_isGameWin)
            return;

        
        if (DateTime.Now.Subtract(_endTime).Seconds < 2)
            return;
        
        var god =  GameObject.FindGameObjectWithTag("DDA").GetComponentInChildren<DDA>();
        if (!dead)
        {
            dead = true;
            god.Kill(); 
        }

        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dead = !dead;
			leveSelecter.LevelChoser (Level,Difficulty);
        }
    }

    private static void HandleExitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void HandleShowCredits()
    {
        if (!_isGameWin)
            return;

        if (DateTime.Now.Subtract(_endTime).Seconds < 5)
            return;

        //_gameWinText.enabled = false;

        _countdownText.enabled = false;

        
    }

    private void HandleNextLevel()
    {
        var judge = GameObject.FindGameObjectWithTag("DDA").GetComponentInChildren<DDA>();
        if (!_isGameOver && !_isGameWin)
            return;

        var udp = GameObject.FindGameObjectWithTag("UDP").GetComponentInChildren<UDPObj>();
        if(DDA == false)
            udp.sendData("5\n");

        if (DateTime.Now.Subtract(_endTime).Seconds < 2)
            return;

		// o codigo comentado abaixo so funciona com a presenca do bitalino
		//tente comenta-lo caso o jogo esteja crashando

        string emotion = udp.getLatestUDPPacket();
		if (judge.GetDDA() < 2)
        {
            if (!emotion.Equals("stressed") && !emotion.Equals("bored") && !emotion.Equals("challenged"))
                return;
        }
        judge.TimeClear();


        _gameWinText.enabled = false;

        _countdownText.enabled = false;
        //MUDA PRA 6 QUANDO FOR TESTAR DE VERDADE
        if (Level == 6)
        {
            udp.sendData("4\n");
            Level++;
        }
			
        //Adquirir dados do teste
        if (!end)
        {			
			var fileName = "L" + (Level-1).ToString() + " d" + Difficulty + DateTime.UtcNow.ToString(" dd.MM.yyyy HH.mm") + ".txt";
            var sr = File.CreateText(fileName);
            sr.WriteLine("Deaths: " + judge.deaths);
            sr.WriteLine("Time: " + time + " s");
            sr.Close();
        }
        if (Level == 6)
        {
            end = true;
            _credits.SetActive(true);
            _copyright.enabled = true;
        }

        int numDeaths = judge.GetDeath();
        if(DDA==false){ 
        //DDA PERFOMANCE+EMOCAO
            if (judge.GetDDA() == 0) {
                if (emotion.Equals("stressed"))
                {
                    if (Difficulty == 0)//EASY
                    {
                        judge.SetSize(-1);

                    }
                    else if (Difficulty == 1)//NORMAL
                    {
                        if (numDeaths <= DeathHard)//died not enough
                        {
                            judge.SetSize(-1);
                        }
                        else if (numDeaths > DeathNormal)//died a lot
                        {
                            Difficulty = 0;
                            judge.SetSize(0);
                        }
                        else//died enough
                        {
                            Difficulty = 0;
                            judge.SetSize(1);
                        }
                    }
                    else //HARD
                    {
                        if (numDeaths <= DeathHard)//died enough
                        {
                            Difficulty = 1;
                            judge.SetSize(1);
                        }
                        else//died a lot
                        {
                            Difficulty = 1;
                            judge.SetSize(0);
                        }
                    }
                }
                else if (emotion.Equals("challenged"))
                {
                    if (Difficulty == 0)//EASY
                    {
                        if (numDeaths < DeathEasy)
                            judge.SetSize(0);
                        else
                            judge.SetSize(1);
                    }
                    else if (Difficulty == 1)//NORMAL
                    {
                        if (numDeaths <= DeathHard)//died not enough
                        {
                            judge.SetSize(1);
                        }
                        else if (numDeaths < DeathEasy)//died enough
                            judge.SetSize(0);
                        else//died a lot
                        {
                            Difficulty = 0;
                            judge.SetSize(1);
                        }

                    }
                    else //HARD
                    {
                        if (numDeaths <= DeathHard)//died not enough
                            judge.SetSize(0);
                        else//died a lot
                        {
                            Difficulty = 1;
                            judge.SetSize(1);
                        }

                    }
                }
                else
                {
                    if (Difficulty == 0)//EASY
                    {
                        if (numDeaths < DeathNormal)//died not enough
                        {
                            judge.SetSize(0);
                            Difficulty = 1;
                        }
                        else//died enough
                        {
                            judge.SetSize(-1);
                            Difficulty = 1;
                        }
                    }
                    else if (Difficulty == 1)//NORMAL
                    {
                        if (numDeaths <= DeathHard)//died not enough
                        {
                            Difficulty = 2;
                            judge.SetSize(0);
                        }
                        else if (numDeaths < DeathEasy)//died enough
                        {
                            Difficulty = 2;
                            judge.SetSize(-1);
                        }
                        else//died a lot
                        {
                            Difficulty = 1;
                            judge.SetSize(1);
                        }
                    }
                    else //HARD
                    {
                        judge.SetSize(1);
                    }
                }
            }
            //DDA EMOTIVO
            else if (judge.GetDDA() == 1) {
                if (emotion.Equals("bored"))
                {
                    if (Difficulty < 2)
                        Difficulty++;
                }
                else if (emotion.Equals("stressed"))
                {
                    if (Difficulty > 0)
                        Difficulty--;
                }
            }
            //DDA PERFOMANCE
            else {
                if (Difficulty == 0)
                {
                    if (numDeaths <= DeathNormal)
                        Difficulty = 1;
                }
                else if (Difficulty == 1)
                {
                    if (numDeaths <= DeathHard)
                        Difficulty = 2;
                    else if (numDeaths > DeathNormal)
                        Difficulty = 0;
                }
                else
                {
                    if (numDeaths > DeathHard)
                        Difficulty = 1;
                }
            }
        }


        DDA = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            judge.Resetd();
			leveSelecter.LevelChoser (Level,Difficulty);
        }

    }

    public void AddToScore(int points)
    {
        Score += points;
    }

    public void SetJumpCountdown(float warpTime)
    {
        if (_isGameWin)
            return;

        if (warpTime > WarpTimeToWin)
            SetGameWin();

        var warpTimeRemaining = WarpTimeToWin - warpTime;

        if (_isGameWin)
            _countdownText.text = "Jump to lightspeed complete.";

        else if (warpTime > 0f)
            _countdownText.text = "Jump to lightspeed in " + warpTimeRemaining.ToString("N2") + " seconds.";

        else 
            _countdownText.text = string.Empty;        
    }

    public void SetGameWin()
    {
        if (Level==5)
            _gameWinText.enabled = true;


        _isGameOver = true;

        _isGameWin = true;

        _endTime = DateTime.Now;

        _faseCompleta.enabled = true;

        Level++;
    }

    public void SetGameOver()
    {
        _gameLoseText.enabled = true;

        _isGameOver = true;               

        _endTime = DateTime.Now;
    }

    public int GetDeathLimit()
    {
        return DeathLimit;
    }

    public int getDifficulty()
    {
        return Difficulty;
    }

	private void LoadResourcers(){

		_startMenu = GameObject.FindGameObjectWithTag("StartMenu");
		_copyright = GameObject.FindGameObjectWithTag("Copyright").GetComponent<Text>();
		_scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
		_distanciaText = GameObject.FindGameObjectWithTag("Distancia").GetComponent<Text>();
		_countdownText = GameObject.FindGameObjectWithTag("Countdown").GetComponent<Text>();
		_gameWinText = GameObject.FindGameObjectWithTag("GameWin").GetComponent<Text>();
		_gameLoseText = GameObject.FindGameObjectWithTag("GameLose").GetComponent<Text>();
		_faseCompleta = GameObject.FindGameObjectWithTag("Completa").GetComponent<Text>();
		_credits = GameObject.FindGameObjectWithTag("Credits");
		_credits.SetActive(false);

		var ship = GameObject.FindGameObjectWithTag("Player");

		_shipMovement = ship.GetComponentInChildren<ShipMovement>();
		_shipFireWeapon = ship.GetComponentInChildren<ShipFireWeapon>();

		var camera = GameObject.FindGameObjectWithTag("MainCamera");

		_cameraChase = camera.GetComponentInChildren<CameraChase>();
		_startMenu.SetActive(false);
		_copyright.enabled = false;

		var udp = GameObject.FindGameObjectWithTag("UDP").GetComponentInChildren<UDPObj>();
		udp.sendData("3\n");
		udp.sendData(Level + "" + Difficulty + "\n");

	}

	private void UpdateWinnigDistance(){	
		if (!_isGameOver) {
			var ship = GameObject.FindGameObjectWithTag("Player");
			Distancia = 500 - (int)ship.transform.position.x;
		}
	}

	private void UpdateDistanceText(){
		if(Distancia>0)
			_distanciaText.text = "Distancia: " + Distancia;
		else 
			_distanciaText.text = "Entre no hiperespaco";
	}

	private void UpdateTime(){
		time = (int)Time.fixedTime;
		_scoreText.text = "Time: " + time;
	}

}

