﻿using System.Windows;
using System.Windows.Controls;
using Ciribob.DCS.SimpleRadio.Standalone.Client.Input;
using Ciribob.DCS.SimpleRadio.Standalone.Client.Settings;

namespace Ciribob.DCS.SimpleRadio.Standalone.Client.UI
{
    /// <summary>
    ///     Interaction logic for InputBindingControl.xaml
    /// </summary>
    public partial class InputBindingControl : UserControl
    {
        private InputDeviceManager _inputDeviceManager;

        public InputBindingControl()
        {
            InitializeComponent();
        }

        public InputDeviceManager InputDeviceManager
        {
            get { return _inputDeviceManager; }
            set
            {
                _inputDeviceManager = value;
                LoadInputSettings();
            }
        }

        public InputBinding ControlInputBinding { get; set; }
        public InputBinding ModifierBinding { get; set; }
        public string InputName { get; set; }

        public void LoadInputSettings()
        {
            DeviceLabel.Content = InputName;
            ModifierLabel.Content = InputName + " Modifier";
            ModifierBinding = (InputBinding) ((int) ControlInputBinding) + 100; //add 100 gets the enum of the modifier

            var currentInputProfile = SettingsStore.Instance.InputSettingsStore.GetCurrentInputProfile();

            if (currentInputProfile != null)
            {
                var devices = currentInputProfile;
                if (currentInputProfile.ContainsKey(ControlInputBinding))
                {
                    var button = devices[ControlInputBinding].Button;
                    DeviceText.Text = button < 128 ? (button+1).ToString() : "POV " + (button - 127); //output POV info
                    Device.Text = devices[ControlInputBinding].DeviceName;
                }
                else
                {
                    DeviceText.Text = "None";
                    Device.Text = "None";
                }

                if (currentInputProfile.ContainsKey(ModifierBinding))
                {
                    var button = devices[ModifierBinding].Button;
                    ModifierText.Text = button < 128 ? (button + 1).ToString() : "POV " + (button - 127); //output POV info
                    ModifierDevice.Text = devices[ModifierBinding].DeviceName;
                }
                else
                {
                    ModifierText.Text = "None";
                    ModifierDevice.Text = "None";

                }
            }
           
        }

        private void Device_Click(object sender, RoutedEventArgs e)
        {
            DeviceClear.IsEnabled = false;
            DeviceButton.IsEnabled = false;


            InputDeviceManager.AssignButton(device =>
            {
                DeviceClear.IsEnabled = true;
                DeviceButton.IsEnabled = true;

                Device.Text = device.DeviceName;
                DeviceText.Text = device.Button < 128 ? (device.Button+1).ToString() : "POV " + (device.Button - 127);
                //output POV info;

                device.InputBind = ControlInputBinding;

                SettingsStore.Instance.InputSettingsStore.SetControlSetting(device);
            });
        }


        private void DeviceClear_Click(object sender, RoutedEventArgs e)
        {
            SettingsStore.Instance.InputSettingsStore.RemoveControlSetting(ControlInputBinding);

            Device.Text = "None";
            DeviceText.Text = "None";
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            ModifierButtonClear.IsEnabled = false;
            ModifierButton.IsEnabled = false;

            InputDeviceManager.AssignButton(device =>
            {
                ModifierButtonClear.IsEnabled = true;
                ModifierButton.IsEnabled = true;

                ModifierDevice.Text = device.DeviceName;
                ModifierText.Text = device.Button < 128 ? (device.Button + 1).ToString() : "POV " + (device.Button - 127);
                //output POV info;

                device.InputBind = ModifierBinding;

                SettingsStore.Instance.InputSettingsStore.SetControlSetting(device);
            });
        }


        private void ModifierClear_Click(object sender, RoutedEventArgs e)
        {
            SettingsStore.Instance.InputSettingsStore.RemoveControlSetting(ModifierBinding);
            ModifierDevice.Text = "None";
            ModifierText.Text = "None";
        }
    }
}