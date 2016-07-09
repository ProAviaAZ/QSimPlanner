﻿using QSP.AircraftProfiles.Configs;
using QSP.AviationTools;
using QSP.RouteFinding.Airports;
using QSP.TOPerfCalculation;
using QSP.UI.ControlStates;
using QSP.UI.ToLdgModule.Common;
using QSP.UI.ToLdgModule.TOPerf.Controllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.ToLdgModule.TOPerf
{
    public partial class TOPerfControl : UserControl
    {
        private const string fileName = "TakeoffPerfControl.xml";

        private FormController controller;
        private TOPerfElements elements;
        private AcConfigManager aircrafts;
        private List<PerfTable> tables;

        private PerfTable currentTable;
        private AutoWeatherSetter wxSetter;

        public AirportManager Airports
        {
            get { return airportInfoControl.Airports; }
            set { airportInfoControl.Airports = value; }
        }

        public TOPerfControl()
        {
            InitializeComponent();

            // Create the reference to the UI controls.
            InitializeElements();

            // Set default values for the controls.
            InitializeControls();

            setWeatherBtnHandlers();
        }

        public void Initialize(
            AcConfigManager aircrafts,
            List<PerfTable> tables,
            AirportManager airports)
        {
            this.aircrafts = aircrafts;
            this.tables = tables;
            UpdateAircraftList();
            this.Airports = airports;
        }

        private void UpdateAircraftList()
        {
            var items = acListComboBox.Items;

            items.Clear();
            items.AddRange(AvailAircraftTypes());

            if (items.Count > 0)
            {
                acListComboBox.SelectedIndex = 0;
            }
        }

        private string[] AvailAircraftTypes()
        {
            var avail = new List<string>();

            foreach (var i in tables)
            {
                if (aircrafts
                    .Aircrafts
                    .Where(c => c.Config.TOProfile == i.Entry.ProfileName)
                    .Count()
                    > 0)
                {
                    avail.Add(i.Entry.Aircraft);
                }
            }

            return avail.ToArray();
        }

        private bool TakeoffProfileExists(string profileName)
        {
            return tables.Any(c => c.Entry.ProfileName == profileName);
        }

        private void RefreshRegistrations(object sender, EventArgs e)
        {
            if (acListComboBox.SelectedIndex >= 0)
            {
                var ac =
                    aircrafts
                    .FindAircraft(acListComboBox.Text);

                var items = regComboBox.Items;

                items.Clear();

                items.AddRange(
                    ac
                    .Where(c => TakeoffProfileExists(c.Config.TOProfile))
                    .Select(c => c.Config.Registration)
                    .ToArray());

                if (items.Count > 0)
                {
                    regComboBox.SelectedIndex = 0;
                }
            }
        }

        public void TryLoadState()
        {
            var doc = StateManager.Load(fileName);
            if (doc != null)
            {
                new ControlState(this).Load(doc);
            }
        }

        private void TrySaveState()
        {
            StateManager.Save(fileName, new ControlState(this).Save());
        }

        private void SaveState(object sender, EventArgs e)
        {
            TrySaveState();
        }

        private void setWeatherBtnHandlers()
        {
            wxSetter = new AutoWeatherSetter(
                weatherInfoControl, airportInfoControl);

            wxSetter.Subscribe();
        }

        private void InitializeControls()
        {
            wtUnitComboBox.SelectedIndex = 0; // KG  
            thrustRatingLbl.Visible = false;
            thrustRatingComboBox.Visible = false;
        }

        private void InitializeElements()
        {
            var ac = airportInfoControl;
            var wic = weatherInfoControl;

            elements = new TOPerfElements(
                ac.airportNameLbl,
                thrustRatingLbl,
                ac.airportTxtBox,
                ac.lengthTxtBox,
                ac.elevationTxtBox,
                ac.rwyHeadingTxtBox,
                wic.windDirTxtBox,
                wic.windSpdTxtBox,
                wic.oatTxtBox,
                wic.pressTxtBox,
                weightTxtBox,
                ac.lengthUnitComboBox,
                ac.slopeComboBox,
                wic.tempUnitComboBox,
                wic.surfCondComboBox,
                wic.pressUnitComboBox,
                wtUnitComboBox,
                flapsComboBox,
                thrustRatingComboBox,
                antiIceComboBox,
                packsComboBox,
                resultsRichTxtBox);
        }

        private void RegistrationChanged(object sender, EventArgs e)
        {
            if (regComboBox.SelectedIndex < 0)
            {
                RefreshWtColor();
                return;
            }

            // unsubsribe all event handlers
            if (controller != null)
            {
                UnSubscribe(controller);
                currentTable = null;
                controller = null;
            }

            // set currentTable and controller
            if (tables != null && tables.Count > 0)
            {
                var profileName =
                    aircrafts
                    .Find(regComboBox.Text)
                    .Config
                    .TOProfile;

                currentTable =
                    tables.First(t => t.Entry.ProfileName == profileName);

                controller = FormControllerFactory.GetController(
                    ControllerType.Boeing,
                    currentTable,
                    elements);
                // TODO: only correct for Boeing. 

                Subscribe(controller);
                controller.Initialize();
                RefreshWtColor();
            }
        }

        private void Subscribe(FormController controller)
        {
            wtUnitComboBox.SelectedIndexChanged += controller.WeightUnitChanged;
            flapsComboBox.SelectedIndexChanged += controller.FlapsChanged;
            calculateBtn.Click += controller.Compute;

            controller.CalculationCompleted += SaveState;
        }

        private void UnSubscribe(FormController controller)
        {
            wtUnitComboBox.SelectedIndexChanged -= controller.WeightUnitChanged;
            flapsComboBox.SelectedIndexChanged -= controller.FlapsChanged;
            calculateBtn.Click -= controller.Compute;

            controller.CalculationCompleted -= SaveState;
        }

        private void RefreshWtColor()
        {
            var ac = aircrafts?.Find(regComboBox.Text);
            var config = ac?.Config;
            double wtKg;

            if (config != null && double.TryParse(weightTxtBox.Text, out wtKg))
            {
                if (wtUnitComboBox.SelectedIndex == 1)
                {
                    wtKg *= Constants.LbKgRatio;
                }

                if (wtKg > config.MaxTOWtKg || wtKg < config.OewKg)
                {
                    weightTxtBox.ForeColor = Color.Red;
                }
                else
                {
                    weightTxtBox.ForeColor = Color.Green;
                }
            }
            else
            {
                weightTxtBox.ForeColor = Color.Black;
            }
        }

        private void WeightTxtBoxChanged(object sender, EventArgs e)
        {
            RefreshWtColor();
        }

        /// <summary>
        /// Refresh the aircraft and registration comboBoxes,
        /// after the AcConfigManager is updated.
        /// </summary>
        public void RefreshAircrafts(object sender, EventArgs e)
        {
            // Set the selected aircraft/registration.
            string ac = acListComboBox.Text;
            string reg = regComboBox.Text;

            UpdateAircraftList();
            acListComboBox.Text = ac;
            regComboBox.Text = reg;

            // Set the color of weight.
            RefreshWtColor();
        }
    }
}
