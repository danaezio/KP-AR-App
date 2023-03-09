using SQLite4Unity3d;

namespace nsDB
{
    [Table("homework")]
    public class Homework
    {
        [PrimaryKey, AutoIncrement, Column("howo_id")]
        public int howo_id            { get; set; }
        public int howo_usr_id        { get; set; }
        public string howo_model_path { get; set; }
        public int howo_mark          { get; set; }
        public string howo_comment    { get; set; }
    }
}