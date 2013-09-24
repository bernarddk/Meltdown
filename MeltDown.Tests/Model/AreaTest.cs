using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Core.Model;
using NUnit.Framework;

namespace MeltDown.Tests.Model
{
    [TestFixture]
    class AreaTest
    {
        [Test]
        public void TrueIsTrue()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void AreasPointToOneOtherAreaPerDirection()
        {
            var a = new Area("Test Area", "Just an empty brick room.");
            var failRoom = new Area("Fail Room", "You shouldn't see this.");
            var expectedRoom = new Area("Success!", "Blue, cloudless sky. W00t!");

            a.Exits[Direction.North] = failRoom;
            a.Exits[Direction.North] = expectedRoom;

            Assert.AreEqual(expectedRoom, a.Exits[Direction.North]);
        }

        [Test]
        public void ExecuteOnEnterExecutesActionIfNotNull()
        {
            var pass = false;

            var area = new Area("Test Area", "Grey floors and beige rooms. Eew.");
            Assert.DoesNotThrow(() => { area.ExecuteOnEnter(); });
            area.OnEnter(() =>
            {
                pass = true;
            });

            area.ExecuteOnEnter();
            Assert.IsTrue(pass);
        }

        [Test]
        public void ExecuteOnExitExecutesActionIfNotNull()
        {
            var pass = false;

            var area = new Area("Test Area", "Grey floors and beige rooms. Eew.");
            Assert.DoesNotThrow(() => { area.ExecuteOnExit(); });
            area.OnExit(() =>
            {
                pass = true;
            });

            area.ExecuteOnExit();
            Assert.IsTrue(pass);
        }
    }
}
