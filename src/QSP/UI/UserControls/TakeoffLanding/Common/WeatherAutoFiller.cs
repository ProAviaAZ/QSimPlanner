﻿using System;
using System.Windows.Forms;
using QSP.MathTools;
using QSP.Metar;
using QSP.Utilities.Units;

namespace QSP.UI.UserControls.TakeoffLanding.Common
{
    public static class WeatherAutoFiller
    {
        /// <summary>
        /// Returns whether the operation was successful.
        /// </summary>
        public static bool Fill(
            string metar,
            TextBox windDirection,
            TextBox windSpeed,
            TextBox oat,
            ComboBox tempUnit,
            TextBox altimeter,
            ComboBox pressUnit,
            ComboBox surfCond)
        {
            var wind = ParaExtractor.GetWind(metar);
            int? temp = ParaExtractor.GetTemp(metar);
            var press = ParaExtractor.GetPressure(metar);
            bool precip = ParaExtractor.PrecipitationExists(metar);

            if (wind == null || temp == null || press == null) return false;

            var w = wind.Value;
            windSpeed.Text = Numbers.RoundToInt(w.Speed).ToString();

            windDirection.Text =
                ((Numbers.RoundToInt(w.Direction) - 1)
                .Mod(360) + 1)
                .ToString()
                .PadLeft(3, '0');

            tempUnit.SelectedIndex = 0;
            oat.Text = temp.Value.ToString();
            pressUnit.SelectedIndex = (int)press.PressUnit;
            altimeter.Text =
                press.PressUnit == PressureUnit.inHg ?
                Math.Round(press.Value, 2).ToString("0.00") :
                Numbers.RoundToInt(press.Value).ToString();

            SetSurfCond(surfCond, precip);

            return true;

        }

        private static void SetSurfCond(ComboBox surfCond, bool precip)
        {
            if (surfCond.Items.Count >= 2)
            {
                surfCond.SelectedIndex = precip ? 1 : 0;
            }
        }
    }
}