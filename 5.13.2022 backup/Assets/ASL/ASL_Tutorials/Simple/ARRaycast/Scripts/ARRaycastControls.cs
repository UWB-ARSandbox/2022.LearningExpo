//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/ASL/ASL_Tutorials/Simple/ARRaycast/Scripts/ARRaycastControls.inputactions
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

public partial class @ARRaycastControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ARRaycastControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ARRaycastControls"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""1854ae13-2658-44bf-bdfc-3a9bc5e01634"",
            ""actions"": [
                {
                    ""name"": ""Raycast"",
                    ""type"": ""Button"",
                    ""id"": ""3a932dbb-de30-4fee-a5b8-f0381c85e5d1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0c1b25f8-7f0f-40ae-a937-548092afa270"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Raycast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Default
        m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
        m_Default_Raycast = m_Default.FindAction("Raycast", throwIfNotFound: true);
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

    // Default
    private readonly InputActionMap m_Default;
    private IDefaultActions m_DefaultActionsCallbackInterface;
    private readonly InputAction m_Default_Raycast;
    public struct DefaultActions
    {
        private @ARRaycastControls m_Wrapper;
        public DefaultActions(@ARRaycastControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Raycast => m_Wrapper.m_Default_Raycast;
        public InputActionMap Get() { return m_Wrapper.m_Default; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
        public void SetCallbacks(IDefaultActions instance)
        {
            if (m_Wrapper.m_DefaultActionsCallbackInterface != null)
            {
                @Raycast.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnRaycast;
                @Raycast.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnRaycast;
                @Raycast.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnRaycast;
            }
            m_Wrapper.m_DefaultActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Raycast.started += instance.OnRaycast;
                @Raycast.performed += instance.OnRaycast;
                @Raycast.canceled += instance.OnRaycast;
            }
        }
    }
    public DefaultActions @Default => new DefaultActions(this);
    public interface IDefaultActions
    {
        void OnRaycast(InputAction.CallbackContext context);
    }
}
