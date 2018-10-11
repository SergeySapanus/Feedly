using Newtonsoft.Json;

namespace MyFeedlyServer.Tests.Extensions
{
    public static class ObjectExtensions
    {
        public static dynamic ConvertToDynamicByJsonConvert(this object obj)
        {
            var serializedObject = JsonConvert.SerializeObject(obj);
            dynamic deserializedObject = JsonConvert.DeserializeObject(serializedObject);

            return deserializedObject;
        }
    }
}