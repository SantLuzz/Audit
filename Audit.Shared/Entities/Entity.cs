using Audit.Shared.Notifiables;

namespace Audit.Shared.Entities
{
    public abstract class Entity : Notifiable
    {
        public Guid Id { get; private set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        protected Entity(Guid id)
        {
            Id = id;
        }
    }
}
