using SQLite4Unity3d;

namespace nsDB
{
    [Table("topic_views")]
    public class TopicView
    {
        [PrimaryKey, AutoIncrement, Column("tovi_id")]
        public int    tovi_id                          { get; set; }
        public int    tovi_usr_id                      { get; set; }
        public int    tovi_top_id                      { get; set; }
        public int    tovi_view_count                  { get; set; }
        public int    tovi_video_view_time             { get; set; }
    }
}