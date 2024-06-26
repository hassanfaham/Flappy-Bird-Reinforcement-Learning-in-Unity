/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour {

        private const float JUMP_AMOUNT = 90f;

    private static Bird instance;

    public static Bird GetInstance() {
        return instance;
    }

    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;

    private Rigidbody2D birdRigidbody2D;
    private State state;

    private enum State {
        WaitingToStart,
        Playing,
        Dead
    }

    private void Awake() {
        instance = this;
        birdRigidbody2D = GetComponent<Rigidbody2D>();
        birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        state = State.Playing;
    }

    private void Update() {
        switch (state) {
        default:
        // case State.WaitingToStart:
        //     if (TestInput()) {
        //         // Start playing
        //         state = State.Playing;
        //         birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        //         Jump();
        //         if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
        //     }
        //     break;
        case State.Playing:
            if (TestInput()) {
                Jump();
            }

            // Rotate bird as it jumps and falls
            transform.eulerAngles = new Vector3(0, 0, birdRigidbody2D.velocity.y * .15f);
            break;
        case State.Dead:
            break;
        }
    }

    private bool TestInput() {
        return 
            Input.GetKeyDown(KeyCode.Space) || 
            Input.GetMouseButtonDown(0) ||
            Input.touchCount > 0;
    }

    public void Reset() {
        birdRigidbody2D.velocity = Vector2.zero;
        birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        transform.position = Vector3.zero;
        state = State.Playing;
    }

    public void Jump() {
        birdRigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
        SoundManager.PlaySound(SoundManager.Sound.BirdJump);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        SoundManager.PlaySound(SoundManager.Sound.Lose);
        if (OnDied != null) OnDied(this, EventArgs.Empty);
        SceneManager.LoadScene("GameScene");
    }

    public float GetVelocityY() {
        return birdRigidbody2D.velocity.y;
    }



}
