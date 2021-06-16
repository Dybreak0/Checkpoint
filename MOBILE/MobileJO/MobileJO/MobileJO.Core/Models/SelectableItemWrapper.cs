namespace MobileJO.Core.Models
{
    public class SelectableItemWrapper<T>
    {
        public bool IsSelected { get; set; }
        public T Item { get; set; }
    }
}
