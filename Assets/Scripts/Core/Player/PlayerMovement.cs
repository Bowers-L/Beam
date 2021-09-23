using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEditor;
using Beam.Utility;

namespace Beam.Core.Player
{
    #region Editor
    public class ReadOnlyAttribute : PropertyAttribute
    {

    }

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
                                                GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position,
                                   SerializedProperty property,
                                   GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }

    public enum PlayerMovementEditorTypes
    {

    }

    //https://blog.terresquall.com/2020/07/organising-your-unity-inspector-fields-with-a-dropdown-filter/
    //This is extremely overkill for this script, but I thought it might be useful for future scripts.
    [CustomEditor(typeof(PlayerMovement))]
    public class PlayerMovementEditor : Editor
    {
        // The various categories the editor will display the variables in
        public enum DisplayCategory
        {
                MovementParameters,
                Jumping,
                GroundCheck,
                Misc
        }
        // The enum field that will determine what variables to display in the Inspector
        public DisplayCategory categoryToDisplay;

        //This is what actually makes the editor
        public override void OnInspectorGUI()
        {
            // Display the enum popup in the inspector
            categoryToDisplay = (DisplayCategory)EditorGUILayout.EnumPopup("Display", categoryToDisplay);

            // Create a space to separate this enum popup from the other variables 
            EditorGUILayout.Space();

            // Check the value of the enum and display variables based on it
            switch (categoryToDisplay)
            {
                case DisplayCategory.MovementParameters:
                    DisplayMovementParametersInfo();
                    break;

                case DisplayCategory.Jumping:
                    DisplayJumpingInfo();
                    break;
                case DisplayCategory.GroundCheck:
                    DisplayGroundCheckInfo();
                    break;
                case DisplayCategory.Misc:
                    DisplayMiscInfo();
                    break;

            }

            // Save all changes made on the Inspector
            serializedObject.ApplyModifiedProperties();
        }

        void DisplayMovementParametersInfo()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("moveParams"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("vel"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("velAtLastJump"));
        }

        void DisplayJumpingInfo()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gravity"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpHeight"));
        }

        void DisplayGroundCheckInfo()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundCheck"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundDistance"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundMask"));
        }

        void DisplayMiscInfo()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("forceMag"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("noClip"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("killPlaneY"));
        }
    }
