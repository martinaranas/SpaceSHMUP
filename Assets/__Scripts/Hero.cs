﻿using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	static public Hero		S;

	public float	gameRestartDelay = 2f;

	public float	speed = 30;
	public float	rollMult = -45;
	public float  	pitchMult=30;

	private float	_shieldLevel=1; // added underscore

	public bool	_____________________;
	public Bounds bounds;

	// Declare a new delegate type WeaponFireDelegate
	public delegate void WeaponFireDelegate();
	// Create a WeaponFireDelegate field named fireDelegate.
	public WeaponFireDelegate fireDelegate;

	void Awake(){
		S = this;
		bounds = Utils.CombineBoundsOfChildren (this.gameObject);
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");

		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.y += yAxis * speed * Time.deltaTime;
		transform.position = pos;
		
		bounds.center = transform.position;
		
		// constrain to screen
		Vector3 off = Utils.ScreenBoundsCheck(bounds,BoundsTest.onScreen);
		if (off != Vector3.zero) {  // we need to move ship back on screen
			pos -= off;
			transform.position = pos;
		}
		
		// rotate the ship to make it feel more dynamic
		transform.rotation =Quaternion.Euler(yAxis*pitchMult, xAxis*rollMult,0);

		// Use the fireDelegate to fire weapons
		// First, make sure the Axis("Jump") button is pressed
		// Then ensure that fireDelegate isn't null to avoid an error
		if (Input.GetAxis ("Jump") == 1 && fireDelegate != null) {
			fireDelegate ();
		}
	}

	// This variable holds a reference to the last triggering GameObject
	public GameObject lastTriggerGo = null;

	void OnTriggerEnter(Collider other) {
		// Find the tag of other.gameObject or its parent GameObjects
		GameObject go = Utils.FindTaggedParent (other.gameObject);
		// If there is a parent with a tag
		if (go != null) {
			// make sure it's not the same triggering go as last time
			if (go == lastTriggerGo) {
				return;
			}
			lastTriggerGo = go;

			if (go.tag =="Enemy") {
				// If the shield was triggered by an enemy
				// Decrease the level of the shield by 1
				shieldLevel--;
				// Destroy the enemy
				Destroy(go);
			} else {
			//Announce it
				print ("Triggered: " + go.name);
			}
			} else {
			// Otherwise announce the original other.gameObject
			print ("Triggered: " + other.gameObject.name); // Move this line here!
		}
	}

	public float shieldLevel {
		get {
			return (_shieldLevel);
		}
		set {
				_shieldLevel = Mathf.Min(value, 4);
				// If the shield is going to be set less than zero
				if (value < 0) {
					Destroy(this.gameObject);
					// Tell Main.S to restart the game after a delay
				Main.S.DelayedRestart(gameRestartDelay);
				}
			}
		}
	}


