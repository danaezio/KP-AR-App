using SQLite4Unity3d;

namespace nsDB
{
    [Table("favorite_topics")]
    public class FavoriteTopic
    {
        [PrimaryKey, AutoIncrement, Column("fato_id")]
        public int fato_id     { get; set; }
        public int fato_usr_id { get; set; }
        public int fato_top_id { get; set; }
    }
}