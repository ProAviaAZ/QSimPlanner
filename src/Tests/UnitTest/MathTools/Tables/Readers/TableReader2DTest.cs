﻿using NUnit.Framework;
using QSP.MathTools.Tables;
using QSP.MathTools.Tables.Readers;
using QSP.MathTools.TablesNew;

namespace UnitTest.MathTools.Tables.Readers
{
    [TestFixture]
    public class TableReader2DTest
    {
        private const double delta = 1E-7;

        private string format1 =
              @"
              -40 10 14 
              1500 51.2 47.0 46.7 
              1600 52.8 48.5 48.1
              1800 55.9 51.3 51.0 
              ";

        private string format2 =
              @"-40 10 14 
              1500 51.2 47.0 46.7
              1600 52.8 48.5 48.1
              1800 55.9 51.3 51.0";

        private string format3 =
              @"
              -40 10 14 
              1500 51.2 47.0 46.7
 
              1600 52.8 48.5 48.1
              1800 55.9 51.3 51.0";

        [Test]
        public void ReadTest1()
        {
            AssertTable(format1);
        }

        [Test]
        public void ReadTest2()
        {
            AssertTable(format2);
        }

        [Test]
        public void ReadTest3()
        {
            AssertTable(format3);
        }

        private void AssertTable(string source)
        {
            var table = TableReader2D.Read(source);

            var expected = TableBuilder.Build2D(
                new[] { 1500.0, 1600.0, 1800.0 },
                new[] { -40.0, 10.0, 14.0 },
                new[] {
                    new[] {51.2, 47.0, 46.7},
                    new[] {52.8, 48.5, 48.1},
                    new[] {55.9, 51.3, 51.0}
                });
            
            Assert.IsTrue(ITableExtension.Equals(table, expected, delta));
        }
    }
}
