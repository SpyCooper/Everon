using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class ToggleCamera : MonoBehaviour
{
    // handles the camera set up, distance, which is active, etc

    private const string PLAYER_PREFS_CAMERA_STATE = "CameraState";

    [SerializeField] private CinemachineVirtualCamera lockedCamera;
    [SerializeField] private CinemachineVirtualCamera  unlockedCamera;
    [SerializeField] private LayerMask aimColliderMask;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;

    // sets up an enum for the camera state
    // translates to unlocked = 1, locked = 2
    private enum CameraState
    {
        unlocked,
        locked,
    }

    private CameraState cameraState;

    //used to adjust the distance of the cameras
    private float minDistance = 3f;
    private float maxDistance = 15f;

    // on Awake
    private void Awake()
    {
        // grabs the start assets inputs and third person controller from the game object
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    // on Start
    private void Start()
    {
        // listens to events that are needed for the cameras
        PlayerSettings.Instance.unlockedSensitivityChanged += PlayerSettings_UnlockedSensitivityChanged;
        PlayerSettings.Instance.lockedSensitivityChanged += PlayerSettings_LockedSensitivityChanged;
        PlayerSettings.Instance.lockedCameraSideChanged += PlayerSettings_LockedCameraSideChanged;
        starterAssetsInputs.ChangeCameraDistanceEvent += StarterAssetsInputs_ChangeCameraDistanceEvent;
        starterAssetsInputs.switchCamera += StarterAssetsInputs_SwitchCamera;

        // gets the camera distance
        // both are the same so only 1 variable is needed
        lockedCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = PlayerManager.Instance.currentGameData.cameraDistance;
        unlockedCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = PlayerManager.Instance.currentGameData.cameraDistance;

        // set up the camera state for the character
        int cam = PlayerManager.Instance.currentGameData.cameraState;
        if(cam == 1)
        {
            cameraState = CameraState.unlocked;
            SetUnlockedCam();
        }
        else if(cam == 2)
        {
            cameraState = CameraState.locked;
            SetLockedCameraSide();
            SetLockedCam();
        }
    }
    
    // on Update
    void Update()
    {
        // if the current camera state is locked
        if(cameraState == CameraState.locked)
        {
            // makes the player look foward

            // set the mouse world position as zero
            Vector3 mouseWorldPosition = Vector3.zero;

            // find the screen center point and casts a ray from the center point forwars
            Vector2 screenCenterpoint = new Vector2(Screen.width/2f, Screen.height/2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterpoint);
            mouseWorldPosition = ray.GetPoint(999);

            // takes the position of the and follows where the mouse goes
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            // linearly rotates toward the position
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
    }

    // set the active camera as the locked camera
    private void SetLockedCam()
    {
        // turns off the unlocked camera and turns on the locked camera
        lockedCamera.gameObject.SetActive(true);
        unlockedCamera.gameObject.SetActive(false);

        // sets the locked camera sensitivity
        thirdPersonController.SetLookSensitivity(PlayerSettings.Instance.GetLockedSensitivity()/100);
        thirdPersonController.SetLockedCamera();

        // camera state is locked
        cameraState = CameraState.locked;

        // sets the current camera state as 2, or locked in the game data
        PlayerManager.Instance.currentGameData.cameraState = 2;
    }
    
    // set the active camera as the unlocked camera
    private void SetUnlockedCam()
    {
        // turns on the unlocked camera and turns off the locked camera
        lockedCamera.gameObject.SetActive(false);
        unlockedCamera.gameObject.SetActive(true);
        
        // sets the unlocked camera sensitivity
        thirdPersonController.SetLookSensitivity(PlayerSettings.Instance.GetUnlockedSensitivity()/100);
        thirdPersonController.SetUnlockedCamera();

        // camera state is unlocked
        cameraState = CameraState.unlocked;

        // sets the current camera state as 1, or unlocked in the game data
        PlayerManager.Instance.currentGameData.cameraState = 1;
    }

    // when the switch camera event is activated, switched the camera
    private void StarterAssetsInputs_SwitchCamera(object sender, System.EventArgs e)
    {
        // if the current camera is unlocked, switched to teh locked camera and sets the side
        if(cameraState == CameraState.unlocked)
        {
            SetLockedCam();
            if(PlayerSettings.Instance.GetLockedCameraSide() == "Left")
            {
                lockedCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraSide = 0f;
            }
            else
            {
                lockedCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraSide = 1f;
            }

            // toggle camera needs to be set to false because of it's type
            starterAssetsInputs.toggleCamera = false;
        }
        //
        else if(cameraState == CameraState.locked)
        {
            SetUnlockedCam();
            // toggle camera needs to be set to false because of it's type
            starterAssetsInputs.toggleCamera = false;
        }
    }
    
    // when UnlockedSensitivityChanged event is active, the sensitivity of the unlocked camera is adjusted
    private void PlayerSettings_UnlockedSensitivityChanged(object sender, System.EventArgs e)
    {
        if(cameraState == CameraState.unlocked)
        {
            thirdPersonController.SetLookSensitivity(PlayerSettings.Instance.GetUnlockedSensitivity()/100);
        }
    }

    // when LockedSensitivityChanged event is active, the sensitivity of the locked camera is adjusted
    private void PlayerSettings_LockedSensitivityChanged(object sender, System.EventArgs e)
    {
        if(cameraState == CameraState.locked)
        {
            thirdPersonController.SetLookSensitivity(PlayerSettings.Instance.GetLockedSensitivity()/100);
        }
    }

    // when LockedCameraSideChanged event is active, the side of the locked camera is adjusted
    private void PlayerSettings_LockedCameraSideChanged(object sender, System.EventArgs e)
    {
        SetLockedCameraSide();
    }

    // sets the locked camera side
    private void SetLockedCameraSide()
    {
        if(PlayerSettings.Instance.GetLockedCameraSide() == "Left")
        {
            lockedCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraSide = 0f;
        }
        else
        {
            lockedCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraSide = 1f;
        }
    }
    
    // when ChangeCameraDistanceEvent event is active, the side of the locked camera is adjusted
    private void StarterAssetsInputs_ChangeCameraDistanceEvent(object sender, System.EventArgs e)
    {
        // gets the current camera distance
        float distance = lockedCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance;

        // checks to see if the adjustment will not go over the max distance
        if(starterAssetsInputs.cameraDistanceDirection > 0 && distance < maxDistance)
        {
            // adds 0.2f to the camera distance
            // both cameras are set to the same distance and then the distance is saved
            lockedCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = distance + 0.2f;
            unlockedCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = distance + 0.2f;
            PlayerManager.Instance.currentGameData.cameraDistance = lockedCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance;
        }
        // checks to see if the adjustment will not go under the min distance
        else if(starterAssetsInputs.cameraDistanceDirection < 0 && distance > minDistance)
        {
            // subtracts 0.2f to the camera distance
            // both cameras are set to the same distance and then the distance is saved
            lockedCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = distance - 0.2f;
            unlockedCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = distance - 0.2f;
            PlayerManager.Instance.currentGameData.cameraDistance = lockedCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance;
        }
    }
}
