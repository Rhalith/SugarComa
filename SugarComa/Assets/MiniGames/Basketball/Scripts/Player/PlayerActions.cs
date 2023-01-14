//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/MiniGames/Basketball/Scripts/Player/PlayerActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Assets.MiniGames.Basketball.Scripts
{
    public partial class @PlayerActions : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActions"",
    ""maps"": [
        {
            ""name"": ""PlayerInputs"",
            ""id"": ""722350d0-3201-4657-adbe-10a4f2ad006a"",
            ""actions"": [
                {
                    ""name"": ""Throw"",
                    ""type"": ""Value"",
                    ""id"": ""3c0505e3-a22a-4463-a863-308793218af2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""67d879ff-ca83-41e1-8210-c4c6a5bd5cda"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // PlayerInputs
            m_PlayerInputs = asset.FindActionMap("PlayerInputs", throwIfNotFound: true);
            m_PlayerInputs_Throw = m_PlayerInputs.FindAction("Throw", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }
        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }
        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // PlayerInputs
        private readonly InputActionMap m_PlayerInputs;
        private IPlayerInputsActions m_PlayerInputsActionsCallbackInterface;
        private readonly InputAction m_PlayerInputs_Throw;
        public struct PlayerInputsActions
        {
            private @PlayerActions m_Wrapper;
            public PlayerInputsActions(@PlayerActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Throw => m_Wrapper.m_PlayerInputs_Throw;
            public InputActionMap Get() { return m_Wrapper.m_PlayerInputs; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerInputsActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerInputsActions instance)
            {
                if (m_Wrapper.m_PlayerInputsActionsCallbackInterface != null)
                {
                    @Throw.started -= m_Wrapper.m_PlayerInputsActionsCallbackInterface.OnThrow;
                    @Throw.performed -= m_Wrapper.m_PlayerInputsActionsCallbackInterface.OnThrow;
                    @Throw.canceled -= m_Wrapper.m_PlayerInputsActionsCallbackInterface.OnThrow;
                }
                m_Wrapper.m_PlayerInputsActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Throw.started += instance.OnThrow;
                    @Throw.performed += instance.OnThrow;
                    @Throw.canceled += instance.OnThrow;
                }
            }
        }
        public PlayerInputsActions @PlayerInputs => new PlayerInputsActions(this);
        public interface IPlayerInputsActions
        {
            void OnThrow(InputAction.CallbackContext context);
        }
    }
}
