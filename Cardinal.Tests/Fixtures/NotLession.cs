using Cardinal.Lessions;
using Cardinal.Lessions.Contract;

namespace Cardinal.Tests.Fixtures
{
    /// <summary>
    /// Логическое инвертирование
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class NotLession : Lession
    {
        public NotLession()
        {
            Dataset = new IDataset[]
            {
                new Dataset(new double[] {1}, new double[] {0}),
                new Dataset(new double[] {0}, new double[] {1})
            };
        }
    }
}