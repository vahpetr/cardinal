using Xunit;
using Xunit.Sdk;

namespace Cardinal.Tests.RetryFact
{
    /// <summary>
    /// Факт который пытается пройти несколько раз. По умолчанию 3 попытки
    /// </summary>
    [XunitTestCaseDiscoverer("Cardinal.Tests.RetryFact.RetryFactDiscoverer", "Cardinal.Tests")]
    public class RetryFactAttribute : FactAttribute
    {
        /// <summary>
        /// Если сходимость за 5 попыток не удалось найти считается что сеть не сможет апроксимировать задачу
        /// </summary>
        public int MaxRetries { get; set; }
    }
}