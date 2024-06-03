namespace PublicCommon;
public static class ListExtensions
    {

    /// <summary>
    /// make sure item is from the same list not any other response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="updatedItem"></param>
    /// <param name="updateAction"></param>
    public static void UpdateAndMoveToFront<T>(List<T> list, T updatedItem, Action<T> updateAction)
        {
        int index = list.IndexOf(updatedItem); // Find the index of the updated item
        if (index != -1)
            {
            updateAction(updatedItem); // Update the item at the found index

            // Remove the updated item from its original position
            list.RemoveAt(index);

            // Insert the updated item at the front
            list.Insert(0, updatedItem);
            }
        }

    public static void UpdateAndMoveToFront<T>(List<T> list, int itemIndex, Action<T> updateAction)
        {
        if (itemIndex < 0 || itemIndex >= list.Count)
            {
            throw new ArgumentOutOfRangeException(nameof(itemIndex));
            }

        T item = list[itemIndex];
        updateAction(item); // Update the item

        list.RemoveAt(itemIndex);
        list.Insert(0, item);
        }

    public static bool HasData<T>(this IEnumerable<T>? list)
        {
        return list != null && list.Any();
        }
    public static bool IsEmpty<T>(this IEnumerable<T>? list)
        {
        return list == null || !list.Any();
        }

    public static List<T> MoveItemToTopById<T>(this List<T> list, T? item, Func<T, int> getId)
        {
        if (item == null) return list;

        if (list == null)
            return [item];

        int targetId = getId(item);


        // Remove existing occurrences of the item (if any)
        if (list.RemoveAll(x => getId(x) == targetId) > 0)
            {
            // Insert the item at the top
            list.Insert(0, item);
            }
        return list;
        }

    }
