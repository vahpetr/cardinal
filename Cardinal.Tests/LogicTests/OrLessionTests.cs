using Cardinal.Builders;
using Cardinal.Learning;
using Cardinal.Lessions.Contract;
using Cardinal.Tests.Fixtures;
using Cardinal.Tests.Utilites;
using Cardinal.Trainings;
using Xunit;
using Xunit.Abstractions;

namespace Cardinal.Tests.LogicTests
{
    public class AndLessionTests : IClassFixture<AndLession>
    {
        private readonly ILession lession;
        private readonly NetworkOutput networkOutput;

        public AndLessionTests(AndLession lession, ITestOutputHelper output)
        {
            this.lession = lession;
            networkOutput = new NetworkOutput(output);
        }

        [Fact]
        public void OfflineTest()
        {
            var networkBuilder = new NetworkBuilder
            {
                Datasets = lession.Train,
                HiddenTopology = new[] { 2 }
            };
            var network = networkBuilder.Build();
            networkOutput.PrintNetworkWeights(network);
            var learning = new SupervisedLearning(network)
            {
                LearningRate = 0.125,
                LearningRateBonus = 0.05,
                Regularization = 0.01
            };
            var training = new Training(learning, lession)
            {
                BatchSize = 2,
                MaxEpoches = 10000
            };
            training.Run();
            networkOutput.PrintDetails(training);
            Assert.True(training.ValidationError.Accuracy > 0.90d);
        }

        [Fact]
        public void OnlineTest()
        {
            var networkBuilder = new NetworkBuilder
            {
                Datasets = lession.Train,
                HiddenTopology = new[] { 2 }
            };
            var network = networkBuilder.Build();
            networkOutput.PrintNetworkWeights(network);
            var learning = new SupervisedLearning(network)
            {
                LearningRate = 0.125,
                LearningRateBonus = 0.05,
                Momentum = 0.9
            };
            var training = new Training(learning, lession)
            {
                MaxEpoches = 10000
            };
            training.Run();
            networkOutput.PrintDetails(training);
            Assert.True(training.ValidationError.Accuracy > 0.90d);
        }
    }
}