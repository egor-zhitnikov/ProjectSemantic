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
                return ApplyFilter(WikidataPersonQueries.GetAll(), filters);
            }
            else
            {
                return ApplyFilter(DBPediaPersonQueries.GetAll(), filters);
            }
        }

        public static List<PersonModel> ApplyFilter(List<PersonModel> personsList, bool[] filters)
        {
            if (filters[1])
            {
                personsList = personsList.Where(x => IsValid(x.Awards)).ToList();
            }
            if (filters[2])
            {
                personsList = personsList.Where(x => IsValid(x.NominatedFor)).ToList();
            }
            if (filters[3])
            {
                personsList = personsList.Where(x => IsValid(x.NotableWork)).ToList();
            }
            if (filters[4])
            {
                personsList = personsList.Where(x => IsValid(x.MemberOf)).ToList();
            }
            if (filters[5])
            {
                personsList = personsList.Where(x => IsValid(x.PositionHeld)).ToList();
            }
            if (filters[6])
            {
                personsList = personsList.Where(x => IsValid(x.ParticipantIn)).ToList();
            }
            if (filters[7])
            {
                personsList = personsList.Where(x => IsValid(x.Office)).ToList();
            }
            if (filters[8])
            {
                personsList = personsList.Where(x => IsValid(x.KnownFor)).ToList();
            }
            return personsList;
        }

        public static bool IsValid(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            if (s == "N/A") return false;

            return true;
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