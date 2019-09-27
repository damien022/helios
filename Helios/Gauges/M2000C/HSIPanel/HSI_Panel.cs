﻿//  Copyright 2014 Craig Courtney
//    
//  Helios is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Helios is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace GadrocsWorkshop.Helios.Gauges.M2000C
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("HELIOS.M2000C.HSI_PANEL", "HSI Panel", "M2000C Gauges", typeof(M2000CDeviceRenderer))]
    class M2000C_HSIPanel : M2000CDevice
    {
        private static readonly Rect SCREEN_RECT = new Rect(0, 0, 300, 287);
        private string _interfaceDeviceName = "HSI Panel";
        private Rect _scaledScreenRect = SCREEN_RECT;

        public M2000C_HSIPanel()
            : base("HSI Panel", new Size(300, 287))
        {
            int row0 = 84;
            int column0 = 95, column1 = 128, column2 = 158, column3 = 188;

            AddDrum("Distance (Decimals)", "{Helios}/Gauges/M2000C/Common/drum_tape.xaml", "decimal distance", "(0 - 9)", "#",
                new Point(column3, row0), new Size(10d, 15d), new Size(20d, 30d));
            AddDrum("Distance (Ones)", "{Helios}/Gauges/M2000C/Common/drum_tape.xaml", "ones distance", "(0 - 9)", "#",
                new Point(column2, row0), new Size(10d, 15d), new Size(20d, 30d));
            AddDrum("Distance (Tens)", "{Helios}/Gauges/M2000C/Common/drum_tape.xaml", "tens distance", "(0 - 9)", "#",
                new Point(column1, row0), new Size(10d, 15d), new Size(20d, 30d));
            AddDrum("Distance (Hundreds)", "{Helios}/Gauges/M2000C/Common/drum_tape.xaml", "hundreds distance", "(0 - 9)", "#",
                new Point(column0, row0), new Size(10d, 15d), new Size(20d, 30d));

            AddNeedle("Compass Rose", "{Helios}/Gauges/M2000C/HSIPanel/compass.xaml", "compass", "(0 - 360)", 
                new Point(186d, 169d), new Size(230d, 230d), new Point(150d, 142d), BindingValueUnits.Numeric, new double[] { 0d, 0d, 360d, 360d });

            AddIndicator("Flag 1", "distance-flag", new Point(90, 92), new Size(120, 15));
            AddIndicator("Flag 2", "left-flag", new Point(100, 143), new Size(30, 15));
            AddIndicator("Flag CAP", "right-flag", new Point(145, 143), new Size(30, 15));

            AddNeedle("Direction Needle", "{M2000C}/Images/HSIPanel/direction-needle.png", "direction needle", "(0 - 360)",
                new Point(152, 20), new Size(40d, 20d), new Point(20d, 10d), BindingValueUnits.Numeric, new double[] { 0d, 0d, 360d, 360d });
            AddNeedle("Mode Needle", "{M2000C}/Images/HSIPanel/mode-selector-needle.png", "mode needle", "(0 - 6)",
                new Point(90, 250), new Size(15d, 15d), new Point(60d, 100d), BindingValueUnits.Numeric, new double[] { 0d, 260d, 6d, 100d });
            AddNeedle("Big Needle", "{M2000C}/Images/HSIPanel/big-needle.png", "big needle", "(0 - 360)",
                new Point(152, 132), new Size(20d, 184d), new Point(10d, 92d), BindingValueUnits.Numeric, new double[] { 0d, 0d, 360d, 360d });
            AddNeedle("Small Needle", "{M2000C}/Images/HSIPanel/small-needle.png", "small needle", "(0 - 360)",
                new Point(152, 145), new Size(8d, 202d), new Point(4d, 101d), BindingValueUnits.Numeric, new double[] { 0d, 0d, 360d, 360d });

            AddPot("VAD Selector", new Point(30,260), new Size(45, 45), "vad-selector", 0d, 0d, 0d, 1000d, 0d, 0.1d, true);
            AddRotarySwitch("Mode Selector", new Point(270, 260), new Size(45, 45), "mode-selector");
        }

        #region Properties

        public override string BezelImage
        {
            get { return "{M2000C}/Images/HSIPanel/hsi-panel.png"; }
        }

        #endregion

        private void AddPot(string name, Point posn, Size size, string imagePrefix, double initialRotation, double rotationTravel, double minValue, double maxValue,
            double initialValue, double stepValue, bool fromCenter)
        {
            AddPot(
                name: name,
                posn: posn,
                size: size,
                knobImage: "{M2000C}/Images/HSIPanel/" + imagePrefix + ".png",
                initialRotation: initialRotation,
                rotationTravel: rotationTravel,
                minValue: minValue,
                maxValue: maxValue,
                initialValue: initialValue,
                stepValue: stepValue,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                fromCenter: true,
                clickType: ClickType.Touch);
        }

        private void AddRotarySwitch(string name, Point posn, Size size, string imagePrefix)
        {
            RotarySwitch rSwitch = AddRotarySwitch(name: name,
                posn: posn,
                size: size,
                knobImage: "{M2000C}/Images/HSIPanel/" + imagePrefix + ".png",
                defaultPosition: 0,
                clickType: ClickType.Touch,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                fromCenter: true);
            rSwitch.Positions.Clear();
            switch (name)
            {
                case "X/Y Selector":
                    rSwitch.Positions.Add(new RotarySwitchPosition(rSwitch, 1, "X", 0d));
                    rSwitch.Positions.Add(new RotarySwitchPosition(rSwitch, 2, "Y", 45d));
                    break;
                case "Mode Selector":
                    rSwitch.Positions.Add(new RotarySwitchPosition(rSwitch, 1, "OFF", 215d));
                    rSwitch.Positions.Add(new RotarySwitchPosition(rSwitch, 2, "REC", 240d));
                    rSwitch.Positions.Add(new RotarySwitchPosition(rSwitch, 3, "T/R", 270d));
                    rSwitch.Positions.Add(new RotarySwitchPosition(rSwitch, 4, "A/A", 300d));
                    break;
            }
            foreach (RotarySwitchPosition position in rSwitch.Positions)
            {
                AddTrigger(rSwitch.Triggers["position " + position.Index + ".entered"], rSwitch.Name);
            }
            rSwitch.DefaultPosition = 1;
        }

        private void AddDrum(string name, string gaugeImage, string actionIdentifier, string valueDescription, string format, Point posn, Size size, Size renderSize)
        {
            AddDrumGauge(name: name,
                gaugeImage: gaugeImage,
                posn: posn,
                size: size,
                renderSize: renderSize,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                actionIdentifier: actionIdentifier,
                valueDescription: valueDescription,
                format: format,
                fromCenter: false);
        }

        private void AddNeedle(string name, string needleImage, string actionIdentifier, string valueDescription, 
            Point posn, Size size, Point centerPoint, BindingValueUnit typeValue, double[] initialCalibration)
        {
            AddNeedle(name: name,
                needleImage: needleImage,
                posn: posn,
                size: size,
                centerPoint: centerPoint,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                actionIdentifier: actionIdentifier,
                valueDescription: valueDescription,
                typeValue: typeValue,
                initialCalibration: initialCalibration,
                fromCenter: false);
        }

        private void AddIndicator(string name, string imagePrefix, Point posn, Size size)
        {
            AddIndicator(
                name: name,
                posn: posn,
                size: size,
                onImage: "{M2000C}/Images/HSIPanel/" + imagePrefix + ".png",
                offImage: "{M2000C}/Images/Miscellaneous/void.png",
                onTextColor: Color.FromArgb(0xff, 0x7e, 0xde, 0x72), //don’t need it because not using text
                offTextColor: Color.FromArgb(0xff, 0x7e, 0xde, 0x72), //don’t need it because not using text
                font: "", //don’t need it because not using text
                vertical: false, //don’t need it because not using text
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                fromCenter: false,
                withText: false); //added in Composite Visual as an optional value with a default value set to true
        }

        protected override void OnPropertyChanged(PropertyNotificationEventArgs args)
        {
            if (args.PropertyName.Equals("Width") || args.PropertyName.Equals("Height"))
            {
                double scaleX = Width / NativeSize.Width;
                double scaleY = Height / NativeSize.Height;
                _scaledScreenRect.Scale(scaleX, scaleY);
            }
            base.OnPropertyChanged(args);
        }

        public override bool HitTest(Point location)
        {
            if (_scaledScreenRect.Contains(location))
            {
                return false;
            }

            return true;
        }

        public override void MouseDown(Point location)
        {
            // No-Op
        }

        public override void MouseDrag(Point location)
        {
            // No-Op
        }

        public override void MouseUp(Point location)
        {
            // No-Op
        }
    }
}
