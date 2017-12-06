using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using SimpleJSON;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        public MenuController gameController;
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }

        public void ResetPosition()
        {
            transform.position = new Vector3(0.0f, 4.0f, 0.0f);
        }

        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (Input.GetKeyDown("r"))
            {
                ResetPosition();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (gameController.GetIfOffline() == false)
            {
                if (other.gameObject.CompareTag("RightAnswer"))
                {
                    JSONNode vals = JSON.Parse("{\"correct\" : \"" + "add_score" + "\" }");
                    // ask EngAGe to assess the action based on the config file
                    StartCoroutine(EngAGe.E.assess("burns_correctAnswer", vals, gameController.ActionAssessed));
                }
                else if (other.gameObject.CompareTag("WrongAnswer"))
                {
                    JSONNode vals = JSON.Parse("{\"correct\" : \"" + "subtract_score" + "\" }");
                    // ask EngAGe to assess the action based on the config file
                    StartCoroutine(EngAGe.E.assess("burns_correctAnswer", vals, gameController.ActionAssessed));
                }
            }
        }

        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
