using UnityEngine;
using System.Collections;

public class AsteroidMovement : MonoBehaviour {

	public int verticalLimit = 500;
	public int horizontalLimit = 100;

	public Vector3 Direction;
	void Update () 
	{
		KeepInVerticalLimits ();
		KeepInHorizontalLimits ();
		gameObject.transform.Translate(TranslateAsteroid());
	}
		
	private Vector3 TranslateAsteroid(){
		var x = Direction.x * Time.deltaTime;        
		var y = Direction.y * Time.deltaTime;
		var z = Direction.z;
		var vector = new Vector3(x, y, z);
		return vector;
	}

	private void KeepInVerticalLimits(){
		if (transform.position.x > verticalLimit)
			Direction.x = -Direction.x;
	}

	private void KeepInHorizontalLimits(){
		if (transform.position.y > horizontalLimit || transform.position.y < -horizontalLimit)
			Direction.y = -Direction.y;	
	}

}

