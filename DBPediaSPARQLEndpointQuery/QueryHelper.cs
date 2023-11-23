using DBPediaSPARQLEndpointQuery;
using OpenLinkedDataLibrary.DBPedia;
using OpenLinkedDataLibrary.Wikidata;

namespace OpenLinkedDataLibrary
{
    public static class QueryHelper
    {
        public static List<PersonModel> GetAll(bool[] filters)
        {
            if (filters[0])
            {
                return WikidataPersonQueries.GetAll();
            }
            else
            {
                return DBPediaPersonQueries.GetAll();
            }
        }

        public static SearchStatusEnum GetPersonByName(string name, bool[] filters, out List<PersonModel> model)
        {
            if (filters[0])
            {
                return WikidataPersonQueries.GetPersonByName(name, out model);
            }
            else
            {
                return DBPediaPersonQueries.GetPersonByName(name, out model);
            }
        }
    }
}