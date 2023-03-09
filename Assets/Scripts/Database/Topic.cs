using SQLite4Unity3d;

namespace nsDB
{
    [Table("topics")]
    public class Topic
    {
        [PrimaryKey, AutoIncrement, Column("top_id")]
        public int    top_id                          { get; set; }
        public int    top_cat_id                      { get; set; }
        public string top_name                        { get; set; }
        public string top_video_url                   { get; set; }
        public string top_description                 { get; set; }
        public string top_additional_information_link { get; set; }
    }
}