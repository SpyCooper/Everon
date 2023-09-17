using System;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// came with the starter input assets

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool attacking;

		[Header("Movement Settings")]
		public bool analogMovement;

		// Toggle Camera
		public bool toggleCamera;
		public event EventHandler switchCamera;

		// Pause menu
		public bool pauseMenuActive;
		public event EventHandler togglePauseMenu;

		[Header("Mouse Cursor Settings")]
		// public bool cursorLocked = true;
		public bool cursorInputForLook = true;
		public bool lookKey = false;

		[Header("Camera Settings")]
		public int cameraDistanceDirection = 0;
		public event EventHandler ChangeCameraDistanceEvent;

		[Header("UI Menus Settings")]
		public bool abilityMenuPressed;
		public event EventHandler toggleAbilityMenu;
		
		public bool questMenuPressed;
		public event EventHandler toggleQuestMenu;
		
		public bool beastiaryMenuPressed;
		public event EventHandler toggleBeastiaryMenu;
		
		public bool inventoryMenuPressed;
		public event EventHandler toggleInventoryMenu;

		public bool interactPressed;

		// testing
		public bool doubleClicking;
		public event EventHandler DoubleClicked;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnToggleCamera(InputValue value)
		{
			ToggleCameraInput(value.isPressed);
		}
		
		public void OnOpenPauseMenu(InputValue value)
		{
			TogglePauseMenu(value.isPressed);
		}

		public void OnAttack(InputValue value)
		{
			AttackInput(value.isPressed);
		}

		public void OnDoubleClick(InputValue value)
		{
			DoubleClickInput(value.isPressed);
		}

		public void OnLookKey(InputValue value)
		{
			LookKeyInput(value.isPressed);
		}
		
		public void OnChangeCameraDistance(InputValue value)
		{
			ChangeCameraDistance(value.Get<float>());
		}
		
		public void OnAbilityMenu(InputValue value)
		{
			ToggleAbilityMenu(value.isPressed);
		}

		public void OnQuestMenu(InputValue value)
		{
			ToggleQuestMenu(value.isPressed);
		}
		
		public void OnBeastiaryMenu(InputValue value)
		{
			ToggleBeastiaryMenu(value.isPressed);
		}
		
		public void OnInventoryMenu(InputValue value)
		{
			ToggleInventoryMenu(value.isPressed);
		}
		
		public void OnInteract(InputValue value)
		{
			ToggleInteractMenu(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void AttackInput(bool newAttackState)
		{
			attacking = newAttackState;
		}

		public void DoubleClickInput(bool newDoubleClickState)
		{
			doubleClicking = newDoubleClickState;
			DoubleClicked?.Invoke(this, EventArgs.Empty);
			doubleClicking = false;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void LookKeyInput(bool newLookKeyState)
		{
			lookKey = newLookKeyState;
			// GetCameraState() = 1 means unlocked camera
			// GetCameraState() = 2 means locked camera
			if(lookKey && PlayerManager.Instance.currentGameData.cameraState == 1)
			{
				SetCursorState(true);
			}
			else if(PlayerManager.Instance.currentGameData.cameraState == 1)
			{
				SetCursorState(false);
			}
		}

		public void ToggleCameraInput(bool newCameraState)
		{
			toggleCamera = newCameraState;
			if(toggleCamera)
			{
				switchCamera?.Invoke(this, EventArgs.Empty);
			}
		}

		public void TogglePauseMenu(bool newPauseState)
		{
			pauseMenuActive = newPauseState;
			if(pauseMenuActive)
			{
				togglePauseMenu?.Invoke(this, EventArgs.Empty);
			}
		}

		public void ToggleAbilityMenu(bool newAbilityState)
		{
			abilityMenuPressed = newAbilityState;
			if(abilityMenuPressed)
			{
				// event has to be lowercase because of the function name
				toggleAbilityMenu?.Invoke(this, EventArgs.Empty);
			}
		}
		
		public void ToggleQuestMenu(bool newQuestState)
		{
			questMenuPressed = newQuestState;
			if(questMenuPressed)
			{
				// event has to be lowercase because of the function name
				toggleQuestMenu?.Invoke(this, EventArgs.Empty);
			}
		}

		public void ToggleBeastiaryMenu(bool newBeastiaryState)
		{
			beastiaryMenuPressed = newBeastiaryState;
			if(beastiaryMenuPressed)
			{
				// event has to be lowercase because of the function name
				toggleBeastiaryMenu?.Invoke(this, EventArgs.Empty);
			}
		}
		
		public void ToggleInventoryMenu(bool newInventoryState)
		{
			inventoryMenuPressed = newInventoryState;
			if(inventoryMenuPressed)
			{
				// event has to be lowercase because of the function name
				toggleInventoryMenu?.Invoke(this, EventArgs.Empty);
			}
		}

		public void ToggleInteractMenu(bool newInteractState)
		{
			interactPressed = newInteractState;
		}

		public void ChangeCameraDistance(float newCameraDirection)
		{
			if(newCameraDirection > 0)
			{
				cameraDistanceDirection = 1;
				ChangeCameraDistanceEvent?.Invoke(this, EventArgs.Empty);
			}
			else if(newCameraDirection < 0)
			{
				cameraDistanceDirection = -1;
				ChangeCameraDistanceEvent?.Invoke(this, EventArgs.Empty);
			}
		}

		// private void OnApplicationFocus(bool hasFocus)
		// {
		// 	SetCursorState(cursorLocked);
		// }

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		public void setMouseLock(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}