using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain
{
    public class Image
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Url { get; set; }
        public required string PublicId { get; set; }

        //nav properties: we do this so entity framework can create the relationship 
        //and also delete the image when the user is deleted
        //We might want to do a batch process instead as it should also be deleted from cloudinary
        public required string UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!; // non-nullable reference type, so we initialize it to null!

    }
}
