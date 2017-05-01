using UnityEngine;
using System.Collections;

public class BluePillCollision : MonoBehaviour {

    private Object _explosion;
    private AudioClip _sound;

    void Start()
    {
		LoadAssests ();
    }
		
	private void LoadAssests(){
		_explosion = Resources.Load("Prefabs/Explosion");
		_sound = Resources.Load<AudioClip>("Sounds/success");
	
	}

    void OnTriggerEnter(Collider collider)
    {
		if (IsPlayer())
            return;
		
		CreateExplosion ();
		YellSound ();
        Destroy(gameObject);

        var ship = GameObject.FindGameObjectWithTag("Player");

        var script = (ShipMovement) ship.GetComponent("ShipMovement");

        script.HasWarp = true;
    }

	private bool IsPlayer(){
		if (GetComponent<Collider>().gameObject.tag.Equals("Player"))
			return true;		
		return false;
	}

	private void YellSound(){

		var mainAudio = GameObject.FindGameObjectWithTag("MainAudio");
		var audio = mainAudio.AddComponent<AudioSource>();

		audio.clip = _sound;
		audio.Play();
	}
	private void CreateExplosion(){

		var pillPosition =GetComponent<Collider>().gameObject.transform.position;
		var rotation = Quaternion.identity;

		//cria um clone do objeto explosion
		Instantiate(_explosion, pillPosition, rotation);

	}

}
