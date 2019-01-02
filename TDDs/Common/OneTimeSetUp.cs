using NUnit.Framework;

namespace TDD
{
    [SetUpFixture]
    public class OneTimeSetUp
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            OneTimeSetUpImpl.SetUp();
        }
    }
}