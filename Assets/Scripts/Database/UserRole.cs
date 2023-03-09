using SQLite4Unity3d;

namespace nsDB
{
    [Table("user_roles")]
    class UserRole
    {
        [PrimaryKey, AutoIncrement, Column("usrl_id")]
        public int    usrl_id   { get; set; }
        public string usrl_name { get; set; }
    }
}