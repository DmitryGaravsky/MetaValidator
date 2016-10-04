namespace MetaValidator.Tests {
    using NUnit.Framework;
    using MetaValidator.Core;

    public class ConfigurationDependentTest {
        protected IConfiguration cfg = Configuration.Default;
        [SetUp]
        public virtual void SetUp() {
            cfg = Configuration.Default;
        }
        [TearDown]
        public virtual void TearDown() {
            cfg.Reset();
        }
    }
}