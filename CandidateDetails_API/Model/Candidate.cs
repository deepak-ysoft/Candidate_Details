namespace CandidateDetails_API.Model
{
    public class Candidate
    {
        public int id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Contact_No { get; set; }
        public string Linkedin_Profile { get; set; }
        public string Email_ID { get; set; }
        public string Roles { get; set; }
        public string Experience { get; set; }
        public string Skills { get; set; }
        public decimal CTC { get; set; }
        public decimal ETC { get; set; }
        public string Notice_Period { get; set; }
        public string Current_Location { get; set; }
        public string Prefer_Location { get; set; }
        public string Reason_For_Job_Change { get; set; }
        public DateTime Schedule_Interview { get; set; }
        public string Schedule_Interview_status { get; set; }
        public string Comments { get; set; }
    }
}
