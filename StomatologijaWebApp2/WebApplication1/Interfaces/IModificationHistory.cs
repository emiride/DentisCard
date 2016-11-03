using System;

namespace WebApplication1.Interfaces
{
    public interface IModificationHistory
    {
        DateTime DateCreated { get; set; }
        DateTime? DateModified { get; set; }
    }
}
