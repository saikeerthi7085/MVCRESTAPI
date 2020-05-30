using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CO7214RestAPI4.Models;
using System.Web.SessionState;
using System.Web;
using System.Web.UI;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace WebRole1.Controllers
{
    [RoutePrefix("CO7214RestAPI4/rest/votingservice")]

    public class RestController : ApiController
    {
        [Route("hello")]
        //GetcreateVoter function is used to create voter with voterID by taking id as voterID
        public string GetHello()
        {
            string str = "Hello";
            return str;
            
        }

        [Route("createVoter/{voter_id}")]
        //GetcreateVoter function is used to create voter with voterID by taking id as voterID
        public bool GetcreateVoter(string voter_id)
        {

            string filename = @"\Content\sample.json";
            //get the Json file path
            string jsonFile = AppDomain.CurrentDomain.BaseDirectory + filename;
            //To read all the data from the Json file
            var Jsondata = File.ReadAllText(jsonFile);

            //retrive the data as JSONobject
            var jsonObj = JObject.Parse(Jsondata);
            //get only voter details into array
            var voterarray = jsonObj.GetValue("voter") as JArray;
            //check if there are any voter records present
            if (voterarray.Count() == 0)
            {

                string voterdetails = "{'V-id': '";
                voterdetails += voter_id;
                voterdetails += "'}";
                //convert voterdetails into JSon object
                var viddetails = JObject.Parse(voterdetails);
                // Add details to the array
                voterarray.Add(viddetails);

                //pass that array to Json object
                jsonObj["voter"] = voterarray;
                //Serialiaze the Json object with intent
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                //Add the new voterrecord to the JSon file 
                File.WriteAllText(jsonFile, output);
                return true;
            }
            //If records present check already voterid present in the Json file
            else
            {

                JArray voteridArrary = (JArray)jsonObj["voter"];
                //if voter id already present return false
                foreach (var item in
                    voteridArrary)
                {
                    if (item["V-id"].ToString() == voter_id)
                    {
                        return false;
                    }
                }

                //voter-id doesn't present in the voter details
                string voterdetails = "{'V-id': '";
                voterdetails += voter_id;
                voterdetails += "'}";
                var viddetails = JObject.Parse(voterdetails);
                voterarray.Add(viddetails);

                jsonObj["voter"] = voterarray;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, output);
                return true;
            }
        }

        [Route("createMotion/{motion_id}/{text}")]
        // create motion record in the Json file with motion id and text recieved
        public bool GetcreateMotion(string motion_id, string text)
        {
            string filename = @"\Content\sample.json";
            //get the Json file path
            string jsonFile = AppDomain.CurrentDomain.BaseDirectory + filename;
            //To read all the data from the Json file
            var Jsondata = File.ReadAllText(jsonFile);
            // convert the text data into Json data
            var jsonObj = JObject.Parse(Jsondata);
            // get the motion array details
            var motionrarray = jsonObj.GetValue("Motion") as JArray;
            //execute if motion array contains no elements
            if (motionrarray.Count() == 0)
            {

                string motiondetails = "{'M-id':'" + motion_id + "','Text':'" + text + "','votes':0,'in-favour':0}";
                var middetails = JObject.Parse(motiondetails);
                motionrarray.Add(middetails);
                jsonObj["Motion"] = motionrarray;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, output);
                return true;
            }
            else
            {
                // to check motion id is already exist
                JArray MotionidArrary = (JArray)jsonObj["Motion"];

                foreach (var item in
                    MotionidArrary)
                {
                    if (item["M-id"].ToString() == motion_id)
                    {
                        return false;
                    }
                }

                // if motion id is not present in the Motion array
                string motiondetails = "{'M-id':'" + motion_id + "','Text':'" + text + "','votes':0,'in-favour':0}";
                var middetails = JObject.Parse(motiondetails);
                motionrarray.Add(middetails);
                jsonObj["Motion"] = motionrarray;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, output);
                return true;
            }

        }
        [Route("createVotingRecord/{motion_id}/{voter_id}/{record_id}")]
        //Function to create the voting records by taking the motion id, voter id and record id
        public bool GetcreateVotingRecord(string motion_id, string voter_id, string record_id)
        {
            string filename = @"\Content\sample.json";
            //get the Json file path
            string jsonFile = AppDomain.CurrentDomain.BaseDirectory + filename;
            //To read all the data from the Json file
            var Jsondata = File.ReadAllText(jsonFile);
            var jsonObj = JObject.Parse(Jsondata);
            var votingrecarray = jsonObj.GetValue("VotingRecord") as JArray;
            // if no elements present in voting record
            if (votingrecarray.Count() == 0)
            {

                string voterrecorddetails = "{'M-id':'" + motion_id + "','V-id':'" + voter_id + "','Vr-id':'" + record_id + "','vote-cast':'False','voted-yes':'irrelevant'}";
                var vrdetails = JObject.Parse(voterrecorddetails);
                votingrecarray.Add(vrdetails);
                jsonObj["VotingRecord"] = votingrecarray;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, output);
                return true;
            }
            else
            {

                JArray MotionidArrary = (JArray)jsonObj["VotingRecord"];
                //check if Voting record is already present
                foreach (var item in
                    MotionidArrary)
                {
                    if ((item["M-id"].ToString() == motion_id && item["V-id"].ToString() == voter_id)
                        || item["Vr-id"].ToString() == record_id)
                    {
                        return false;
                    }
                }

                // if no record is present for the voterid and record it insert them 
                string voterrecorddetails = "{'M-id':'" + motion_id + "','V-id':'" + voter_id + "','Vr-id':'" + record_id + "','vote-cast':'False','voted-yes':'irrelevant'}";
                var vrdetails = JObject.Parse(voterrecorddetails);
                votingrecarray.Add(vrdetails);
                jsonObj["VotingRecord"] = votingrecarray;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, output);
                return true;
            }

        }
        [Route("vote/{voter_id}/{record_id}/{vote}")]
        //To update the voter record with the data
        public bool Getvote(string voter_id, string record_id, bool vote)
        {
            string filename = @"\Content\sample.json";
            //get the Json file path
            string jsonFile = AppDomain.CurrentDomain.BaseDirectory + filename;
            //To read all the data from the Json file
            var Jsondata = File.ReadAllText(jsonFile);
            var jsonObj = JObject.Parse(Jsondata);
            //to check voting record is present or not
            var Motionobj = jsonObj["VotingRecord"];
            var motionrarray = jsonObj.GetValue("VotingRecord") as JArray;
            if (motionrarray.Count() != 0)
            {
                
                JArray votingRecordArrary = (JArray)jsonObj["VotingRecord"];
                JArray MotionArrary = (JArray)jsonObj["Motion"];
                foreach (var item in
                    votingRecordArrary)
                {
                    if (item["V-id"].ToString() == voter_id && item["Vr-id"].ToString() == record_id && item["vote-cast"].Value<bool>() == false)
                    {
                        item["vote-cast"] = true;
                        item["voted-yes"] = vote;
                        //update the motion according to the vote data
                        foreach (var motionitem in MotionArrary.Where(obj => obj["M-id"].Value<string>() == item["M-id"].ToString()))
                        {
                            motionitem["votes"] = Convert.ToInt32(motionitem["votes"]) + 1;
                            if (vote == true)
                            {
                                motionitem["in-favour"] = Convert.ToInt32(motionitem["in-favour"]) + 1;

                            }
                            jsonObj["Motion"] = MotionArrary;


                        }
                        jsonObj["VotingRecord"] = votingRecordArrary;
                        string output1 = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(jsonFile, output1);
                        return true;
                    }


                }
                return false;


            }
            return false;
        }

        [Route("getMotion/{motion_id}")]
        //to get the motion details
        public Motion GetMotion(string motion_id)
        {
            string filename = @"\Content\sample.json";
            //get the Json file path
            string jsonFile = AppDomain.CurrentDomain.BaseDirectory + filename;
            //To read all the data from the Json file
            var Jsondata = File.ReadAllText(jsonFile);
            var jsonObj = JObject.Parse(Jsondata);

            Motion motionobj = new Motion();
            var Motionobj = jsonObj["Motion"];
            var motionrarray = jsonObj.GetValue("Motion") as JArray;
            //to check if there are elements in the motion array or not
            if (motionrarray.Count() != 0)
            {

                JArray MotionArrary = (JArray)jsonObj["Motion"];
                // retrive the details and add to the motion obj 
                foreach (var motionitem in MotionArrary.Where(obj => obj["M-id"].Value<string>() == motion_id))
                {

                    motionobj.m_id = motionitem["M-id"].Value<string>();
                   
                    
                    return motionobj;
                }
                
                
                return motionobj;
            }
            
            return motionobj;
        }
        [Route("getVoter/{voter_id}")]
        //To get all the voter details 
        public Voter GetVoter(string voter_id)
        {
            string filename = @"\Content\sample.json";
            //get the Json file path
            string jsonFile = AppDomain.CurrentDomain.BaseDirectory + filename;
            //To read all the data from the Json file
            var Jsondata = File.ReadAllText(jsonFile);
            var jsonObj = JObject.Parse(Jsondata);

            Voter votermodelobj = new Voter();
            var VoterJsonobj = jsonObj["voter"];
            var Voterrarray = jsonObj.GetValue("voter") as JArray;
            //check if any records present in the voter array or not
            if (Voterrarray.Count() != 0)
            {

                JArray voterrarray = (JArray)jsonObj["voter"];

                foreach (var voteritem in voterrarray.Where(obj => obj["V-id"].Value<string>() == voter_id))
                {

                    votermodelobj.v_id = voteritem["V-id"].Value<string>();

                    
                    return votermodelobj;
                }
               
                
                return votermodelobj;
            }
            
            return votermodelobj;
        }
        [Route("getVotingRecordsForVoter/{voter_id}")]

        // to get the set of voter records present
        public dynamic GetVoterRecord(string voter_id)
        {
            
            string filename = @"\Content\sample.json";
            //get the Json file path
            string jsonFile = AppDomain.CurrentDomain.BaseDirectory + filename;
            //To read all the data from the Json file
            var Jsondata = File.ReadAllText(jsonFile);
            var jsonObj = JObject.Parse(Jsondata);
            VotingRecord votermodelobj = new VotingRecord();
            List<VotingRecord> votingrecordlist = new List<VotingRecord>();
            var VoterrecJsonobj = jsonObj["VotingRecord"];
            var ResponseJsonobj = jsonObj["Voting Record"];
            JArray responsearray = (JArray)jsonObj["Voting Record"];
            var Voterrecarray = jsonObj.GetValue("VotingRecord") as JArray;
            if (Voterrecarray.Count() != 0)
            {

                JArray voterrarray = (JArray)jsonObj["VotingRecord"];

                foreach (var voterrecitem in voterrarray.Where(obj => obj["V-id"].Value<string>() == voter_id))
                {

                    //votermodelobj.v_id = voterrecitem["V-id"].Value<string>();
                    votermodelobj.m_id = voterrecitem["M-id"].Value<string>();
                    votermodelobj.vr_id = voterrecitem["Vr-id"].Value<string>();
                    votermodelobj.vote_cast = voterrecitem["vote-cast"].Value<bool>();
                    votermodelobj.voted_yes = voterrecitem["voted-yes"].Value<bool>();

                    var responseobj = Newtonsoft.Json.JsonConvert.SerializeObject(votermodelobj);
                    var responseobj1 = JObject.Parse(responseobj);
                    responsearray.Add(responseobj1);
                    
                }

                jsonObj["Voting Record"] = responsearray;
            }
            var Voterrecarray1 = jsonObj.GetValue("Voting Record") as JArray;

            var obj1 = "{'VotingRecord': " + Voterrecarray1 + "}";
            var str = JObject.Parse(obj1);
            //string responseobj2 = Newtonsoft.Json.JsonConvert.SerializeObject(str);
           


            return str;



        }
        [Route("clearDatabase")]
        // To clear the voter records and motion class with vote cast records to zero
        public bool GetclearDatabase()
        {
            string filename = @"\Content\sample.json";
            //get the Json file path
            string jsonFile = AppDomain.CurrentDomain.BaseDirectory + filename;
            //To read all the data from the Json file
            var Jsondata = File.ReadAllText(jsonFile);
            var jsonObj = JObject.Parse(Jsondata);
           
                JArray voterrarray = (JArray)jsonObj["VotingRecord"];
                JArray MotionArrary = (JArray)jsonObj["Motion"];
                JArray voteArrary = (JArray)jsonObj["voter"];
                voterrarray.RemoveAll();
                MotionArrary.RemoveAll();
                voteArrary.RemoveAll();
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, output);
                return true;

        }

    }
}
