using Cardinal.Lessions;
using Cardinal.Lessions.Contract;

namespace Cardinal.Tests.Fixtures
{
    /// <summary>
    /// Логическое и. И. &. AND
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AndLession : Lession
    {
        public AndLession()
        {
            Dataset = new IDataset[]
            {
                new Dataset(new double[] {0, 0}, new double[] {0}),
                new Dataset(new double[] {0, 1}, new double[] {0}),
                new Dataset(new double[] {1, 0}, new double[] {0}),
                new Dataset(new double[] {1, 1}, new double[] {1})
            };
        }
    }
}