using System.Reflection;

public static class Extensions
{
    public static void DoubleBuffered(this Control control, bool enabled)
    {
        var prop = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
        prop.SetValue(control, enabled, null);
    }

    public static IEnumerable<LinkedListNode<T>> EnumerateNodes<T>(this LinkedList<T> list)
    {
        var node = list.First;
        while (node != null)
        {
            yield return node;
            node = node.Next;
        }
    }
}