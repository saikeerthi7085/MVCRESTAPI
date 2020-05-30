using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CO7214RestAPI4.Models
{
    
        public class Voter
    {
            public string v_id { get; set; }
        

    }
    public class Motion
    {
        public string m_id { get; set; }
   
        

    }

    public class VotingRecord
        {
           // public string v_id { get; set; }
            public string m_id { get; set; }
            public string vr_id { get; set; }
            public bool vote_cast { get; set; }
            public bool voted_yes { get; set; }
        

    }
     


    
}