using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
    private float asteoidStartTime;
    public float timeNow;
    public float ExpirationTime;
	// Use this for initialization

	void Start () {
		asteoidStartTime = Time.fixedTime;
        ExpirationTime = 2;
	}
	
	// Update is called once per frame
	void Update () {
        timeNow = Time.fixedTime;
		DestroiAsteroid ();
	}

	private void DestroiAsteroid(){
		if (timeNow > asteoidStartTime + ExpirationTime)
			Destroy(gameObject);
	}

}
