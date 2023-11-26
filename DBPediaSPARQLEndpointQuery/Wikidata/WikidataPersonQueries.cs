using AngleSharp.Common;
using DBPediaSPARQLEndpointQuery;
using OpenLinkedDataLibrary.DBPedia;
using System.Data;
using System.Net;
using System.Text.RegularExpressions;
using VDS.RDF.Query;

namespace OpenLinkedDataLibrary.Wikidata
{
    public class WikidataPersonQueries
    {
        public static List<PersonModel> personList = new List<PersonModel>();
        public static List<PersonModel> GetAll()
        {
            var result = new List<PersonModel>();
            if (personList.Count != 0) 
            {
                return personList;
            }
            var endpoint = new CustomSparqlEndPoint(new Uri("https://query.wikidata.org/sparql"));
            string sparqlQuery =
                @"SELECT 
                ?humanLabel 
                (SAMPLE(?date_of_birth) AS ?birthDate)
                ?academic_degreeLabel  
                (SAMPLE(?image) AS ?sampleImage)
                (GROUP_CONCAT(DISTINCT ?participant_inText; SEPARATOR = "", "") AS ?pa) 
                (GROUP_CONCAT(DISTINCT ?award_receivedText; SEPARATOR = "", "") AS ?aw) 
                (GROUP_CONCAT(DISTINCT ?place_of_birthText ; SEPARATOR = "", "") AS ?pl) 
                (GROUP_CONCAT(DISTINCT ?position_heldText  ; SEPARATOR = "", "") AS ?po) 
                (GROUP_CONCAT(DISTINCT ?member_ofText  ; SEPARATOR = "", "") AS ?me) 
                (GROUP_CONCAT(DISTINCT ?notable_workText; SEPARATOR = "", "") AS ?no) 
                (GROUP_CONCAT(DISTINCT ?nominated_forText   ; SEPARATOR = "", "") AS ?nm) 
                WHERE {
                SERVICE wikibase:label { bd:serviceParam wikibase:language ""uk"", ""en"". }
                ?human wdt:P31 wd:Q5;
                wdt:P69 wd:Q84151.
                OPTIONAL { ?human wdt:P18 ?image. }

                OPTIONAL {
                ?human wdt:P512 ?academic_degree. 
                ?academic_degree rdfs:label ?academic_degreeText.
                FILTER(LANGMATCHES(LANG(?academic_degreeText), ""uk""))
                }
                OPTIONAL { 
                ?human wdt:P1344 ?participant_in. 
                ?participant_in rdfs:label ?participant_inText.
                FILTER(LANGMATCHES(LANG(?participant_inText), ""uk""))
                }
                OPTIONAL { 
                ?human wdt:P166 ?award_received. 
                ?award_received rdfs:label ?award_receivedText.
                FILTER(LANGMATCHES(LANG(?award_receivedText), ""uk""))
                }
                OPTIONAL {
                ?human wdt:P569 ?date_of_birth.
                }
                OPTIONAL { 
                ?human wdt:P19 ?place_of_birth.
                ?place_of_birth rdfs:label ?place_of_birthText.
                FILTER(LANGMATCHES(LANG(?place_of_birthText), ""uk""))
                }
                OPTIONAL { 
                ?human wdt:P39 ?position_held. 
                ?position_held rdfs:label ?position_heldText.
                FILTER(LANGMATCHES(LANG(?position_heldText), ""uk""))
                }
                OPTIONAL { 
                ?human wdt:P463 ?member_of.
                ?member_of rdfs:label ?member_ofText.
                FILTER(LANGMATCHES(LANG(?member_ofText), ""uk""))
                }
                OPTIONAL { 
                ?human wdt:P1830 ?owner_of. 
                ?owner_of rdfs:label ?owner_ofText.
                FILTER(LANGMATCHES(LANG(?owner_ofText), ""uk""))
                }
                OPTIONAL { 
                ?human wdt:P1026 ?academic_thesis.
                ?academic_thesis rdfs:label ?academic_thesis.
                FILTER(LANGMATCHES(LANG(?academic_thesis), ""uk""))
                }
                OPTIONAL { 
                ?human wdt:P800 ?notable_work. 
                ?notable_work rdfs:label ?notable_workText.
                FILTER(LANGMATCHES(LANG(?notable_workText), ""uk""))
                }
                OPTIONAL { 
                ?human wdt:P1411 ?nominated_for. 
                ?nominated_for rdfs:label ?nominated_forText.
                FILTER(LANGMATCHES(LANG(?nominated_forText), ""uk""))
                }
                }
                GROUP BY ?humanLabel ?sampleImage  ?birthDate  ?academic_degreeLabel 
";
            SparqlResultSet dataset = endpoint.QueryWithResultSet(sparqlQuery);
            foreach (var item in dataset)
            {
                result.Add(FromRDFToPersonModel(item));
            }
            personList = result;
            return result;
        }

        public static SearchStatusEnum GetPersonByName(string name, out List<PersonModel> model)
        {
            model = new List<PersonModel>();
            var rset = personList;
            if (personList.Count == 0)
            {
                rset = GetAll();
            }
            model = rset.Where(X => Regex.IsMatch(X.Name, name)).ToList();
            if (model.Count != 0)
                return SearchStatusEnum.Success;
            return SearchStatusEnum.No_matches;
        }
        public static PersonModel FromRDFToPersonModel(ISparqlResult item)
        {
            var person = new List<string>();
            for (int i = 0; i < item.Count; i++)
            {
                var a = item[i].ToDictionary();
                string f = string.Empty;
                a.TryGetValue("Value", out f);
                if (f == null) a.TryGetValue("Uri", out f);
                if (string.IsNullOrEmpty(f)) f = "N/A";
                person.Add(f);
            }
            var model = new PersonModel();
            model.Name = person[0];
            model.BirthDate = person[1].Substring(0, Math.Max(0, person[1].Length - 10));
            model.AcademicDegree = DBPediaPersonQueries.ConcatStrings(DBPediaPersonQueries.GetUniqueStrings(person[2]));
            model.Img = person[3];
            model.ParticipantIn = DBPediaPersonQueries.ConcatStrings(DBPediaPersonQueries.GetUniqueStrings(person[4]));
            model.Awards = DBPediaPersonQueries.ConcatStrings(DBPediaPersonQueries.GetUniqueStrings(person[5]));
            model.BirthPlace = DBPediaPersonQueries.ConcatStrings(DBPediaPersonQueries.GetUniqueStrings(person[6]));
            model.PositionHeld = DBPediaPersonQueries.ConcatStrings(DBPediaPersonQueries.GetUniqueStrings(person[7]));
            model.MemberOf = DBPediaPersonQueries.ConcatStrings(DBPediaPersonQueries.GetUniqueStrings(person[8]));
            model.NotableWork = DBPediaPersonQueries.ConcatStrings(DBPediaPersonQueries.GetUniqueStrings(person[9]));
            model.NominatedFor = DBPediaPersonQueries.ConcatStrings(DBPediaPersonQueries.GetUniqueStrings(person[10]));
            return model;
        }
    }

    public class CustomSparqlEndPoint : SparqlRemoteEndpoint
    {
        public CustomSparqlEndPoint(Uri endpointUri) : base(endpointUri) { }

        protected override void ApplyCustomRequestOptions(HttpWebRequest httpRequest)
        {
            httpRequest.UserAgent = ".Net Client";
            base.ApplyCustomRequestOptions(httpRequest);
        }
    }
}
