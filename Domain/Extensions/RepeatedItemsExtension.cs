namespace Domain.Extensions
{
    public static class RepeatedItemsExtension
    {
        public static string[] ChangeRepeatedItemsNames(this string[] items)
        {
            string temp = items[0];
            int repeated = 1;
            for (int i = 1; i < items.Length; i++)
            {
                if (items[i] == temp)
                {
                    temp = items[i];
                    items[i] = items[i] + repeated.ToString();
                    repeated++;
                }
                else
                {
                    repeated = 1;
                    temp = items[i];
                }
            }
            return items;
        }
    }
}
