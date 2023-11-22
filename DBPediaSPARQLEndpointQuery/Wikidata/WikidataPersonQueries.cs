using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF.Query;

namespace OpenLinkedDataLibrary.Wikidata
{
    internal class WikidataPersonQueries
    {

        public static List<WikidataPersonModel> GetAll() 
        {
            var endpoint = new CustomSparqlEndPoint(new Uri("https://query.wikidata.org/sparql"));
            string sparqlQuery = @"SELECT 
?humanLabel 
?image 
?date_of_birth 
?academic_degreeLabel  
(GROUP_CONCAT(DISTINCT ?occupationText; SEPARATOR = "", "") AS ?occupations) 
(GROUP_CONCAT(DISTINCT ?participant_inText; SEPARATOR = "", "") AS ?pa) 
(GROUP_CONCAT(DISTINCT ?award_receivedText; SEPARATOR = "", "") AS ?aw) 
(GROUP_CONCAT(DISTINCT ?place_of_birthText ; SEPARATOR = "", "") AS ?pl) 
(GROUP_CONCAT(DISTINCT ?position_heldText  ; SEPARATOR = "", "") AS ?po) 
(GROUP_CONCAT(DISTINCT ?member_ofText  ; SEPARATOR = "", "") AS ?me) 
(GROUP_CONCAT(DISTINCT ?owner_ofText  ; SEPARATOR = "", "") AS ?ow) 
(GROUP_CONCAT(DISTINCT ?academic_thesisText  ; SEPARATOR = "", "") AS ?ac) 
(GROUP_CONCAT(DISTINCT ?notable_workText; SEPARATOR = "", "") AS ?no) 
(GROUP_CONCAT(DISTINCT ?nominated_forText   ; SEPARATOR = "", "") AS ?nm) 
WHERE {
  SERVICE wikibase:label { bd:serviceParam wikibase:language ""uk"", ""en"". }
  ?human wdt:P31 wd:Q5;
    wdt:P69 wd:Q84151.
  OPTIONAL { ?human wdt:P18 ?image. }
  OPTIONAL {
    ?human wdt:P106 ?occupation.
    ?occupation rdfs:label ?occupationText.
    FILTER(LANGMATCHES(LANG(?occupationText), ""uk""))
  }
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
GROUP BY ?humanLabel ?image  ?date_of_birth  ?academic_degreeLabel ";
            SparqlResultSet result = endpoint.QueryWithResultSet(sparqlQuery);
            return null;
        }

        public static List<WikidataPersonModel> GetPersonByName() 
        {
            return null;
        }
    }

    public class CustomSparqlEndPoint : SparqlRemoteEndpoint
    {
        public CustomSparqlEndPoint(Uri endpointUri) : base(endpointUri) { }

        protected override void ApplyCustomRequestOptions(HttpWebRequest httpRequest)
        {
            //httpRequest.Method = "GET";
            //httpRequest.Accept = "application/sparql-results+json";
            httpRequest.UserAgent = ".Net Client";
            base.ApplyCustomRequestOptions(httpRequest);
        }
    }
}
