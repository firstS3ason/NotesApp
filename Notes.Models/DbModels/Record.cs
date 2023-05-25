using System.ComponentModel.DataAnnotations;

namespace Notes.Models.DbModels
{
    public class Record
    {
        [Key]
        public int id { get; set; }
        public string? recordName { get; set; }
        public string? recordType { get; set; }
        public string? recordInfo { get; set; }
        public DateTime recordCreationTime { get; set; }

        public Record() { }
        public Record( string? _recordName, string? _recordType, string? _recordInfo, DateTime _recordCreationTime)
            => ( recordName, recordType, recordInfo, recordCreationTime) = ( _recordName, _recordType, _recordInfo, _recordCreationTime);
    }
}
