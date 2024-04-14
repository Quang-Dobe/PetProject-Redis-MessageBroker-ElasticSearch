using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Data.Core
{
    public class User : Base
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Guid? DeviceId { get; set; }

        [ForeignKey(nameof(DeviceId))]
        public Device? Device { get; set; }
    }
}
