﻿using UnityEngine;
using System.Collections;

public class CreateAsteroids : MonoBehaviour {

    public int AsteroidCount = 10;
    public bool Indestructible = false;
    public bool Shielded = false;
    public bool Explosive = false;
    public float mimFieldX = 100f;
    public float maxFieldX = 200f;
    public float minFieldY = -100f;
    public float maxFieldY = 100f;
    public float minSize = 0.75f;
    public float maxSize = 0.75f;
    public float minSpeed = -5f;
    public float maxSpeed = 5f;

	// Use this for initialization
	void Start () 
    {
        var asteroid = Resources.Load("Prefabs/Asteroid");
        if(Explosive)
            asteroid = Resources.Load("Prefabs/Asteroid2");

		AccelPerSize ();
		SetAccel ();

        for (var i = 0; i < AsteroidCount; i++)
        {
			var position = AsteroidStartPosition ();
            var rotation = new Quaternion(0f, 0f, 0f, 0f);
            var gameObject = (GameObject)Instantiate(asteroid, position, rotation);
            var scale = Random.Range(minSize, maxSize);

			gameObject.transform.localScale = new Vector3(scale, scale, scale);

			var movement = GameObject.FindObjectOfType<AsteroidMovement>();
			if (IsIndestructible ())
				SetIndesctrutible ();
			if(IsShielded())
				SetShielded ();
			if(IsExplosive())
				SetExplosive ();

			movement.Direction = AsteroidDirection();
        }
	}

    private void AccelPerSize()
    {
        var controle = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<GameController>();
        int difficulty = controle.getDifficulty();

        var judge = GameObject.FindGameObjectWithTag("DDA").GetComponentInChildren<DDA>();
        int size = judge.GetSize();


        if (size == 1)
        {

            if (difficulty == 0) {
                maxSpeed = -4.0f;
                minSpeed = 4.0f;
            }
            else if (difficulty == 1) {
                maxSpeed = -7.0f;
                minSpeed = 7.0f;
            }
            else {
                minSpeed = 5.0f;
            }
        }
        else if (size == -1)
        {
            if (difficulty == 0) {
                maxSpeed = -1.0f;
                minSpeed = 1.0f;
            }
            else if (difficulty == 1) {
                maxSpeed = -2.0f;
                minSpeed = 2.0f;
            }
            else {
                maxSpeed = 4.0f;
            }
        }
    }


    private void SetAccel(){
        float accelMinus = 0.5f;

        var controle = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<GameController>();
        int deathLimit = controle.GetDeathLimit();
        int difficulty = controle.getDifficulty();

        var judge = GameObject.FindGameObjectWithTag("DDA").GetComponentInChildren<DDA>();
        int numDeath = judge.GetDeath();
        int size = judge.GetSize();
        int mult = (numDeath - deathLimit) / 2;

        if (numDeath >= deathLimit)
        {
            if (difficulty == 0)
            {
                if (size == 0) {
                    if (mult > 1)
                        mult = 1;
                    maxSpeed = maxSpeed - accelMinus - (mult * accelMinus);
                    minSpeed = minSpeed + accelMinus + (mult * accelMinus);
                }
                else if (size == 1)
                {
                    if (mult > 4)
                        mult = 4;
                    maxSpeed = maxSpeed - accelMinus - (mult * accelMinus);
                    minSpeed = minSpeed + accelMinus + (mult * accelMinus);
                }
            }
            else if (difficulty == 1)
            {
                if (size == 0)
                {
                    if (mult > 4)
                        mult = 4;
                    maxSpeed = maxSpeed - accelMinus - (mult * accelMinus);
                    minSpeed = minSpeed + accelMinus + (mult * accelMinus);
                }
                else if (size == 1)
                {
                    if (mult > 6)
                        mult = 6;
                    maxSpeed = maxSpeed - accelMinus - (mult * accelMinus);
                    minSpeed = minSpeed + accelMinus + (mult * accelMinus);
                }
            }
            else
            {
                if (size == 0)
                {
                    if (mult > 6)
                        mult = 6;
                    maxSpeed = maxSpeed - accelMinus - (mult * accelMinus);
                }
                else if (size == 1)
                {
                    if (mult > 4)
                        mult = 4;
                    maxSpeed = maxSpeed - accelMinus - (mult * accelMinus);
                }
            }
        }

    }

	private Vector3 AsteroidDirection(){
		var mx = Random.Range(minSpeed, maxSpeed);
		var my = Random.Range(minSpeed, maxSpeed);
		var mz = 0f;

		var mmx = Random.Range(0, 2);
		var mmy = Random.Range(0, 2);

		if (mmx == 0) {
			mx *= -1;
		}
		if (mmy == 0) {
			my *= -1;
		}

		var direction = new Vector3(mx, my, mz);
		return direction;
	}

	private Vector3 AsteroidStartPosition(){
		var x = Random.Range(mimFieldX, maxFieldX);
		var y = Random.Range(minFieldY, maxFieldY);
		var z = 0f;
		return new Vector3(x, y, z);;
	}

	private bool IsIndestructible(){
		return Indestructible;
	}

	private bool IsShielded(){
		return Shielded;
	}

	private bool IsExplosive(){
		return Explosive;
	}

	private void SetIndesctrutible(){
		var ast = GameObject.FindObjectOfType<AsteroidType>();
		ast.indestructible = true;
	}

	private void SetShielded(){
		var shield = GameObject.Find("Shield Asteroid");
		shield.GetComponent<MeshRenderer>().enabled = true;
		shield.GetComponent<SphereCollider>().enabled = true;
	}


	private void SetExplosive(){
		if (Explosive) {
			var ast = GameObject.FindObjectOfType<AsteroidType>();
			ast.explosive = true;

		}
	}

}
