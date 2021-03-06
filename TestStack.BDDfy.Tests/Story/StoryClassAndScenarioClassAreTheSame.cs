using NUnit.Framework;
using System.Linq;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute.GwtAttributes;

namespace TestStack.BDDfy.Tests.Story
{
    [TestFixture]
    public class StoryClassAndScenarioClassAreTheSame
    {
        private Core.Story _story;

        [Story(
            AsA = "As a story with no scenarios specified using attributes",
            IWant = "I want to be considered as a scenario",
            SoThat = "So that single scenario stories do not have to separate into two classes")]
        public class StoryAsScenario
        {
            [Then(StepTitle = "See?! I am a normal scenario even though I have been decorated with StoryAttribute")]
            void SeeIAmAScenario()
            {
            }
        }

        void WhenTheStoryIsBddified()
        {
            _story = new StoryAsScenario().BDDfy();
        }

        void ThenStoryIsReturnedAsAStory()
        {
            Assert.That(_story.MetaData.Type, Is.EqualTo(typeof(StoryAsScenario)));
        }

        [AndThen(StepTitle = "and as a scenario")]
        void andAsAScenario()
        {
            Assert.That(_story.Scenarios.Count(), Is.EqualTo(1));
            var scenario = _story.Scenarios.First();
            Assert.That(scenario.TestObject.GetType(), Is.EqualTo(typeof(StoryAsScenario)));
        }

        void andTheNarrativeIsReturnedAsExpected()
        {
            var expectedNarrative = (StoryAttribute)typeof(StoryAsScenario).GetCustomAttributes(typeof(StoryAttribute), false).First();
            Assert.That(_story.MetaData, Is.Not.Null);
            Assert.That(_story.MetaData.AsA, Is.EqualTo(expectedNarrative.AsA));
            Assert.That(_story.MetaData.IWant, Is.EqualTo(expectedNarrative.IWant));
            Assert.That(_story.MetaData.SoThat, Is.EqualTo(expectedNarrative.SoThat));
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }
    }
}