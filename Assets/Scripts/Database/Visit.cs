using SQLite4Unity3d;

namespace nsDB
{
    [Table("visits")]
    public class Visit
    {
        [PrimaryKey, AutoIncrement, Column("vis_id")]
        public int    vis_id             { get; set; }
        public int    vis_usr_id         { get; set; }
        public string vis_entry_date     { get; set; }
        public string vis_exit_date      { get; set; }
    }
}