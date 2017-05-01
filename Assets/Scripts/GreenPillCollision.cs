﻿using UnityEngine;
using System.Collections;

public class GreenPillCollision : MonoBehaviour {

    private Object _explosion;
    private AudioClip _sound;

	//resources load carrega assets para o jogo
    void Start()
    {
		LoadAssets ();
	}

	private void LoadAssets(){

		_explosion = Resources.Load("Prefabs/Explosion");
		_sound = Resources.Load<AudioClip>("Sounds/shields-up");

	}

    void OnTriggerEnter(Collider collider)
    {
		if (IsPlayer ())
			return;

        var pillPosition = collider.gameObject.transform.position;

        var rotation = Quaternion.identity;

		//cria um clone do objeto explosion
        Instantiate(_explosion, pillPosition, rotation);

        var mainAudio = GameObject.FindGameObjectWithTag("MainAudio");

        var audio = mainAudio.AddComponent<AudioSource>();

        audio.clip = _sound;

        audio.Play();

        Destroy(gameObject);

        var shield = GameObject.FindGameObjectWithTag("Shield");

        shield.GetComponent<MeshRenderer>().enabled = true;

        shield.GetComponent<SphereCollider>().enabled = true;
    }

	private bool IsPlayer(){
		if (GetComponent<Collider>().gameObject.tag.Equals("Player"))
			return true;		
		return false;
	}
}