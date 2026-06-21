using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Models
{
    [Table("FinalUser")]
    /// <summary>
    /// Модель пользователей
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
