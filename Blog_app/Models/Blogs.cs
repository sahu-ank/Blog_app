using System.ComponentModel.DataAnnotations;

namespace Blog_app.Models
{
    public class Blogs
    {
        [Key]
        public int Id { get; set; }

        public string Tttle { get; set; }
        public string DateCreated { get; set; }

        public string DateModified { get; set; }

    }
}
