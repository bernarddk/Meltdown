using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Core;
using Meltdown.Core.Model;
using NUnit.Framework;
using ScriptRunner.Core;

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
            var actual = Runner.Instance.Execute<InteractiveObject>(@"Scripts\Ruby\Car.rb");
            Assert.AreEqual("Car", actual.Name);
            Assert.AreEqual("A shiny red car!", actual.Description);
            Assert.AreEqual(1, actual.Affordances.Count);
            Assert.AreEqual("Burn".ToLower(), actual.Affordances.First().ToLower());
        }

        [Test]
        public void RunnerCanGetAndExecuteCommand()
        {
            Runner.Instance.BindParameter("current_area", new Area("Empty Area", "An empty room. Full of dust."));
            var car = Runner.Instance.Execute<InteractiveObject>(@"Scripts\Ruby\Car.rb");
            Assert.IsTrue(car.Affordances.Any(a => a.ToUpper() == "burn".ToUpper()), "Test needs a burnable object.");

            var command = Runner.Instance.Execute<Command>(@"Scripts\Ruby\Burn.rb");
            Assert.AreEqual("Burn", command.Name);
            Assert.AreEqual(1, command.Verbs.Count());
            Assert.AreEqual("burn", command.Verbs.First().ToLower());
            Assert.IsTrue(command.Invoke().Equals("Burn what?"));

            Assert.IsTrue(command.Invoke(car).ToLower().Contains("you burn the car"));
        }
    }
}
