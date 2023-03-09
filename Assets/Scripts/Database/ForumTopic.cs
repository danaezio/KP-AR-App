using SQLite4Unity3d;

namespace nsDB
{
    [Table("forum_topics")]
    public class ForumTopic
    {
        [PrimaryKey, AutoIncrement, Column("foto_id")]
        public int    foto_id     { get; set; }
        public string foto_name   { get; set; }
    }
}