using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

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
        public void RunnerCanCreateCommand()
        {
            var command = ScriptRunner.Core.ScriptRunner.Instance
                .Execute<Meltdown.Core.Command>(@"Scripts\Javascript\Eat.js");

            Assert.IsNotNull(command);

            Assert.AreEqual("Eat", command.Name);
            Assert.AreEqual(1, command.Verbs.Count());
            Assert.AreEqual("eat", command.Verbs.First().ToLower());
            Assert.AreEqual("Eat what?", command.Invoke());
        }
    }
}
