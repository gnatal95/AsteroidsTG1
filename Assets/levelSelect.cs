using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class levelSelect{
    public int Difficulty;
	public bool DDALearn = true;

	public void LevelChoser(int level,int difficulty){
		switch (level)
		{
		case 0:
			LevelPass (0,difficulty);
			break;
		case 1:
			LevelPass (1,difficulty);
			break;
		case 2:
			LevelPass (2,difficulty);
			break;
		case 3:
			LevelPass (3,difficulty);
			break;
		case 4:
			LevelPass (4,difficulty);
			break;
		case 5:
			LevelPass (5,difficulty);
			break;
		}

	}

	private void LevelPass(int level,int Difficulty){
		if (DDALearn) {
			NormalLeverSelecter (level);
		return;
		}

		if (Difficulty == 0)
			SceneManager.LoadScene("level "+ level.ToString()+ " easy",LoadSceneMode.Single);
		else if (Difficulty == 2)
			SceneManager.LoadScene("level "+ level.ToString()+ " hard",LoadSceneMode.Single);
		else
			SceneManager.LoadScene("level "+ level.ToString()+ " normal",LoadSceneMode.Single);
	
	}

	private void NormalLeverSelecter(int level){
		if (level == 0) {
			SceneManager.LoadScene ("level " + level.ToString (), LoadSceneMode.Single);
			return;
		}
		SceneManager.LoadScene("level "+ level.ToString()+ " normal",LoadSceneMode.Single);
	
	}
		
}
