﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

	private Rigidbody2D rb;

	[Range (1, 2)]
	public int playerNumber;

	public float speed;
	public float pullSpeed;
	public float jumpForce;
	public float jumpSpeed;
	public float jumpRate;
	public float gravityForce = 5;
	public float maxFallSpeed = 10;
	public float fallForce;

	public int airJumps;
	public float maxTime;

	public bool onGround;
	public Vector2 velocity;
	public PlayerState currentState;

	private Animator anim;

	float horizontalDir = 0;
	float verticalDir = 0;
	float airTime;
	bool jumped = false;


	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponentInChildren<Animator> ();
		currentState = PlayerState.STAND;
		onGround = false;
		airTime = 0;
		if (playerNumber == 2) {
			transform.localScale = new Vector3(-1, 1, 1);
		}
	}

	void FixedUpdate () {
		velocityUpdate ();
		setFacing (horizontalDir);
		rb.velocity = velocity;
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.CompareTag ("Ground")) {
			velocity.y = -1f;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.CompareTag ("Ground")) {
			onGround = true;
			//anim.Play ("Aterrizaje");
			airJumps = 2;
			airTime = maxTime;
			jumped = false;
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.gameObject.CompareTag ("Ground")) {
			onGround = false;
		}
	}

	void velocityUpdate () {
		switch (currentState) {
		case PlayerState.STAND:
			anim.Play ("IDE");

			if (playerNumber == 1) {
				velocity.x = -DifficultyService.difficulty * 2;
				setFacing (horizontalDir);
			}else if(playerNumber == 2){
				velocity.x = DifficultyService.difficulty * 2;
				setFacing (horizontalDir);
			}

			jumpSpeed = jumpRate;

			if (!onGround) {
				currentState = PlayerState.JUMPING;
				break;
			}
			if (Input.GetAxis ("Horizontal" + playerNumber) != 0) {
				currentState = PlayerState.WALKING;
			}

			if (Input.GetButtonDown ("Jump" + playerNumber)) {
				anim.Play ("SALTO");
				airJumps--;
				currentState = PlayerState.JUMPING;
				break;
			}
			break;

		case PlayerState.JUMPING:
			progresiveJump ();
			anim.Play ("SaltoIDE");
			velocity.y -= gravityForce * Time.deltaTime;
			velocity.y = Mathf.Max (velocity.y, -maxFallSpeed);
			horizontalDir = Input.GetAxis ("Horizontal" + playerNumber);
			verticalDir = Input.GetAxis ("Vertical" + playerNumber);

			if (verticalDir == -1) {
				velocity.y -= gravityForce * Time.deltaTime * fallForce;
			}
			if (horizontalDir == 0) {
				if (playerNumber == 1) {
					velocity.x = -DifficultyService.difficulty * 2;
					setFacing (horizontalDir);
				}else if(playerNumber == 2){
					velocity.x = DifficultyService.difficulty * 2;
					setFacing (horizontalDir);
				}
			} else {
				if (playerNumber == 1 && horizontalDir == -1) {
					velocity.x = speed * horizontalDir - DifficultyService.difficulty * 2;
				} else if (playerNumber == 2 && horizontalDir == 1) {
					velocity.x = speed * horizontalDir + DifficultyService.difficulty * 2;
				} else {
					velocity.x = speed * horizontalDir;
				}
			}

			if (onGround) {
				if (horizontalDir == 0) {
					currentState = PlayerState.STAND;
				} else {
					currentState = PlayerState.WALKING;
				}
			} else {
				if (jumped) {
					if (Input.GetButtonDown ("Jump" + playerNumber) && airJumps > 0) {
						airJumps--;
						airTime = maxTime;
						jumpSpeed = jumpRate;
						currentState = PlayerState.DOUBLEJUMPING;
						break;
					}
				}
			}
			break;

		case PlayerState.DOUBLEJUMPING:
			progresiveJump ();
			velocity.y -= gravityForce * Time.deltaTime;
			velocity.y = Mathf.Max (velocity.y, -maxFallSpeed);
			horizontalDir = Input.GetAxis ("Horizontal" + playerNumber);
			verticalDir = Input.GetAxis ("Vertical" + playerNumber);

			if (verticalDir == -1) {
				velocity.y -= gravityForce * Time.deltaTime * fallForce;
			}
			if (horizontalDir == 0) {
				if (playerNumber == 1) {
					velocity.x = -DifficultyService.difficulty * 2;
					setFacing (horizontalDir);
				}else if(playerNumber == 2){
					velocity.x = DifficultyService.difficulty * 2;
					setFacing (horizontalDir);
				}
			} else {
				if (playerNumber == 1 && horizontalDir == -1) {
					velocity.x = speed * horizontalDir - DifficultyService.difficulty * 2;
				} else if (playerNumber == 2 && horizontalDir == 1) {
					velocity.x = speed * horizontalDir + DifficultyService.difficulty * 2;
				} else {
					velocity.x = speed * horizontalDir;
				}
			}
			if (onGround) {
				if (horizontalDir == 0) {
					currentState = PlayerState.STAND;
				} else {
					currentState = PlayerState.WALKING;
				}
			}

			break;

		case PlayerState.WALKING:
			if (playerNumber == 1) {
				setFacing (horizontalDir);
			} else if (playerNumber == 2) {
				setFacing (horizontalDir);
			}
			anim.Play ("CarreraNew");
			jumpSpeed = jumpRate;
			horizontalDir = Input.GetAxis ("Horizontal" + playerNumber);

			if (horizontalDir == 0) {
				currentState = PlayerState.STAND;
			}

			if (Input.GetButtonDown ("Jump" + playerNumber)) {
				airJumps--;
				currentState = PlayerState.JUMPING;
				break;
			}

			if (playerNumber == 1 && horizontalDir == -1) {
				velocity.x = speed * horizontalDir - DifficultyService.difficulty * 2;
			} else if (playerNumber == 2 && horizontalDir == 1) {
				velocity.x = speed * horizontalDir + DifficultyService.difficulty * 2;
			} else {
				velocity.x = speed * horizontalDir;
			}
			if (!onGround) {
				currentState = PlayerState.JUMPING;
			}
			break;
		}
	}

	void progresiveJump(){
		if (Input.GetButton ("Jump" + playerNumber) && airTime > 0) {
			velocity.y = Mathf.Min (jumpForce * jumpSpeed, jumpForce);
			airTime -= 0.25f;
			if (jumpSpeed <= 1) {
				jumpSpeed += 0.05f;
			}
			jumped = true;
		}
	}

	void setFacing (float horizontalDir) {
		Vector3 vScale = Vector3.one;
		if (playerNumber == 1) {
			if (horizontalDir > 0) {
				vScale.x = 1;
			} else if (horizontalDir < 0) {
				vScale.x = -1;
			} else {
				vScale.x = 1;
			}
		} else if (playerNumber == 2) {
			if (horizontalDir > 0) {
				vScale.x = 1;
			} else if (horizontalDir < 0) {
				vScale.x = -1;
			} else {
				vScale.x = -1;
			}
		}
		transform.localScale = vScale;
	}

	public enum PlayerState {
		STAND,
		JUMPING,
		DOUBLEJUMPING,
		WALKING}

	;
}
