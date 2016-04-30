﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.NavData.OpenData;
using QSP.RouteFinding.Airports;
using System.Linq;

namespace UnitTest.NavData.OpenData
{
    [TestClass]
    public class AirportDataLoaderTest
    {
        private const double delta = 0.005;

        [TestMethod]
        public void LoadFromFileTest()
        {
            var loader = new AirportDataLoader(@"NavData\OpenData");
            var airports = loader.LoadFromFile();

            // Airport parameter incomplete - should not load.
            Assert.IsNull(airports.Find("37NJ"));

            var vhhh = airports.Find("VHHH");
            var rwys = vhhh.Rwys;

            assertVhhh(vhhh);
            assertVhhhRwys(vhhh);
        }

        private static void assertVhhh(Airport vhhh)
        {
            Assert.IsTrue("VHHH" == vhhh.Icao);
            Assert.IsTrue("Chek Lap Kok International Airport" == vhhh.Name);
            Assert.AreEqual(22.3089008331, vhhh.Lat, delta);
            Assert.AreEqual(113.915000916, vhhh.Lon, delta);
            Assert.AreEqual(28, vhhh.Elevation);
            Assert.AreEqual(false, vhhh.TransAvail);
            Assert.AreEqual(3800, vhhh.LongestRwyLength);
        }
        private static void assertVhhhRwys(Airport vhhh)
        {
            var rwys = vhhh.Rwys;

            var _7L = rwys.Where(r => r.RwyIdent == "07L").First();
            Assert.IsTrue(_7L.Heading == "");//TODO:
            Assert.AreEqual(3800, _7L.Length);
            Assert.AreEqual(60, _7L.Width);
            Assert.AreEqual(false, _7L.HasIlsInfo);
            Assert.AreEqual(22.3104, vhhh.Lat, delta);
            Assert.AreEqual(113.896, vhhh.Lon, delta);
            Assert.AreEqual(22, _7L.Elevation);
            Assert.IsTrue("ASP" == _7L.SurfaceType);

            var _7R = rwys.Where(r => r.RwyIdent == "07R").First();
            Assert.IsTrue(_7R.Heading == "");//TODO:
            Assert.AreEqual(3800, _7R.Length);
            Assert.AreEqual(60, _7R.Width);
            Assert.AreEqual(false, _7R.HasIlsInfo);
            Assert.AreEqual(22.2962, vhhh.Lat, delta);
            Assert.AreEqual(113.898, vhhh.Lon, delta);
            Assert.AreEqual(28, _7R.Elevation);
            Assert.IsTrue("ASP" == _7R.SurfaceType);

            var _25R = rwys.Where(r => r.RwyIdent == "25R").First();
            Assert.IsTrue(_25R.Heading == "");//TODO:
            Assert.AreEqual(3800, _25R.Length);
            Assert.AreEqual(60, _25R.Width);
            Assert.AreEqual(false, _25R.HasIlsInfo);
            Assert.AreEqual(22.3216, vhhh.Lat, delta);
            Assert.AreEqual(113.931, vhhh.Lon, delta);
            Assert.AreEqual(23, _25R.Elevation);
            Assert.IsTrue("ASP" == _25R.SurfaceType);

            var _25L = rwys.Where(r => r.RwyIdent == "25L").First();
            Assert.IsTrue(_25L.Heading == "");//TODO:
            Assert.AreEqual(3800, _25L.Length);
            Assert.AreEqual(60, _25L.Width);
            Assert.AreEqual(false, _25L.HasIlsInfo);
            Assert.AreEqual(22.3074, vhhh.Lat, delta);
            Assert.AreEqual(113.933, vhhh.Lon, delta);
            Assert.AreEqual(27, _25L.Elevation);
            Assert.IsTrue("ASP" == _25L.SurfaceType);
        }

    }
}