using SQLite4Unity3d;

namespace nsDB
{
    [Table("categories")]
    public class Category
    {
        [PrimaryKey, AutoIncrement, Column("cat_id")]
        public int    cat_id   { get; set; }
        public string cat_name { get; set; }
    }
}