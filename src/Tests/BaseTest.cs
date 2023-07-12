namespace Parking.Control.Tests
{
    public abstract class BaseTest
    {
        protected readonly Fixture _fixture;
        public BaseTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}
