using System;

namespace Project.Common.Types
{
    public interface IIdentifiable
    {
        Guid EntityId { get; set; }
    }
}
