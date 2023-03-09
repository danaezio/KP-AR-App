using SQLite4Unity3d;

namespace nsDB
{
    [Table("users")]
    public class User
    {
        [PrimaryKey, AutoIncrement, Column("usr_id")]
        public int    usr_id             { get; set; }
        public int    usr_usrl_id        { get; set; }
        public string usr_name           { get; set; }
        public string usr_password       { get; set; }
        public string usr_email          { get; set; }
        public string usr_register_date  { get; set; }
        public string usr_models_path    { get; set; }
        public string usr_photo          { get; set; }
    }
}