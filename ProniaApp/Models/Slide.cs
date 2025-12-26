using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaApp.Models
{
    public class Slide:BaseEntity
    {
        public string Title { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public int Order { get; set; }

        [NotMapped]
        // bunun isi file inoutdan seklin adini alib image-e vermekdir, image bu sekli aparqib databazaya oz adindan elave edecek.
        public IFormFile Photo { get; set; }
    }
}