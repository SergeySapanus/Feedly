namespace MyFeedlyServer.Models
{
    public class NullModel<T>
    {
        public NullModel(T source)
        {
            Source = source;
        }

        protected T Source { get; }

        public bool IsNull() => ReferenceEquals(Source, null);
    }
}
