namespace Ryze.System.Domain.Entity
{
    public abstract class BaseEntity : IBaseEntity
    {
        protected BaseEntity()
        {
            _Insert = DateTime.Now;
            IsActive = true;
        }

        public int Id { get; set; }
        public DateTime _Insert { get; set; }
        public DateTime? _Update { get; private set; }
        public bool _Deleted { get; private set; }
        public bool IsActive { get; set; }

        protected void SetUpdate()
        {
            _Update = DateTime.Now;
        }

        protected void SetDeleted()
        {
            _Deleted = true;
        }
    }
    public interface IBaseEntity
    {
    }
}
