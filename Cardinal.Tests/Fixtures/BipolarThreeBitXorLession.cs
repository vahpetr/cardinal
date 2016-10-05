using Cardinal.Lessions;
using Cardinal.Lessions.Contract;

namespace Cardinal.Tests.Fixtures
{
    /// <summary>
    /// Логическое исключающее или. ИЛИ НЕ. ^. XOR. Трёхбитное. Биполярное
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class BipolarThreeBitXorLession : Lession
    {
        public BipolarThreeBitXorLession()
        {
            Dataset = new IDataset[]
            {
                new Dataset(new double[] {0, 0, 0}, new double[] {0}),
                new Dataset(new double[] {0, 0, 1}, new double[] {1}),
                new Dataset(new double[] {0, 1, 0}, new double[] {1}),
                new Dataset(new double[] {0, 1, 1}, new double[] {1}),
                new Dataset(new double[] {1, 0, 0}, new double[] {1}),
                new Dataset(new double[] {1, 0, 1}, new double[] {1}),
                new Dataset(new double[] {1, 1, 0}, new double[] {1}),
                new Dataset(new double[] {1, 1, 1}, new double[] {0}),

                new Dataset(new double[] {0, 0, -1}, new double[] {1}),
                new Dataset(new double[] {0, -1, 0}, new double[] {1}),
                new Dataset(new double[] {0, -1,-1}, new double[] {1}),
                new Dataset(new double[] {-1, 0, 0}, new double[] {1}),
                new Dataset(new double[] {-1, 0, -1}, new double[] {1}),
                new Dataset(new double[] {-1, -1, 0}, new double[] {1}),
                new Dataset(new double[] {-1, -1, -1}, new double[] {0}),

                new Dataset(new double[] {1, 1, -1}, new double[] {1}),
                new Dataset(new double[] {1, -1, 1}, new double[] {1}),
                new Dataset(new double[] {1, -1,-1}, new double[] {1}),
                new Dataset(new double[] {-1, 1, 1}, new double[] {1}),
                new Dataset(new double[] {-1, 1, -1}, new double[] {1}),
                new Dataset(new double[] {-1, -1, 1}, new double[] {1}),

                new Dataset(new double[] {0, 1,-1}, new double[] {1}),
                new Dataset(new double[] {-1,0, 1}, new double[] {1}),
                new Dataset(new double[] {1,-1, 0}, new double[] {1})
            };
        }
    }
}