#endregion

    //https://www.youtube.com/watch?v=_QajrabyTJc
    public class PlayerMovement : MonoBehaviour
    {

        //Parameters used to calculate the velocity
        [System.Serializable]

        public struct MovementParameters
        {
            public float maxMoveSpeed;

            public float accelGround;
            public float accelAir;

            //Controls how fast the player slows down when exceeding the speed cap.
            [Range(0f, 1f)]
            public float overCapSmoothing;
            //The speed cap while in the air as a percentage of the liftoff speed
            public float airCapWeight;

            //raw movement from player's input
            //x = right movement, y = forward movement
            public Vector2 rawMoveInput {
                get;
                internal set;
            }
        }

        public CharacterController controller;
        
        //might need to put gravity in an external game manager if other things use it.
        public float gravity = 1f;
        public float jumpHeight = 3f;

        public Transform groundCheck;
        public float groundDistance = 0.1f;
        public LayerMask groundMask;

        public float forceMag = 10.0f;  //used for physics when the player collides with a rigidbody.

        public bool noClip; //for debugging purposes

        public float killPlaneY;

        [SerializeField]
        [ReadOnly]
        private Vector3 vel;
        [SerializeField]
        [ReadOnly]
        private Vector3 velAtLastJump;

        [SerializeField]
        private MovementParameters moveParams;

        public Vector3 Velocity
        {
            get
            {
                return vel;
            }

            set
            {
                vel = value;
            }
        }


        [SerializeField]
        public bool isGrounded
        {
            get
            {
                //Multiple groundchecks around the player's capsule to cover the more complicated cases (such as ascending stairs)
                float groundCheckPrecision = 0.1f;
                bool center = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
                //bool xplus = Physics.CheckSphere(groundCheck.position + controller.radius * groundCheckPrecision * Vector3.right, groundDistance, groundMask);
                //bool xminus = Physics.CheckSphere(groundCheck.position + controller.radius * groundCheckPrecision * Vector3.right, groundDistance, groundMask);
                //bool zplus = Physics.CheckSphere(groundCheck.position + controller.radius * groundCheckPrecision * Vector3.forward, groundDistance, groundMask);
                //bool zminus = Physics.CheckSphere(groundCheck.position + controller.radius * groundCheckPrecision * Vector3.back, groundDistance, groundMask);
                return center;
            }
        }

        private void Start()
        {
            if (GetComponent<CharacterController>() == null)
            {
                Debug.LogError("Player should have a CharacterController component.");
            }
        }

        // Update is called once per frame
        void Update()
        {
            //float deltaX = Input.GetAxis("Horizontal");
            //float deltaZ = Input.GetAxis("Vertical");

            updateVelocity();

            GetComponent<CharacterController>().enabled = !noClip;
            if (noClip)
            {
                transform.position += vel * Time.deltaTime;
            } else
            {
                controller.Move(vel * Time.deltaTime);
            }

            if (transform.position.y <= killPlaneY)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            moveParams.rawMoveInput = ctx.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                velAtLastJump = vel;
                vel.y = isGrounded ? Mathf.Sqrt(2f * jumpHeight * gravity) : vel.y;
                
            }

            /* Don't know if this should be kept since there's problems with external forces.
            if (ctx.canceled && vel.y > 0f)
            {
                //release jump button and player is still moving upwards.
                vel.y = 0f;
            }
            */
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //Implement collision physics manually because character controller doesn't come with rigidbody.
            if (hit.rigidbody != null) {
                hit.rigidbody.AddForceAtPosition(vel * forceMag, hit.point);
            }
        }

        //Updates the player's velocity, taking into account smoothing, player rotation, input, etc.
        private void updateVelocity()
        {
            #region Ground Check
            //update player's y velocity based on gravity
            if (isGrounded)
            {
                //Stops the player when they hit the ground. 
                if (vel.y < 0)
                {
                    vel.y = 0;
                }
            }
            else
            {
                vel.y -= gravity * Time.deltaTime;
            }
            #endregion

            //Get the acceleration vector from the player's imput.
            Vector3 moveAccel;
            if (noClip)
            {
                MouseCameraControl camera = GetComponentInChildren<MouseCameraControl>();
                moveAccel = moveParams.accelGround * Vector3.Normalize((camera.transform.right * moveParams.rawMoveInput.x + camera.transform.forward * moveParams.rawMoveInput.y));
            }
            else
            {
                moveAccel = (isGrounded ? moveParams.accelGround : moveParams.accelAir) * Vector3.Normalize((transform.right * moveParams.rawMoveInput.x + transform.forward * moveParams.rawMoveInput.y));
            }

            Vector3 oldXZVel = new Vector3(vel.x, noClip ? vel.y : 0, vel.z);
            Vector3 newXZVel = moveAccel * Time.deltaTime + oldXZVel;

            //Cap the player's speed based on parameter weights/player input
            #region Speed Cap
            float playerSpeedCap = isGrounded ? moveParams.maxMoveSpeed * moveParams.rawMoveInput.magnitude : velAtLastJump.magnitude * moveParams.airCapWeight;
            Vector3 newVelDir = Vector3.Normalize(newXZVel);
            float newVelMagAdjusted;
            if (newXZVel.magnitude > playerSpeedCap)
            {
                newVelMagAdjusted = Mathf.Lerp(oldXZVel.magnitude, playerSpeedCap, moveParams.overCapSmoothing);
            }
            else
            {
                newVelMagAdjusted = newXZVel.magnitude;
            }

            newXZVel = newVelMagAdjusted * newVelDir;
            #endregion

            vel.x = newXZVel.x;
            vel.z = newXZVel.z;

            if (noClip)
            {
                vel.y = newXZVel.y;
            }
        }
    }
}
