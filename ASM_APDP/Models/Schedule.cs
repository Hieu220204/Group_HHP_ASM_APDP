namespace ASM_APDP.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public string Date { get; set; } // YYYY-MM-DD
        public string Time { get; set; } // HH:mm
    }
}
