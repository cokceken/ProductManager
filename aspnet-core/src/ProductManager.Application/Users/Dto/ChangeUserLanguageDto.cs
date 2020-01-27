using System.ComponentModel.DataAnnotations;

namespace ProductManager.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}