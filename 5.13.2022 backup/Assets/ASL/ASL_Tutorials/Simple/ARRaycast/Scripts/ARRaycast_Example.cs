using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System;

namespace SimpleDemos
{
    /// <summary> 
    /// A demo showcasing how to use two types of raycasts in AR. A standard physics raycast can be used normally in AR 
    /// as long as it is provided with the touch position. A Pose ASL raycast receives the Pose of any object the raycast hits.
    /// 
    /// This demo allows AR users to raycast on objects within an AR space, as well as allowing a secondary PC user to spectate and select ASL objects.
    /// At least one AR user must be present when using this demo, as the PC user cannot create/view AR planes and thus cannot create spheres. Additionally, 
    /// for simplicity, the PC user cannot move or rotate, however, these values can be changed if running within the Unity editor.
    /// 
    /// Users switch between two modes, Select mode or Create mode. In Create mode, AR users can create spheres (ASL objects) on scanned AR planes. 
    /// In Select mode, AR users can select AR planes as well as spheres, and PC users can select created spheres. Information is displayed to the user via text
    /// and informs the user of the object they have hit, or if the raycast hit the UI, or did not hit any object.
    /// 
    /// Video Example: https://drive.google.com/file/d/1-tFcISyVAa5TGP3zB6ys4ay6Tqq7fy_n/view?usp=sharing
    /// </summary>
    public class ARRaycast_Example : MonoBehaviour
    {
        /// <summary>Determines which mode the example is in: Select or Create</summary>
        public Dropdown m_ModeDropDown;

        /// <summary>Text that displays scene information to the user</summary>
        public Text m_DisplayInformation;

        /// <summary>Camera for the PC User</summary>
        public Camera m_PCCamera;

        /// <summary>GameObject that is currently selected</summary>
        private GameObject m_SelectedObject;

        /// <summary>Vector2 position of last touch</summary>
        private Vector2 m_TouchPosition;

        private Camera _camera;

        private ARRaycastControls _rcc;

        private void Awake()
        {
            _rcc = new ARRaycastControls();
        }

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            _rcc.Default.Raycast.performed += Raycast;
            _rcc.Default.Enable();
        }

        

        private void OnDisable()
        {
            _rcc.Default.Raycast.performed -= Raycast;
        }

        /// <summary>Determines if user is on PC</summary>
        private static bool IsPC
        {
            get
            {
                return (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor);
            }
        }

        /// <summary>
        /// Called on start up - creates the world origin at the origin. For simplicity, this demo creates the origin at the exact location the 
        /// AR user starts at, which may cause issues if the AR user starts in one location and then moves to another area and begins creating 
        /// objects - the objects may be far from the PC user in this case.
        /// For a better representation of accurate world origin creation, check out the ARWorldOrigin demo.
        /// </summary>
        void Start()
        {

            if (ASL.GameLiftManager.GetInstance().AmLowestPeer())
            {
                ASL.ASLHelper.InstantiateASLObject("SimpleDemoPrefabs/WorldOriginCloudAnchorObject", Vector3.zero, Quaternion.identity, string.Empty, string.Empty, SpawnWorldOrigin);
            }

            if(IsPC)
            {
                //If on PC, set normal Camera as Main Camera tag, remove AR Camera
                m_PCCamera.gameObject.tag = "MainCamera";
                Destroy(GameObject.Find("AR Camera"));
            }

            _camera = Camera.main;
        }

        /// <summary>
        /// Spawns the world origin cloud anchor after the world origin object visualizer has been created (blue cube)
        /// </summary>
        /// <param name="_worldOriginVisualizationObject">The game object that represents the world origin</param>
        private static void SpawnWorldOrigin(GameObject _worldOriginVisualizationObject)
        {
            _worldOriginVisualizationObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                ASL.ASLHelper.CreateARCoreCloudAnchor(Pose.identity, _worldOriginVisualizationObject.GetComponent<ASL.ASLObject>(), _waitForAllUsersToResolve: false);
            });
        }

        /// <summary>
        /// The create object call back for the normal spheres - used to inform the user the sphere was created and to scale it down so it matches the size of the other objects
        /// </summary>
        /// <param name="_sphere">The game object that was just created</param>
        public static void SpawnSphere(GameObject _sphere)
        {
            //Scale the sphere down to 5cm (the same size as the other objects)
            _sphere.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                _sphere.GetComponent<ASL.ASLObject>().SendAndSetLocalScale(new Vector3(.05f, .05f, .05f));
            });
        }

        /// <summary>
        /// Selects the given object
        /// </summary>
        /// <param name="_myGameObject">The game object to be selected</param>
        private void SelectObject(GameObject _myGameObject)
        {
            DeselectObject();
            m_SelectedObject = _myGameObject;
        }

        /// <summary>
        /// Deselects the currently selected object
        /// </summary>
        private void DeselectObject()
        {
            if (m_SelectedObject != null)
            {
                m_SelectedObject = null;
            }
        }

        private void Raycast(InputAction.CallbackContext obj)
        {
            var pointer = Pointer.current;

            int pointerID = -1;
            m_TouchPosition = pointer.position.ReadValue();
            if (!IsPC)
            {
                pointerID = pointer.deviceId;
            }

            //Check if touch on UI
            if (EventSystem.current.IsPointerOverGameObject(pointerID))
            {
                m_DisplayInformation.text = "UI Hit";
                return;
            }

            //Raycast on touch position
            RaycastHit hitObject;

            var ray = new Ray(_camera.transform.position, _camera.transform.forward);

            //Check for raycast hit
            if (Physics.Raycast(ray, out hitObject))
            {
                if (hitObject.collider != null)
                {
                    m_DisplayInformation.text = "Hit: " + hitObject.collider.gameObject.name + " (ID: " + hitObject.collider.gameObject.GetInstanceID() + ")";

                    //Select
                    if (m_ModeDropDown.value == 0)
                    {
                        if (m_SelectedObject == null)
                        {
                            //Select object at touch position
                            SelectObject(hitObject.collider.gameObject);
                        }
                    }
                    //Create
                    else if (m_ModeDropDown.value == 1)
                    {
                        Pose? touchPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(m_TouchPosition);
                        if (touchPose != null)
                        {
                            //Create sphere at touch position
                            ASL.ASLHelper.InstantiateASLObject(PrimitiveType.Sphere, (Vector3)touchPose?.position, Quaternion.identity, string.Empty, string.Empty, SpawnSphere);
                        }
                    }
                }
                else
                {
                    m_DisplayInformation.text = "No Hit";
                }
            }
        }
    }
}