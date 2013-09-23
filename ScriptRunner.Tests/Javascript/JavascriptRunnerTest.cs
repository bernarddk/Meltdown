using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Core;
using Meltdown.Core.Model;
using NUnit.Framework;
using ScriptRunner.Core;

namespace ScriptRunner.Tests.Javascript
{
    [TestFixture]
    class JavascriptRunnerTest
    {
        [Test]
        public void TrueIsTrue()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void RunnerCanCreateInteractiveObject()
        {
            var o = Runner.Instance.Execute<InteractiveObject>(@"Scripts\Javascript\Samosa.js");

            Assert.IsNotNull(o);

            Assert.AreEqual("Samosa", o.Name);
            Assert.AreEqual("A crisp, triangular samosa.", o.Description);
            Assert.AreEqual(1, o.Affordances.Count());
            Assert.AreEqual("eat", o.Affordances.First().ToLower());            
        }

        [Test]
        public void RunnerCanCreateAndInvokeCommand()
        {
            var command = Runner.Instance.Execute<Command>(@"Scripts\Javascript\Eat.js");

            Assert.IsNotNull(command);

            Assert.AreEqual("Eat", command.Name);
            Assert.AreEqual(1, command.Verbs.Count());
            Assert.AreEqual("eat", command.Verbs.First().ToLower());
            Assert.AreEqual("Eat what?", command.Invoke());

            var samosa = Runner.Instance.Execute<InteractiveObject>(@"Scripts\Javascript\Samosa.js");
            Assert.IsTrue(samosa.Affordances.Any(a => a.ToLower() == "eat"), "Need edible item for this test.");
            Assert.AreEqual("you eat the samosa.", command.Invoke(samosa).ToLower());
        }

        [Test]
        public void AfterCommandActionsInvoke()
        {
            var potato = Runner.Instance.Execute<InteractiveObject>(@"Scripts\Javascript\AfterCommand.js");
            Assert.IsFalse(potato.Description.StartsWith("A steaming")); // changes on invoke
            Assert.IsTrue(potato.ListensFor("get"));
            potato.ProcessCommand("Get");
            Assert.IsTrue(potato.Description.StartsWith("A steaming"));
        }
    }
}
