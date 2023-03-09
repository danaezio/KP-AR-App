using SQLite4Unity3d;

namespace nsDB
{
    [Table("models")]
    public class Model
    {
        [PrimaryKey, AutoIncrement, Column("mod_id")]
        public int    mod_id          { get; set; }
        public int    mod_cat         { get; set; }
        public string mod_name        { get; set; }
        public string mod_path        { get; set; }
    }
}