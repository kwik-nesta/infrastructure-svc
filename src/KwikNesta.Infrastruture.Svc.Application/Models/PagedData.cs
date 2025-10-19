namespace KwikNesta.Infrastruture.Svc.Application.Models
{
    public class PagedData<T>
    {
        public MetaData? MetaData { get; set; }
        public List<T> Data { get; set; } = [];
    }

    public class MetaData
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int Pages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public int Count { get; set; }
    }
}
