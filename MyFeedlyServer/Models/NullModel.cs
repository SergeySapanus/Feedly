namespace MyFeedlyServer.Models
{
    public class NullModel<T>
    {
        protected T Source { get; }

        public NullModel(T source)
        {
            Source = source;
        }

        public bool IsNull() => ReferenceEquals(Source, null);
    }
}
