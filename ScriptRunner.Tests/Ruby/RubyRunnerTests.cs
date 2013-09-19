using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Core.Model;
using NUnit.Framework;

namespace ScriptRunner.Tests.Ruby
{
    [TestFixture]
    class RubyRunnerTests
    {
        [Test]
        public void TrueIsTrue()
        {
            Assert.That(true, Is.True);
        }

        [Test]
        public void RunnerCanGetInteractiveObjects()
        {
            var actual = ScriptRunner.Core.ScriptRunner.Instance.Execute<InteractiveObject>(@"Scripts/Ruby\Car.rb");
            Assert.AreEqual("Car", actual.Name);
            Assert.AreEqual("A shiny red car!", actual.Description);
            Assert.AreEqual(1, actual.Affordances.Count);
            Assert.AreEqual("Burn".ToLower(), actual.Affordances.First().ToLower());
        }

        [Test]
        public void RunnerCanGetCommand()
        {
            var car = ScriptRunner.Core.ScriptRunner.Instance.Execute<InteractiveObject>(@"Scripts/Ruby\Car.rb");
            Assert.IsTrue(car.Affordances.Any(a => a.ToUpper() == "burn".ToUpper()), "Test needs a burnable object.");
        }
    }
}
