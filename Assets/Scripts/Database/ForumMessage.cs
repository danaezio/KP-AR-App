using SQLite4Unity3d;

namespace nsDB
{
    [Table("forum_messages")]
    public class ForumMessage
    {
        [PrimaryKey, AutoIncrement, Column("fome_id")]
        public int    fome_id        { get; set; }
        public int    fome_usr_id    { get; set; }
        public int    fome_foto_id   { get; set; }
        public string fome_subject   { get; set; }
        public string fome_message   { get; set; }
        public string fome_send_date { get; set; }
    }
}