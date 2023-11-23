namespace OpenLinkedDataLibrary.Filter
{
    public static class FilterList
    {
        public static readonly string[] AvailableFilters = new string[]
        {
            "Wikidata замість DbPedia",
            "Тільки з нагородами",
            "Тільки номінанти",
            "Тільки NotableWork",
            "Тільки MemberOf",
            "Тільки займаючі посади",
            "Тільки ParticipantIn",
            "Тільки Office",
            "Тільки KnownFor",
        };

        public static bool[] StringArrayToLogicalArray(IEnumerable<string> values)
        {
            var res = new bool[AvailableFilters.Length];

            foreach (var item in values)
            {
                res[Array.IndexOf(AvailableFilters, item)] = true;
            }

            return res;
        }
    }
}