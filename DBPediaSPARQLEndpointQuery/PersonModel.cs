using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBPediaSPARQLEndpointQuery
{
    public class PersonModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string BirthDate { get; set; }
        public string BirthPlace { get; set; }

        public string Child { get; set; }
        public string Subject { get; set; }
        public string Spouce { get; set; }

        public string AcademicDiscipline { get; set; }
        public string Occupation { get; set; }

        public string Founder { get; set; }

        public string Leader { get; set; }

        public string Nominee { get; set; }
        public string Awards { get; set; }

        public string Office { get; set; }
        public string KnownFor { get; set; }

        public string Writer { get; set; }

        public PersonModel()
        {
            
        }
    }
}
