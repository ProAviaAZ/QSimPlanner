﻿using System.Xml.Linq;

namespace QSP.UI.ToLdgModule.LandingPerf
{
    public class ControlState
    {
        // Strings for xml tags.
        private string airportIcao = "AirportIcao";
        private string rwy = "Runway";
        private string lengthUnit = "LengthUnit";
        private string windDir = "WindDir";
        private string windSpeed = "WindSpeed";
        private string tempUnit = "TempUnit";
        private string oat = "OAT";
        private string pressUnit = "PressUnit";
        private string pressure = "Pressure";
        private string surfCond = "SurfCond";
        private string aircraft = "Aircraft";
        private string wtUnit = "WeightUnit";
        private string ldgWt = "LandingWeight";
        private string flaps = "Flaps";
        private string rev = "Reverser";
        private string brakes = "Brakes";
        private string appSpeedInc = "AppSpeedInc";

        private LandingPerfControl control;

        public ControlState(LandingPerfControl control)
        {
            this.control = control;
        }

        public XElement Save()
        {
            var airport = control.airportInfoControl;
            var weather = control.weatherInfoControl;

            return new XElement("LandingPerfState", new XElement[]
            {
                new XElement(airportIcao,airport.airportTxtBox.Text),
                new XElement(rwy,airport.rwyComboBox.Text),
                new XElement(lengthUnit,airport.lengthUnitComboBox.Text),
                new XElement(windDir,weather.windDirTxtBox.Text),
                new XElement(windSpeed,weather.windSpdTxtBox.Text),
                new XElement(tempUnit,weather.tempUnitComboBox.Text),
                new XElement(oat,weather.oatTxtBox.Text),
                new XElement(pressUnit,weather.pressUnitComboBox.Text),
                new XElement(pressure,weather.pressTxtBox.Text),
                new XElement(surfCond,control.weatherInfoControl.surfCondComboBox.Text),
                new XElement(aircraft,control.acListComboBox.Text),
                new XElement(wtUnit,control.wtUnitComboBox.Text),
                new XElement(ldgWt,control.weightTxtBox.Text),
                new XElement(flaps,control.flapsComboBox.Text),
                new XElement(rev,control.revThrustComboBox.Text),
                new XElement(brakes,control.brakeComboBox.Text),
                new XElement(appSpeedInc,control.appSpdIncTxtBox.Text)
            });
        }

        public void Load(XDocument doc)
        {
            var root = doc.Root;
            var airport = control.airportInfoControl;
            var weather = control.weatherInfoControl;

            // The order is important. E.g. "pressUnit" has to be 
            // loaded before "pressure", due to events handlers attached 
            // to pressure.TextChanged.
            airport.airportTxtBox.Text = root.Element(airportIcao).Value;
            airport.rwyComboBox.Text = root.Element(rwy).Value;
            airport.lengthUnitComboBox.Text = root.Element(lengthUnit).Value;
            weather.windDirTxtBox.Text = root.Element(windDir).Value;
            weather.windSpdTxtBox.Text = root.Element(windSpeed).Value;
            weather.tempUnitComboBox.Text = root.Element(tempUnit).Value;
            weather.oatTxtBox.Text = root.Element(oat).Value;
            weather.pressUnitComboBox.Text = root.Element(pressUnit).Value;
            weather.pressTxtBox.Text = root.Element(pressure).Value;
            control.weatherInfoControl.surfCondComboBox.Text = root.Element(surfCond).Value;
            control.acListComboBox.Text = root.Element(aircraft).Value;
            control.wtUnitComboBox.Text = root.Element(wtUnit).Value;
            control.weightTxtBox.Text = root.Element(ldgWt).Value;
            control.flapsComboBox.Text = root.Element(flaps).Value;
            control.revThrustComboBox.Text = root.Element(rev).Value;
            control.brakeComboBox.Text = root.Element(brakes).Value;
            control.appSpdIncTxtBox.Text = root.Element(appSpeedInc).Value;
        }
    }
}