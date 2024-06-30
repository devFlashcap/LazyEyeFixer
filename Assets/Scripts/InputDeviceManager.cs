using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class InputDeviceManager : MonoBehaviour
{
    public InputDevice _leftController;
    public InputDevice _rightController;

    private void InitializeInputDevices()
    {
        if (!_leftController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref _leftController);
        if (!_rightController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref _rightController);
    }

    private void InitializeInputDevice(InputDeviceCharacteristics inputDeviceCharacteristics, ref InputDevice inputDevice)
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, inputDevices);
        if(inputDevices.Count > 0)
        {
            inputDevice = inputDevices[0];
        }
    }

    void Update()
    {
        if (!_leftController.isValid || !_rightController.isValid)
            InitializeInputDevices();
    }
}
