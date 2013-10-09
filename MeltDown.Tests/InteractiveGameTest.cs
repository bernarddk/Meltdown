using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meltdown.Core.Model;
using Meltdown.Game;
using NUnit.Framework;
using ScriptRunner.Core;

namespace MeltDown.Tests
{
    [TestFixture]
    class InteractiveGameTest
    {
        [Test]
        public void TrueIsTrue()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void ApiVariablesAreSet()
        {
            TestForLanguage(ScriptRunner.Core.ScriptHelper.ScriptType.Javascript);
            TestForLanguage(ScriptRunner.Core.ScriptHelper.ScriptType.Ruby);
        }

        private static void TestForLanguage(ScriptRunner.Core.ScriptHelper.ScriptType language)
        {
            // Force Game to set up variables on the singleton script runner
            new InteractiveFictionGame();

            Assert.IsNotNull(Runner.Instance.Execute<Player>("player", language));
            Assert.IsNotNull(Runner.Instance.Execute<InteractiveFictionGame>("game", language));
        }
    }
}
