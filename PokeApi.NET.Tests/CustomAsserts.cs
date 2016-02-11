using Xunit;

namespace PokeAPI.Tests
{
    public static class CustomAsserts
    {
        public static void AssertResourceWellConfigured<T>(this ApiObject<T> apiObject)
            where T : ApiObject<T>
        {
            Assert.NotNull(apiObject);
            Assert.False(apiObject.AnyStringPropertyEmpty());
        }
    }
}
