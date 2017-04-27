using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class levelSelect : MonoBehaviour {
    public int Level;
    public int Difficulty;

	// Use this for initialization
	void Start () {
        switch (Level)
        {
            case 0:
                if (Difficulty == 0)
                    SceneManager.LoadScene("level 0 easy",LoadSceneMode.Single);
                else if (Difficulty == 2)
                    SceneManager.LoadScene("level 0 hard",LoadSceneMode.Single);
                else
					SceneManager.LoadScene("level 0",LoadSceneMode.Single);
                break;
            case 1:
                if (Difficulty == 0)
					SceneManager.LoadScene("level 1 easy",LoadSceneMode.Single);
                else if (Difficulty == 2)
					SceneManager.LoadScene("level 1 hard",LoadSceneMode.Single);
                else
					SceneManager.LoadScene("level 1 normal",LoadSceneMode.Single);
                break;
            case 2:
                if (Difficulty == 0)
					SceneManager.LoadScene("level 2 easy",LoadSceneMode.Single);
                else if (Difficulty == 2)
					SceneManager.LoadScene("level 2 hard",LoadSceneMode.Single);
                else
					SceneManager.LoadScene("level 2 normal",LoadSceneMode.Single);
                break;
            case 3:
                if (Difficulty == 0)
					SceneManager.LoadScene("level 3 easy",LoadSceneMode.Single);
                else if (Difficulty == 2)
					SceneManager.LoadScene("level 3 hard",LoadSceneMode.Single);
                else
					SceneManager.LoadScene("level 3 normal",LoadSceneMode.Single);
                break;
            case 4:
                if (Difficulty == 0)
					SceneManager.LoadScene("level 4 easy",LoadSceneMode.Single);
                else if (Difficulty == 2)
					SceneManager.LoadScene("level 4 hard",LoadSceneMode.Single);
                else
					SceneManager.LoadScene("level 4 normal",LoadSceneMode.Single);
                break;
            case 5:
                if (Difficulty == 0)
					SceneManager.LoadScene("level 5 easy",LoadSceneMode.Single);
                else if (Difficulty == 2)
					SceneManager.LoadScene("level 5 hard",LoadSceneMode.Single);
                else
					SceneManager.LoadScene("level 5 normal",LoadSceneMode.Single);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
