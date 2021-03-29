using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlexLights.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace PlexLights.Tests
{
    public class TimerCollectionTests
    {
        private readonly ITestOutputHelper _output;

        public TimerCollectionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task CallsAction_AfterDurationPeriodExpires()
        {
            var collection = new TimerCollection();
            var output = new List<string>();

            Action action = () => output.Add("Job executed successfully.");

            collection.AddOrUpdate("job", action, TimeSpan.FromSeconds(1));

            await Task.Delay(TimeSpan.FromSeconds(1.1));
            Assert.Single(output);
        }

        [Fact]
        public async Task DoesntCallFirstActionWhenUpdatingExistingKey()
        {
            var collection = new TimerCollection();
            var output = new List<string>();

            Action action1 = () => output.Add("First job.");
            collection.AddOrUpdate("job", action1, TimeSpan.FromSeconds(1));

            Action action2 = () => output.Add("Second job.");
            collection.AddOrUpdate("job", action2, TimeSpan.FromSeconds(1));

            await Task.Delay(TimeSpan.FromSeconds(1.1));

            foreach (var item in output)
            {
                _output.WriteLine(item);
            }

            Assert.Single(output);
            Assert.Equal(0, collection.Count);
            Assert.Equal("Second job.", output[0]);
        }

        [Fact]
        public async Task NothingHappensWhenCollectionClearedBeforeJobFires()
        {
            var collection = new TimerCollection();
            var output = new List<string>();

            Action action1 = () => output.Add("First job.");
            collection.AddOrUpdate("job", action1, TimeSpan.FromSeconds(0.5));

            collection.Clear();

            await Task.Delay(TimeSpan.FromSeconds(1.1));
            Assert.Empty(output);
        }
    }
}